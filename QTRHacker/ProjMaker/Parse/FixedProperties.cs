using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.ProjMaker.Parse
{
	public class FixedProperties
	{
		public float X
		{
			get;
		}
		public float Y
		{
			get;
		}
		public float SpeedX
		{
			get;
		}
		public float SpeedY
		{
			get;
		}
		public FixedProperties(float X, float Y, float SpeedX, float SpeedY)
		{
			this.X = X;
			this.Y = Y;
			this.SpeedX = SpeedX;
			this.SpeedY = SpeedY;
		}
		public static FixedProperties GetGlobalProperties()
		{
			return new FixedProperties(0, 0, 0, 0);
		}
	}
}
