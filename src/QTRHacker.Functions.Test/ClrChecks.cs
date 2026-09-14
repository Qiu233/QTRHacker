using QHackCLR.Builders;
using QHackCLR.Common;
using QHackCLR.DataTargets;
using QTRHacker.Core;
using QTRHacker.Core.GameObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace QTRHacker.Functions.Test;

internal static unsafe class ClrChecks
{
	private struct ContainsReference { public string Value; }

	public static void Verify(string targetExecutable)
	{
		VerifyMemoryAccess();
		using var process = Process.Start(new ProcessStartInfo(Path.GetFullPath(targetExecutable))
		{
			UseShellExecute = false,
			CreateNoWindow = true,
			RedirectStandardInput = true,
			RedirectStandardOutput = true,
		});
		try
		{
			var ready = process.StandardOutput.ReadLineAsync();
			if (!ready.Wait(TimeSpan.FromSeconds(15)))
				throw new TimeoutException("CLR fixture did not start.");
			if (ready.Result != process.Id.ToString())
				throw new InvalidOperationException("CLR fixture exited before initialization.");
			using var target = new DataTarget(process.Id);
			var runtime = target.ClrVersions[0].CreateRuntime();
			var builder = (RuntimeBuilder)runtime.RuntimeHelper;
			var heap = runtime.Heap;
			if (heap.FreeType.Name != "Free" || heap.ObjectType.Name != "System.Object" ||
				heap.StringType.Name != "System.String" || heap.ExceptionType.Name != "System.Exception")
				throw new InvalidOperationException("CLR heap globals could not be resolved.");
			if (runtime.BaseClassLibrary.GetTypeByName("System.Byte") == null)
				throw new InvalidOperationException("Patch loader's System.Byte lookup failed.");
			var module = runtime.AppDomain.Modules.Single(m => m.Name == "QHackCLR.TestTarget");
			VerifyTypeLookup(runtime, module, target.DataAccess);
			var type = module.GetTypeByName("QHackCLR.TestTarget.FixtureState");
			if (type is null)
				throw new InvalidOperationException("Fixture type was not found.");
			if (!type.MethodsInVTable.Any(m => m.Signature?.Contains("UncompiledMethod") == true))
				throw new InvalidOperationException("Uncompiled method was not enumerated.");
			var derived = module.GetTypeByName("QHackCLR.TestTarget.DerivedFixture");
			Equal(3, derived.Fields.Count, "declared fields excluding inherited fields");
			Equal(1, derived.BaseType.Fields.Count, "base instance fields");
			if (!derived.Fields.Any(f => f.Name == "ThreadLocal"))
				throw new InvalidOperationException("Thread-static field was not enumerated.");
			VerifyStrings(process);
			VerifyPatchInitializationError(process);
			Throws<InvalidOperationException>(() => derived.GetLength(
				target.DataAccess.Read<nuint>(type.GetStaticFieldByName("Derived").GetAddress())));

			using var vector = GetArray("Vector");
			Equal(22, vector.ReadArrayElement<int>(new[] { 1 }), "vector value");
			Throws<ArgumentOutOfRangeException>(() => vector.GetArrayElementAddress(new[] { -1 }));
			Throws<ArgumentOutOfRangeException>(() => vector.GetArrayElementAddress(new[] { 3 }));
			Throws<ArgumentOutOfRangeException>(() => vector.GetLength(-1));
			Throws<ArgumentOutOfRangeException>(() => vector.GetLowerBound(-1));
			Throws<ArgumentException>(() => vector.GetArrayElementAddress(new[] { 0, 0 }));
			Throws<ArgumentNullException>(() => vector.GetArrayElementAddress(null));
			using var empty = GetArray("Empty");
			Throws<ArgumentOutOfRangeException>(() => empty.GetArrayElementAddress(new[] { 0 }));

			using var matrix = GetArray("Matrix");
			Equal(55, matrix.ReadArrayElement<int>(new[] { -2, 5 }), "matrix first value");
			Equal(77, matrix.ReadArrayElement<int>(new[] { -1, 7 }), "matrix last value");
			Throws<ArgumentOutOfRangeException>(() => matrix.GetArrayElementAddress(new[] { -3, 5 }));
			Throws<ArgumentOutOfRangeException>(() => matrix.GetArrayElementAddress(new[] { -2, 4 }));
			Throws<ArgumentOutOfRangeException>(() => matrix.GetArrayElementAddress(new[] { 0, 5 }));
			Throws<ArgumentOutOfRangeException>(() => matrix.GetArrayElementAddress(new[] { int.MaxValue, 5 }));
			using var nonVector = GetArray("NonVector");
			Equal(-3, nonVector.GetLowerBound(0), "rank-one non-SZ lower bound");
			Equal(88, nonVector.ReadArrayElement<int>(new[] { -3 }), "non-SZ first value");
			Equal(99, nonVector.ReadArrayElement<int>(new[] { -2 }), "non-SZ last value");
			Throws<ArgumentOutOfRangeException>(() => nonVector.GetArrayElementAddress(new[] { -4 }));
			using var nullObject = new ClrObject(vector.Type, 0);
			if (!nullObject.IsNullPtr)
				throw new InvalidOperationException("Null wrapper was not preserved.");
			Throws<ArgumentNullException>(() => nullObject.GetLength());

			Throws<COMException>(() => builder.GetClrType(1));
			Throws<COMException>(() => builder.GetModule(1));
			Throws<COMException>(() => new ClrMethod(builder, 1));
			Throws<COMException>(() => new ClrObject(vector.Type, 1));
			string fieldName = null;
			FieldAttributes fieldAttributes = 0;
			Throws<COMException>(() => builder.GetFieldProps(type, -1, ref fieldName, ref fieldAttributes));
			// Repeated metadata lookup must not accumulate DAC process references.
			int before = GetDacReferenceCount(runtime);
			for (int i = 0; i < 100; i++)
				builder.GetFieldProps(type, type.GetStaticFieldByName("Counter").MDToken, ref fieldName, ref fieldAttributes);
			Equal(before, GetDacReferenceCount(runtime), "DAC references after metadata enumeration");
			for (int i = 0; i < 10; i++)
				Throws<COMException>(() => builder.GetFieldProps(type, -1, ref fieldName, ref fieldAttributes));
			Equal(before, GetDacReferenceCount(runtime), "DAC references after failed metadata enumeration");
			runtime.Flush();
			if (runtime.AppDomain.Modules.All(m => m.Name != module.Name))
				throw new InvalidOperationException("Module enumeration failed after Flush.");
			process.StandardInput.WriteLine();
			if (!process.WaitForExit(5000))
				throw new TimeoutException("CLR fixture did not exit.");
			Throws<IOException>(() => vector.GetLength());
			Console.WriteLine("CLR checks passed: targeted type lookup, strings, memory errors, array bounds, DAC failures, metadata references, Flush, and target exit.");

			ClrObject GetArray(string name)
			{
				var address = target.DataAccess.Read<nuint>(type.GetStaticFieldByName(name).GetAddress());
				return new ClrObject(builder.GetClrType(target.DataAccess.Read<nuint>(address)), address);
			}
		}
		finally
		{
			if (!process.HasExited)
			{
				process.StandardInput.WriteLine();
				if (!process.WaitForExit(5000))
					process.Kill();
			}
		}
	}

