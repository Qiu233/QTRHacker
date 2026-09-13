using System;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;

namespace QHackCLR.TestTarget;

public class FixtureState
{
	public static int[] Vector = { 11, 22, 33 };
	public static int[] Empty = Array.Empty<int>();
	public static int[] NullVector;
	public static Array Matrix = Array.CreateInstance(typeof(int), new[] { 2, 3 }, new[] { -2, 5 });
	public static Array NonVector = Array.CreateInstance(typeof(int), new[] { 2 }, new[] { -3 });
	public static int Counter = 42;
	public static DerivedFixture Derived = new DerivedFixture();
	public static string Text = "Player \u6d4b\u8bd5\0\ud83c\udf0d";
	public static string EmptyText = string.Empty;
	public virtual int UncompiledMethod() => Counter + 1;
}

public class BaseFixture { public int Inherited = 1; }
public class DerivedFixture : BaseFixture
{
	public int Introduced = 2;
	public static int Static = 3;
	[ThreadStatic] public static int ThreadLocal;
}

internal static class Program
{
	private static void Main()
	{
		FixtureState.Matrix.SetValue(55, -2, 5);
		FixtureState.Matrix.SetValue(77, -1, 7);
		FixtureState.NonVector.SetValue(88, -3);
		FixtureState.NonVector.SetValue(99, -2);
		DerivedFixture.ThreadLocal = 4;
		var arrays = new Array[] { FixtureState.Vector, FixtureState.Empty, FixtureState.Matrix, FixtureState.NonVector };
		var pins = new GCHandle[arrays.Length];
		for (int i = 0; i < arrays.Length; i++)
			pins[i] = GCHandle.Alloc(arrays[i], GCHandleType.Pinned);
		// Exercise a legitimate module without an on-disk path during module enumeration.
		var dynamicAssembly = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName("QHackCLR.DynamicFixture"), AssemblyBuilderAccess.Run);
		dynamicAssembly.DefineDynamicModule("DynamicFixture").DefineType("DynamicFixtureType", TypeAttributes.Public).CreateType();
		Console.WriteLine(Process.GetCurrentProcess().Id);
		Console.ReadLine();
		foreach (var pin in pins)
			pin.Free();
		GC.KeepAlive(dynamicAssembly);
	}
}
