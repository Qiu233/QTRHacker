using QHackLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Functions.GameObjects
{
	public class ValueTypeArray<T> : GameObject where T : struct
	{
		public virtual T this[int index]
		{
			get => ReadFromOffset<T>(0x8 + ValueTypeMeasurer.Measure<T>() * index);
			set => WriteFromOffset(0x8 + ValueTypeMeasurer.Measure<T>() * index, value);
		}
		public virtual int Length
		{
			get
			{
				ReadFromOffset(0x4, out int v);
				return v;
			}
		}
		public ValueTypeArray(GameContext context, int bAddr) : base(context, bAddr)
		{

		}
	}
}
