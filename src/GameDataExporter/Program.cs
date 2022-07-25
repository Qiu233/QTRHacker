using Microsoft.CSharp;
using QHackCLR.Common;
using QHackCLR.DataTargets;
using QHackLib;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDataExporter
{
	class Program
	{
		private static readonly Dictionary<string, string> TypeRedefs = new()
		{
			{ "Microsoft.Xna.Framework.Color", "ValueTypeRedefs.Xna.Color" },
			{ "Microsoft.Xna.Framework.Rectangle", "ValueTypeRedefs.Xna.Rectangle" },
			{ "Microsoft.Xna.Framework.Vector2", "ValueTypeRedefs.Xna.Vector2" },
			{ "Microsoft.Xna.Framework.Point", "ValueTypeRedefs.Xna.Point" },
			{ "Terraria.BitsByte", "ValueTypeRedefs.Terraria.BitsByte" },
		};

		static string GetTypeName(string type)
		{
			if (TypeRedefs.TryGetValue(type, out string v))
				return v;
			string name;
			using CSharpCodeProvider provider = new();
			name = provider.GetTypeOutput(new CodeTypeReference(type));
			return name;
		}
		static void WriteTypeInfo(string file, ClrType type)
		{
			StringBuilder sb = new StringBuilder();
			StringWriter sw = new StringWriter(sb);
			sw.WriteLine($"Type: {type.Name}");
			sw.Write($"\t");
			ClrType baseType = type.BaseType;
			while (baseType != null)
			{
				sw.Write($"->{baseType.Name}");
				baseType = baseType.BaseType;
			}
			sw.WriteLine();
			sw.WriteLine($"Methods:");
			type.MethodsInVTable.ToList().ForEach(t =>
			{
				sw.WriteLine($"{t.Signature}");
			});
			sw.WriteLine();
			sw.WriteLine($"Fields:");
			var fields = type.Fields.ToList();
			fields.Sort((f1, f2) =>
			{
				int result = string.Compare(f1.Type.Name, f2.Type.Name);
				if (result == 0)
					result = string.Compare(f1.DeclaringType.Name, f2.DeclaringType.Name);
				if (result == 0)
					result = string.Compare(f1.Name, f2.Name);
				return result;
			});
			fields.ForEach(t =>
			{
				sw.WriteLine(string.Format("|Name: {0,-20}|Type: {1,-40}|From: {2}", t.Name, t.Type?.Name, t.DeclaringType));
			});
			sw.Close();
			File.WriteAllText(file, sb.ToString());
		}
		static void WriteTypeTT(string file, ClrType type)
		{
			StringBuilder sb = new StringBuilder();
			StringWriter sw = new StringWriter(sb);
			var fields = type.Fields.Where(t => t.DeclaringType == type).ToList();
			fields.Sort((f1, f2) =>
			{
				int result = string.Compare(f1.Type.Name, f2.Type.Name);
				if (result == 0)
					result = string.Compare(f1.Name, f2.Name);
				return result;
			});
			fields.ForEach(t =>
			{
				string typeName;
				using (var provider = new CSharpCodeProvider())
					typeName = provider.GetTypeOutput(new CodeTypeReference(t.Type.Name));
				if (t.Type.IsArray)
				{
					var ct = t.Type.ComponentType;
					typeName = GetTypeName(ct.Name);
					if (t.Type.Rank == 1)
					{
						if (ct.IsValueType)
							sw.Write(string.Format("\t\t<# PROPERTY_ARRAYV_VIRTUAL(\"{0,-10}\", \"{1,-20}\"); #>\r\n", typeName, t.Name));
						else
							sw.Write(string.Format("\t\t<# PROPERTY_ARRAY_VIRTUAL(\"{0,-10}\", \"{1,-20}\"); #>\r\n", typeName, t.Name));
					}
					else if (t.Type.Rank == 2)
					{
						if (ct.IsValueType)
							sw.Write(string.Format("\t\t<# PROPERTY_ARRAY2DV_VIRTUAL(\"{0,-10}\", \"{1,-20}\"); #>\r\n", typeName, t.Name));
						else
							sw.Write(string.Format("\t\t<# PROPERTY_ARRAY2D_VIRTUAL(\"{0,-10}\", \"{1,-20}\"); #>\r\n", typeName, t.Name));
					}
					else
					{
						if (ct.IsValueType)
							sw.Write(string.Format("\t\t<# PROPERTY_ARRAYMDV_VIRTUAL(\"{0,-10}\", \"{1,-20}\"); #>\r\n", typeName, t.Name));
						else
							sw.Write(string.Format("\t\t<# PROPERTY_ARRAYMD_VIRTUAL(\"{0,-10}\", \"{1,-20}\"); #>\r\n", typeName, t.Name));
					}
				}
				else if (t.Type == Runtime.Heap.StringType)
				{
					sw.Write(string.Format("\t\t<# PROPERTY_STR_VIRTUAL(\"{0,-10}\"); #>\r\n", t.Name));
				}
				else
				{
					if (TypeRedefs.TryGetValue(typeName, out string v))
						sw.Write(string.Format("\t\t<# PROPERTY_VIRTUAL(\"{0,-10}\", \"{1,-20}\"); #>\r\n", v, t.Name));
					else
						sw.Write(string.Format("\t\t<# PROPERTY_VIRTUAL(\"{0,-10}\", \"{1,-20}\"); #>\r\n", typeName, t.Name));
				}
			});
			sw.Close();
			File.WriteAllText(file, sb.ToString());
		}
		static void WriteType(string file, ClrType type)
		{
			WriteTypeInfo(file + ".txt", type);
			WriteTypeTT(file + ".tt", type);
		}
		static void WriteTypes(ClrModule module)
		{
			foreach (var type in module.DefinedTypes)
			{
				string[] path = type.Name.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);
				string cur = "./Types";
				bool flag = false;
				for (int i = 0; i < path.Length; i++)
				{
					string sec = path[i];
					if (!Directory.Exists(cur))
						Directory.CreateDirectory(cur);
					try
					{
						cur = Path.Combine(cur, sec);
					}
					catch
					{
						Console.WriteLine($"Skipped: {type.Name}");
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					try
					{
						WriteType(cur, type);
					}
					catch
					{
						Console.WriteLine($"Skipped: {type.Name}");
					}
				}
			}
		}
		private static ClrRuntime Runtime;
		static void Main(string[] args)
		{
			var ps = Process.GetProcesses().Where(t =>
			{
				if (!string.Equals(t.ProcessName, "dotnet", StringComparison.OrdinalIgnoreCase))
					return false;
				using QHackContext ctx = QHackContext.Create(t.Id);
				return ctx.CLRHelpers.Where(t => string.Equals(t.Key.Name, "tModLoader", StringComparison.OrdinalIgnoreCase)).Any();
			}).ToArray();
			if (ps.Length == 0)
			{
				Console.WriteLine("Please be sure that you have launched tModLoader");
				return;
			}
			var id = ps[0].Id;
			DataTarget dataTarget = new(id);
			Runtime = dataTarget.ClrVersions[0].CreateRuntime();
			ClrModule module = Runtime.AppDomain.Modules.First(t => t.Name == "tModLoader");
			WriteTypes(module);
		}
	}
}
