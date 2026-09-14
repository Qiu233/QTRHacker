using QHackLib;
using QHackCLR.Common;
using QHackCLR.DataTargets;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace QTRHacker.Functions.Test;

internal static class DacAddressChecks
{
	public static void Verify(string targetExecutable)
	{
		VerifyAddressForms();
		// Patch only a temporary copy of the fixture's PE flag, so x86 Windows lets
		// its CLR allocate above 2 GiB. No application/game binary is modified.
		string directory = Path.Combine(AppContext.BaseDirectory, "clr-high-" + Guid.NewGuid().ToString("N"));
		Directory.CreateDirectory(directory);
		string executable = Path.Combine(directory, Path.GetFileName(targetExecutable));
		byte[] pe = File.ReadAllBytes(targetExecutable);
		int header = BitConverter.ToInt32(pe, 0x3C);
		if (BitConverter.ToUInt32(pe, header) != 0x4550 || BitConverter.ToUInt16(pe, header + 4) != 0x14C)
			throw new InvalidOperationException("Expected an x86 PE fixture.");
		pe[header + 22] |= 0x20; // IMAGE_FILE_LARGE_ADDRESS_AWARE
		File.WriteAllBytes(executable, pe);
		if (File.Exists(targetExecutable + ".config")) File.Copy(targetExecutable + ".config", executable + ".config");
		using var process = Process.Start(new ProcessStartInfo(executable, "--high-addresses")
		{
			UseShellExecute = false, CreateNoWindow = true,
			RedirectStandardInput = true, RedirectStandardOutput = true, RedirectStandardError = true,
		});
		var errors = process.StandardError.ReadToEndAsync();
		try
		{
			var ready = process.StandardOutput.ReadLineAsync();
			if (!ready.Wait(TimeSpan.FromSeconds(30)) || ready.Result != process.Id.ToString())
				throw new InvalidOperationException("High-address fixture did not initialize: " + (errors.IsCompleted ? errors.Result : "timeout"));
			Console.WriteLine($"High-address CLR fixture ready: PID={process.Id}");
			using var context = QHackContext.Create(process.Id);
			for (int pass = 0; pass < 2; pass++)
			{
				var modules = context.Runtime.AppDomain.Modules.Where(m => m.Name == "QHackCLR.TestTarget").ToArray();
				if (!modules.Any(m => m.ClrHandle >= 0x80000000u)) throw new InvalidOperationException("Fixture did not produce a high-address module.");
				int probes = 0, highTypes = 0, highMethods = 0;
				foreach (var module in modules)
				{
					var type = module.GetTypeByName("QHackCLR.TestTarget.HighAddressProbe");
					// The original executable has not initialized this type. All 512
					// copies loaded from bytes must contribute one initialized probe.
					if (type == null) continue;
					probes++;
					if (type.Module.ClrHandle != module.ClrHandle) throw new InvalidOperationException("Type module differs.");
					if (type.ClrHandle >= 0x80000000u) highTypes++;
					var field = type.GetStaticFieldByName("Value");
					if (context.DataAccess.Read<int>(field.GetAddress()) != 42) throw new InvalidOperationException("High-address static field differs.");
					var root = type.GetStaticFieldByName("Root");
					var instance = context.DataAccess.Read<nuint>(root.GetAddress());
					var instanceType = context.Runtime.RuntimeHelper.TypeFactory.GetClrType(context.DataAccess.Read<nuint>(instance));
					if (instanceType.ClrHandle != type.ClrHandle) throw new InvalidOperationException("High-address instance type differs.");
					var number = type.GetInstanceFieldByName("Number");
					if (context.DataAccess.Read<int>(instance + 4 + number.Offset) != 73) throw new InvalidOperationException("High-address instance field differs.");
					var method = type.MethodsInVTable.Single(m => m.Signature.Contains("ReadValue"));
					if (probes == 1 || probes == 512) Console.WriteLine($"PROBE {probes}: module=0x{module.ClrHandle:X8}, type=0x{type.ClrHandle:X8}, code=0x{method.NativeCode:X8}");
					if (method.DeclaringType.ClrHandle != type.ClrHandle) throw new InvalidOperationException("Method declaring type differs.");
					if (method.NativeCode >= 0x80000000u) highMethods++;
				}
				if (probes != 512 || highTypes == 0 || highMethods == 0) throw new InvalidOperationException($"Fixture coverage differs: probes={probes}, high types={highTypes}, high methods={highMethods}.");
				var fixture = modules.Select(m => m.GetTypeByName("QHackCLR.TestTarget.HighAddressFixture")).Single(t => t != null);
				nuint arrayAddress = context.DataAccess.Read<nuint>(fixture.GetStaticFieldByName("HighArray").GetAddress());
				if (arrayAddress < 0x80000000u) throw new InvalidOperationException("Array is not above 2 GiB.");
				var arrayType = context.Runtime.RuntimeHelper.TypeFactory.GetClrType(context.DataAccess.Read<nuint>(arrayAddress));
				using var array = new ClrObject(arrayType, arrayAddress);
				if (array.GetLength() != 8 * 1024 * 1024 ||
					array.ReadArrayElement<int>(new[] { 0 }) != 55 ||
					array.ReadArrayElement<int>(new[] { array.GetLength() - 1 }) != 77)
					throw new InvalidOperationException("High-address DAC object data or array contents differ.");
				Console.WriteLine($"PASS: GetObjectData and first/last elements for array at 0x{arrayAddress:X8}.");
				Console.WriteLine($"PASS {(pass == 0 ? "attach" : "flush")}: {modules.Length} modules; high modules={modules.Count(m => m.ClrHandle >= 0x80000000u)}, types={highTypes}, methods={highMethods}; primitive/reference statics and instance types valid.");
				if (pass == 0) context.Flush();
			}
			process.StandardInput.WriteLine();
			if (!process.WaitForExit(5000) || process.ExitCode != 0)
				throw new InvalidOperationException("High-address fixture did not exit normally.");
		}
		finally
		{
			if (!process.HasExited)
			{
				process.StandardInput.WriteLine();
				if (!process.WaitForExit(5000)) process.Kill();
			}
			if (errors.Wait(TimeSpan.FromSeconds(5)) && errors.Result.Length > 0) Console.WriteLine(errors.Result);
		}
	}

