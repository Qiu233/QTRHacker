using QHackCLR.Common;
using QTRHacker.Core;
using QTRHacker.Core.GameObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace QTRHacker.Functions.Test;

// A read-only check against the running game's CLR metadata. No game methods,
// hooks or setters are executed, so this also works from the title screen.
internal static class GameCompatibility
{
	public static void Verify(string repository)
	{
		string source = Path.Combine(Path.GetFullPath(repository), "src");
		if (!Directory.Exists(Path.Combine(source, "QTRHacker.Core")))
			throw new DirectoryNotFoundException("Pass the repository directory after --verify-game-compatibility.");
		using var context = GameContext.OpenGame(Process.GetProcessesByName("Terraria").First());
		var helper = context.GameModuleHelper;
		var errors = new List<string>();
		var signatures = new HashSet<string>();
		var namedMethods = new HashSet<string>();
		var fields = new HashSet<string>();
		var types = new Dictionary<string, ClrType>();
		ClrType GameType(string name)
		{
			if (!types.TryGetValue(name, out var type))
				types[name] = type = helper.GetClrType(name);
			return type ?? throw new InvalidOperationException("Missing game type: " + name);
		}
		void Field(string type, string name, string expectedType = null)
		{
			fields.Add(type + "." + name);
			var field = GameType(type).Fields.FirstOrDefault(f => f.Name == name);
			if (field == null)
				errors.Add("Missing field: " + type + "." + name);
			else if (expectedType != null && (expectedType.EndsWith("[*]")
				? !field.Type.IsArray || field.Type.Rank < 3 || field.Type.ComponentType.Name != expectedType.Substring(0, expectedType.Length - 3)
				: field.Type.Name != expectedType))
				errors.Add($"Field type: {type}.{name}: expected {expectedType}, found {field.Type.Name}");
		}

		foreach (string project in new[] { "QTRHacker.Core", "QTRHacker" })
		foreach (string file in Directory.EnumerateFiles(Path.Combine(source, project), "*.cs", SearchOption.AllDirectories))
		{
			if (file.Split(Path.DirectorySeparatorChar).Any(p => p == "obj" || p == "bin"))
				continue;
			string text = WithoutComments(File.ReadAllText(file));
			foreach (Match match in Regex.Matches(text, "\"(Terraria\\.[^\"\\r\\n]+\\([^\"\\r\\n]*\\))\""))
			{
				string signature = match.Groups[1].Value;
				if (!signatures.Add(signature))
					continue;
				string method = signature.Substring(0, signature.IndexOf('('));
				string type = method.Substring(0, method.LastIndexOf('.'));
				if (!GameType(type).MethodsInVTable.Any(m => m.Signature == signature))
					errors.Add("Missing method: " + signature);
			}
			foreach (Match match in Regex.Matches(text,
				"(?:GetFunctionAddress|GetClrMethod)\\(\\s*(?:ctx,\\s*)?\"(Terraria\\.[^\"]+)\",\\s*\"([A-Za-z_]\\w*)\""))
			{
				string type = match.Groups[1].Value, method = match.Groups[2].Value;
				if (namedMethods.Add(type + "." + method) && !GameType(type).MethodsInVTable.Any(m => m.Name == method))
					errors.Add("Missing named method: " + type + "." + method);
			}
			foreach (Match match in Regex.Matches(text,
				"(?:GetStatic\\w+|SetStatic\\w+|GetOffset)(?:<[^>]+>)?\\(\\s*(?:ctx,\\s*)?\"(Terraria\\.[^\"]+)\",\\s*\"([^\"]+)\""))
				Field(match.Groups[1].Value, match.Groups[2].Value);

			string wrapperRoot = Path.Combine(source, "QTRHacker.Core", "GameObjects", "Terraria") + Path.DirectorySeparatorChar;
			if (!file.StartsWith(wrapperRoot, StringComparison.OrdinalIgnoreCase))
				continue;
			string wrapperName = Path.GetRelativePath(wrapperRoot, file).Replace(".ps.cs", "").Replace(".cs", "").Replace(Path.DirectorySeparatorChar, '.');
			Type wrapper = typeof(GameObject).Assembly.GetType("QTRHacker.Core.GameObjects.Terraria." + wrapperName);
			if (wrapper == null || !typeof(GameObject).IsAssignableFrom(wrapper))
				continue;
			string gameName = "Terraria." + wrapperName;
			string expectedBase = wrapper.BaseType == typeof(GameObject) ? "System.Object" : "Terraria." + wrapper.BaseType.Name;
			if (wrapper.BaseType != typeof(GameObject) && GameType(gameName).BaseType.Name != expectedBase)
				errors.Add("Wrong wrapper base: " + gameName);
			foreach (Match match in Regex.Matches(text, @"(?<!Typed)InternalObject\.([a-zA-Z_]\w*)"))
				Field(gameName, match.Groups[1].Value);
		}

		var typeNames = new Dictionary<string, string>
		{
			["bool"] = "System.Boolean", ["byte"] = "System.Byte", ["sbyte"] = "System.SByte",
			["short"] = "System.Int16", ["ushort"] = "System.UInt16", ["int"] = "System.Int32",
			["uint"] = "System.UInt32", ["long"] = "System.Int64", ["ulong"] = "System.UInt64",
			["float"] = "System.Single", ["double"] = "System.Double", ["GameString"] = "System.String",
		};
		foreach (string name in new[] { "Item", "NPC", "Player" })
		foreach (string line in File.ReadLines(Path.Combine(source, "QTRHacker.Core", "GameObjects", "Terraria", name + "Properties")))
		{
			Match macro = Regex.Match(line, @"(PROPERTY_\w+)\((.*)\)");
			if (!macro.Success)
				continue;
			string[] args = Regex.Matches(macro.Groups[2].Value, "\"([^\"]*)\"").Select(m => m.Groups[1].Value.Trim()).ToArray();
			string type = args[0];
			if (typeNames.TryGetValue(type, out string clrName))
				type = clrName;
			type = type.Replace("ValueTypeRedefs.Xna.", "Microsoft.Xna.Framework.").Replace("ValueTypeRedefs.Terraria.", "Terraria.");
			if (macro.Groups[1].Value.Contains("ARRAYMD"))
				type += "[*]";
			else if (macro.Groups[1].Value.Contains("ARRAY2D"))
				type += "[,]";
			else if (macro.Groups[1].Value.Contains("ARRAY"))
				type += "[]";
			Field("Terraria." + name, args.Last(), type);
		}

		foreach (string error in errors.Distinct())
			Console.Error.WriteLine(error);
		if (errors.Count != 0)
			throw new InvalidOperationException($"Game compatibility check failed: {errors.Distinct().Count()} errors.");
		Console.WriteLine($"Verified {signatures.Count} method signatures, {namedMethods.Count} named methods and {fields.Count} fields against the running game; template types and wrapper base classes match.");
	}

	private static string WithoutComments(string text) => Regex.Replace(text,
		"(@\"(?:\"\"|[^\"])*\"|\"(?:\\\\.|[^\"\\\\])*\")|//[^\\r\\n]*|/\\*[\\s\\S]*?\\*/",
		m => m.Groups[1].Success ? m.Value : "");
}