	private static void VerifyTypeLookup(ClrRuntime runtime, ClrModule module, DataAccess data)
	{
		var typesCache = typeof(ClrModule).GetField("m_DefinedTypes", BindingFlags.Instance | BindingFlags.NonPublic);
		var bcl = runtime.BaseClassLibrary;
		object cachedBcl = typesCache.GetValue(bcl), cachedModule = typesCache.GetValue(module);
		try
		{
			// Simulate failure in unrelated type materialization. A precise lookup
			// must succeed while complete enumeration continues to report the error.
			typesCache.SetValue(bcl, new FailingTypeList());
			typesCache.SetValue(module, new FailingTypeList());
			Throws<COMException>(() => bcl.DefinedTypes.ToArray());
			foreach (string name in new[] { "System.Type", "System.Runtime.InteropServices.Marshal", "System.Threading.Tasks.Task", "System.Action" })
				if (bcl.GetTypeByName(name)?.Name != name)
					throw new InvalidOperationException("Remote thread prerequisite lookup failed: " + name);
			var nested = module.GetTypeByName("QHackCLR.TestTarget.FixtureState+NestedLookupFixture");
			if (nested == null) throw new InvalidOperationException("Nested type lookup failed.");
			Equal(73, data.Read<int>(nested.GetStaticFieldByName("Value").GetAddress()), "nested type static value");
			if (module.GetTypeByName("QHackCLR.TestTarget.NoSuchType") != null ||
				module.GetTypeByName("QHackCLR.TestTarget.UnloadedLookupFixture") != null)
				throw new InvalidOperationException("Missing or unloaded type was returned as loaded.");
			// Metadata imports are caller-owned on both success and not-found paths.
			int references = GetDacReferenceCount(runtime);
			for (int i = 0; i < 100; i++)
			{
				bcl.GetTypeByName("System.Type");
				module.GetTypeByName("QHackCLR.TestTarget.NoSuchType");
			}
			Equal(references, GetDacReferenceCount(runtime), "DAC references after targeted type lookups");
			Throws<COMException>(() => module.DefinedTypes.ToArray());
		}
		finally
		{
			typesCache.SetValue(bcl, cachedBcl);
			typesCache.SetValue(module, cachedModule);
		}
	}

