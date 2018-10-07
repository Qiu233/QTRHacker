using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Functions.ProjectileImage
{
	public class ProjImage
	{
		public const uint FileVersion = 1;
		public const ushort FileHead = 0xA8;
		public uint Width
		{
			get;
		}
		public uint Height
		{
			get;
		}
		public bool[,] Value
		{
			get;
			private set;
		}
		public ProjImage(uint Width, uint Height)
		{
			this.Width = Width;
			this.Height = Height;
			Value = new bool[Width, Height];
		}

		public void Emit(GameContext context, float X, float Y)
		{
			for (int i = 0; i < Width; i++)
			{
				for (int j = 0; j < Height; j++)
				{
					if (!Value[i, j])
						continue;
					Projectile.NewProjectile(context, X + i * 16, Y + j * 16, 0, 0, 355, 0, 0, context.MyPlayerIndex);
				}
			}
		}
		public void ToStream(Stream stream)
		{
			BinaryWriter bw = new BinaryWriter(stream);
			bw.Write(FileHead);
			bw.Write(FileVersion);
			bw.Write(Width);
			bw.Write(Height);
			for (int i = 0; i < Width; i++)
				for (int j = 0; j < Height; j++)
					bw.Write(Value[i, j]);
		}
		public static ProjImage FromStream(Stream stream)
		{
			BinaryReader br = new BinaryReader(stream);
			if (br.ReadUInt16() != FileHead)
				throw new Exception("错误的文件格式");
			if (br.ReadUInt32() != FileVersion)
				throw new Exception("不支持的(老)文件版本");
			uint Width, Height;
			Width = br.ReadUInt32();
			Height = br.ReadUInt32();
			ProjImage img = new ProjImage(Width, Height);
			for (int i = 0; i < Width; i++)
				for (int j = 0; j < Height; j++)
					img.Value[i, j] = br.ReadBoolean();
			return img;
		}
		public static ProjImage FromImage(string file)
		{

			Bitmap img = (Bitmap)Image.FromFile(file);
			ProjImage proj = new ProjImage((uint)img.Width, (uint)img.Height);
			for (int i = 0; i < proj.Width; i++)
			{
				for (int j = 0; j < proj.Height; j++)
				{
					if (img.GetPixel(i, j).A == 0)
						proj.Value[i, j] = false;
					else
						proj.Value[i, j] = true;
				}
			}
			return proj;
		}
	}
}
