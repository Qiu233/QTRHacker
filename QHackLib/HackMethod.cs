using QHackCLR.Clr;
using QHackLib.Assemble;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QHackLib
{
	/// <summary>
	/// A wrapper for game method
	/// </summary>
	public sealed class HackMethod : IEquatable<HackMethod>
	{
		public QHackContext Context { get; }
		public ClrMethod InternalClrMethod { get; }

		public HackMethod(QHackContext context, ClrMethod method)
		{
			Context = context;
			InternalClrMethod = method;
		}

		public AssemblyCode Call(bool regProtection, nuint? thisPtr, nuint? retBuf, nuint? userEax, params object[] args)
		{
			return AssemblySnippet.FromClrCall(InternalClrMethod.NativeCode, regProtection, thisPtr, retBuf, userEax, args);
		}
		public HackMethodCall Call(nuint? thisPtr)
		{
			return new HackMethodCall(this, thisPtr);
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as HackMethod);
		}
		public override int GetHashCode()
		{
			return InternalClrMethod.GetHashCode();
		}

		public bool Equals(HackMethod other)
		{
			return InternalClrMethod.Equals(other?.InternalClrMethod);
		}

		public static bool operator ==(HackMethod a, HackMethod b)
		{
			if (a == null)
				return b == null;
			return a.Equals(b);
		}
		public static bool operator !=(HackMethod a, HackMethod b)
		{
			return !(a == b);
		}
	}

	/// <summary>
	/// Represents a call to a HackMethod.
	/// </summary>
	public class HackMethodCall
	{
		public HackMethod Method { get; }
		public nuint? ThisPointer { get; }

		public HackMethodCall(HackMethod method, nuint? thisPointer)
		{
			Method = method;
			ThisPointer = thisPointer;
		}
		public HackMethodCall(HackMethod method, AddressableTypedEntity entity)
		{
			Method = method;
			ThisPointer = entity.Address;
		}
		public AssemblyCode Call(bool regProtection, nuint? retBuf, nuint? userEax, params object[] args)
		{
			return Method.Call(regProtection, ThisPointer, retBuf, userEax, args);
		}
	}
}
