using QHackLib;
using QHackLib.Assemble;
using QHackLib.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Core.GameObjects
{
	public class GameString : GameObjectArrayV<char>
	{
		public GameString(GameContext ctx, HackObject obj) : base(ctx, obj)
		{
		}

		public unsafe string GetValue()
		{
			return Encoding.Unicode.GetString(
				Context.HContext.DataAccess.ReadBytes(
					TypedInternalObject.BaseAddress + (uint)sizeof(nuint) * 2, (uint)Length * sizeof(char)));
		}

		public unsafe static string GetString(HackObject obj)
		{
			return Encoding.Unicode.GetString(
				obj.Context.DataAccess.ReadBytes(
					obj.BaseAddress + (uint)sizeof(nuint) + 4, (uint)obj.GetArrayLength() * sizeof(char)));
		}

		/// <summary>
		/// This method will make the game create a new object of string.
		/// </summary>
		/// <param name="s"></param>
		public unsafe static GameString New(GameContext ctx, string s)
		{
			using var alloc = MemoryAllocation.Alloc(ctx.HContext, (uint)(s.Length * 2 + 2) + (uint)sizeof(nuint));
			nuint addr = alloc.AllocationBase;
			nuint resAddr = addr + (uint)(s.Length * 2 + 2);
			byte[] data = Encoding.Unicode.GetBytes(s);
			ctx.HContext.DataAccess.WriteBytes(addr, data);
			ctx.HContext.DataAccess.Write<short>(addr + (uint)data.Length, 0);
			var asm = AssemblySnippet.FromCode(new AssemblyCode[] {
				AssemblySnippet.FromConstructString(ctx.HContext, addr, resAddr)
			});
			var thread = ctx.RunOnManagedThread(asm);
			if (!Task.Run(() => thread.WaitToDispose()).Wait(2000))//won't forcefully dispose because that would cause game crashing
			{
				throw new Exception("Failed to create string object");
			}
			nuint res = ctx.HContext.DataAccess.Read<nuint>(resAddr);
			return new GameString(ctx, new HackObject(ctx.HContext, ctx.HContext.Runtime.Heap.StringType, res));
		}

		public static implicit operator string(GameString s)
		{
			return s.ToString();
		}

		public override string ToString() => GetValue();
	}
}
