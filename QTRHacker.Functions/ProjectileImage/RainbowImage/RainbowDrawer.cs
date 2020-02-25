using QTRHacker.Functions.ProjectileImage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Functions.ProjectileImage.RainbowImage
{
	public class RainbowDrawer : IEmmitable
	{
		public const int RainbowProjectileType = 251;
		public ProjImage Image
		{
			get;
		}
		public RainbowDrawer()
		{
			Image = new ProjImage();
		}

		public void DrawPoint(MPointF location, float direction)
		{
			MPointF unit = new MPointF((float)Math.Cos(direction), (float)Math.Sin(direction));
			MPointF speed = unit * 0.0001f;
			Image.Projs.Add(new Proj(RainbowProjectileType, speed, location + unit * 13));
		}

		public void DrawLine(MPointF start, MPointF end)
		{
			MPointF dP = end - start;
			MPointF unit = dP / dP.Length * 40;
			int count = (int)Math.Ceiling(dP.Length / 40);
			float dir = (float)Math.Atan2(unit.Y, unit.X);
			MPointF s = start;
			for (int i = 0; i < count; i++)
			{
				DrawPoint(s, dir);
				s += unit;
			}
			DrawPoint(end, dir);
		}

		public void DrawArc(MPointF center, float radius, float startRadian, float endRadian)
		{
			float dRadian = endRadian - startRadian;
			int count = (int)Math.Round((Math.Abs(dRadian) * radius / 40)) + 2;
			float unitRadian = dRadian / count;
			float radian = startRadian;
			for (int i = 0; i < count; i++)
			{
				MPointF point = center + new MPointF((float)Math.Cos(radian), (float)Math.Sin(radian)) * radius;
				DrawPoint(point, radian + (float)Math.PI / 2);
				radian += unitRadian;
			}
			MPointF last_point = center + new MPointF((float)Math.Cos(radian), (float)Math.Sin(radian)) * radius;
			DrawPoint(last_point, radian + (float)Math.PI / 2);
		}

		public void Emit(GameContext Context, MPointF Location)
		{
			Image.Emit(Context, Location);
		}
	}
}