	private sealed class FailingTypeList : IReadOnlyList<ClrType>
	{
		public int Count => throw Failure();
		public ClrType this[int index] => throw Failure();
		public IEnumerator<ClrType> GetEnumerator() => throw Failure();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		private static COMException Failure() => new("Simulated unrelated type lookup failure.", unchecked((int)0x80070057));
	}

	private static void VerifyStrings(Process process)
	{
		using var context = GameContext.OpenGame(process);
		var helper = context.HContext.GetCLRHelper("QHackCLR.TestTarget");
		Verify("Text", "Player \u6d4b\u8bd5\0\ud83c\udf0d");
		Verify("EmptyText", "");
		void Verify(string field, string expected)
		{
			var obj = helper.GetStaticHackObject("QHackCLR.TestTarget.FixtureState", field);
			var value = new GameString(context, obj);
			Equal(expected.Length, value.Length, "GameString length");
			Equal(expected.Length, obj.Type.GetLength(obj.BaseAddress), "CLR string length");
			string converted = value;
			if (value.GetValue() != expected || value.ToString() != expected || converted != expected || GameString.GetString(obj) != expected)
				throw new InvalidOperationException("GameString read or conversion changed the string contents.");
		}
	}

	private static void VerifyPatchInitializationError(Process process)
	{
		using var context = GameContext.OpenGame(process);
		var expected = new COMException("Fixture DAC failure", unchecked((int)0x80070057));
		var load = Task.FromException<bool>(expected);
		typeof(PatchesManager).GetField("initialization", BindingFlags.Instance | BindingFlags.NonPublic)
			.SetValue(context.Patches, load);
		for (int i = 0; i < 2; i++)
		{
			try { context.Patches.Init(); }
			catch (COMException actual) when (ReferenceEquals(actual, expected)) { continue; }
			throw new InvalidOperationException("Patch initialization hid or retried a failed load.");
		}
	}

	private static int GetDacReferenceCount(ClrRuntime runtime)
	{
		object pointer = runtime.DacLibrary.GetType().GetProperty("ClrDataProcess").GetValue(runtime.DacLibrary);
		IntPtr address = (IntPtr)Pointer.Unbox(pointer);
		Marshal.AddRef(address);
		return Marshal.Release(address);
	}

	private static void VerifyMemoryAccess()
	{
		using var process = Process.GetCurrentProcess();
		var access = new DataAccess((nuint)process.Handle);
		IntPtr allocation = Marshal.AllocHGlobal(16);
		try
		{
			nuint address = (nuint)allocation;
			access.Write(address, 123);
			Equal(123, access.Read<int>(address), "scalar round trip");
			access.Write(address, new[] { 7, 8 }, 2);
			var values = new int[2];
			access.Read(address, values, 2);
			Equal(8, values[1], "array round trip");
			Throws<ArgumentOutOfRangeException>(() => access.Read(address, new byte[1], 2));
			Throws<ArgumentOutOfRangeException>(() => access.Write(address, new byte[1], 2));
			Throws<ArgumentOutOfRangeException>(() => access.Read(address, new int[1], uint.MaxValue));
			Throws<ArgumentOutOfRangeException>(() => access.Write(address, new int[1], 2));
			Throws<ArgumentOutOfRangeException>(() => access.Read(address, (void*)allocation, -1));
			Throws<ArgumentNullException>(() => access.Read(address, (byte[])null, 0));
			Throws<ArgumentNullException>(() => access.WriteBytes(address, null));
			Throws<ArgumentException>(() => access.Read<ContainsReference>(address));
			Throws<ArgumentException>(() => access.Write(address, new ContainsReference { Value = "kept alive" }));
			access.Read(0, Array.Empty<byte>(), 0);
			access.Write(0, Array.Empty<int>(), 0);
			Equal(0, access.ReadBytes(0, 0).Length, "zero-length read");
			access.WriteBytes(0, Array.Empty<byte>());
			Throws<IOException>(() => access.Read<int>(0));
			Throws<IOException>(() => access.Read(0, out int _));
			Throws<IOException>(() => access.ReadBytes(0, 4));
			Throws<IOException>(() => access.Write(0, 42));
			Throws<IOException>(() => access.WriteBytes(0, new byte[4]));
			var invalid = new DataAccess(0);
			Throws<IOException>(() => invalid.Read<int>(address));
			Throws<IOException>(() => invalid.Write(address, 42));
		}
		finally { Marshal.FreeHGlobal(allocation); }
	}

	private static void Equal(int expected, int actual, string description)
	{
		if (expected != actual)
			throw new InvalidOperationException($"{description}: expected {expected}, got {actual}.");
	}

	private static void Throws<T>(Action action) where T : Exception
	{
		try { action(); }
		catch (T) { return; }
		throw new InvalidOperationException($"Expected {typeof(T).Name}.");
	}
}
