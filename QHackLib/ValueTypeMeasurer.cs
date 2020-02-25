using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QHackLib
{
	public sealed class ValueTypeMeasurer
	{
		public static int Measure<T>() where T : struct
		{
			var t = typeof(T);
			if (t.Equals(typeof(bool)) || t.Equals(typeof(byte)) || t.Equals(typeof(sbyte)))
				return 1;
			else if (t.Equals(typeof(short)) || t.Equals(typeof(ushort)))
				return 2;
			else if (t.Equals(typeof(int)) || t.Equals(typeof(uint)) || t.Equals(typeof(float)))
				return 4;
			else if (t.Equals(typeof(long)) || t.Equals(typeof(ulong)) || t.Equals(typeof(double)))
				return 8;
			return Marshal.SizeOf<T>();
		}
	}
}
