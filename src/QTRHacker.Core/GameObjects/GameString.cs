using QHackLib;
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

		public static implicit operator string(GameString s)
		{
			return s.ToString();
		}

		public unsafe override string ToString() => Encoding.Unicode.GetString(
				Context.HContext.DataAccess.ReadBytes(
					TypedInternalObject.BaseAddress + (uint)sizeof(nuint) * 2, (uint)Length * sizeof(char)));
	}
}
