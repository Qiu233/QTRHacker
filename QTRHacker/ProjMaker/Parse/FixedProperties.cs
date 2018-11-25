using QTRHacker.Functions.ProjectileImage;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.ProjMaker.Parse
{
	public class FixedProperties
	{
		public MPointF Location
		{
			get;
		}
		public MPointF Speed
		{
			get;
		}
		public FixedProperties(MPointF Location, MPointF Speed)
		{
			this.Location = Location;
			this.Speed = Speed;
		}
		public static FixedProperties GetGlobalProperties()
		{
			return new FixedProperties(new MPointF(0, 0), new MPointF(0, 0));
		}
	}
}
