using Microsoft.CSharp;
using QHackCLR.Common;
using QHackCLR.DataTargets;
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
				sw.WriteLine(string.Format("\t\t<# PROPERTY_VIRTUAL(\"{0,-10}\", \"{1,-20}\"); #>", typeName, t.Name));
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
		static void Main(string[] args)
		{
			var id = Process.GetProcessesByName("Terraria")[0].Id;
			DataTarget dataTarget = new(id);
			ClrRuntime runtime = dataTarget.ClrVersions[0].CreateRuntime();
			ClrModule module = runtime.AppDomain.Modules.First(t => t.Name == "Terraria");
			WriteTypes(module);
		}
	}
}