	private static void VerifyAddressForms()
	{
		if (UIntPtr.Size != 4) throw new InvalidOperationException("Run this regression with the x86 test runner.");
		var convert = typeof(DataTarget).Assembly.GetType("QHackCLR.DacHelpers.GlobalHelpers", true)
			.GetMethod("ToNativeAddress", BindingFlags.Public | BindingFlags.Static);
		var toDac = convert.DeclaringType.GetMethod("ToDacAddress", BindingFlags.Public | BindingFlags.Static);
		foreach (ulong address in new[] { 0UL, 0x7FFFFFFFUL, 0x80000000UL, 0xFFFFFFFFUL, 0xFFFFFFFF80000000UL, ulong.MaxValue })
		{
			var actual = (UIntPtr)convert.Invoke(null, new object[] { address });
			if (actual.ToUInt32() != unchecked((uint)address)) throw new InvalidOperationException($"DAC address 0x{address:X16} changed its low bits.");
			ulong extended = (ulong)toDac.Invoke(null, new object[] { actual });
			if (extended != unchecked((ulong)(long)(int)(uint)address))
				throw new InvalidOperationException($"Pointer 0x{actual:X8} was not sign-extended for DAC.");
		}
		foreach (ulong address in new[] { 0x100000000UL, 0x180000000UL, 0xFFFFFFFF7FFFFFFFUL, 0xFFFFFFFF00000000UL })
		{
			try { convert.Invoke(null, new object[] { address }); }
			catch (TargetInvocationException ex) when (ex.InnerException is OverflowException) { continue; }
			throw new InvalidOperationException($"Invalid DAC address 0x{address:X16} was silently truncated.");
		}
		Console.WriteLine("PASS: x86 zero/sign-extended addresses and -1 sentinel; malformed 64-bit addresses rejected.");
	}
}
