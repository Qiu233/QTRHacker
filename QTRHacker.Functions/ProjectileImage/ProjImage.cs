using QHackLib;
using QHackLib.Assemble;
using QHackLib.FunctionHelper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Functions.ProjectileImage
{
	public class Proj
	{
		public int ProjType
		{
			get;
			set;
		}
		public PointF Speed
		{
			get;
			set;
		}
		public PointF Location
		{
			get;
			set;
		}
	}
	public class ProjImage
	{
		public const uint FileVersion = 2;
		public const ushort FileHead = 0xA8;
		public int Width
		{
			get;
		}
		public int Height
		{
			get;
		}
		/// <summary>
		/// 只在加载图片时这个东西才有效
		/// </summary>
		public int Resolution
		{
			get;
			private set;
		}
		public Proj[,] Projs
		{
			get;
		}
		public ProjImage(int Width, int Height)
		{
			this.Width = Width;
			this.Height = Height;
			Projs = new Proj[Width, Height];
			for (int i = 0; i < Projs.GetLength(0); i++)
				for (int j = 0; j < Projs.GetLength(1); j++)
					Projs[i, j] = new Proj
					{
						Speed = new PointF(0, 0)
					};
		}

		public void Emit(GameContext context, float X, float Y)
		{
			/*for (int i = 0; i < Width; i++)
			{
				for (int j = 0; j < Height; j++)
				{
					var p = Projs[i, j];
					if (!p.Valid)
						continue;

					Projectile.NewProjectile(context, X + i * resolution, Y + j * resolution, 0, 0, p.ProjType, 0, 0, context.MyPlayerIndex);
					//snippet.Content.Add(Projectile.GetSnippet_Call_NewProjectile(context, null, false, X + i * resolution, Y + j * resolution, 0f, 0f, projType, 0, 0f, context.MyPlayerIndex, 0f, 0f));
				}
			}*/
			int data = NativeFunctions.VirtualAllocEx(context.HContext.Handle, 0, (int)(32 * Width * Height), NativeFunctions.AllocationType.Commit, NativeFunctions.MemoryProtection.ExecuteReadWrite);
			NativeFunctions.WriteProcessMemory(context.HContext.Handle, data, BitConverter.GetBytes(Width), 4, 0);
			NativeFunctions.WriteProcessMemory(context.HContext.Handle, data + 4, BitConverter.GetBytes(Height), 4, 0);
			for (int i = 0; i < Projs.GetLength(0); i++)
			{
				for (int j = 0; j < Projs.GetLength(1); j++)
				{
					int t = data + 8 + (int)(j * Width + i) * 32;
					NativeFunctions.WriteProcessMemory(context.HContext.Handle, t, BitConverter.GetBytes(Projs[i, j].ProjType), 4, 0);
					NativeFunctions.WriteProcessMemory(context.HContext.Handle, t + 4, BitConverter.GetBytes(context.MyPlayer.X + Projs[i, j].Location.X), 4, 0);
					NativeFunctions.WriteProcessMemory(context.HContext.Handle, t + 8, BitConverter.GetBytes(context.MyPlayer.Y + Projs[i, j].Location.Y), 4, 0);
					NativeFunctions.WriteProcessMemory(context.HContext.Handle, t + 12, BitConverter.GetBytes(Projs[i, j].Speed.X), 4, 0);
					NativeFunctions.WriteProcessMemory(context.HContext.Handle, t + 16, BitConverter.GetBytes(Projs[i, j].Speed.Y), 4, 0);
				}
			}
			AssemblySnippet snippet = AssemblySnippet.FromCode(
				new AssemblyCode[] {
					(Instruction)$"pushad",
					(Instruction)$"mov ebx,{data}",
			});
			snippet.Content.Add(AssemblySnippet.Loop(
				AssemblySnippet.Loop(
					AssemblySnippet.FromCode(
						new AssemblyCode[] {
							(Instruction)$"mov eax,[esp]",//j
							(Instruction)$"mul dword ptr [ebx]",//宽度，用到了edx
							(Instruction)$"add eax,[esp+4]",//i
							(Instruction)$"shl eax,5",
							(Instruction)$"lea eax,[ebx+8+eax]",
							Projectile.GetSnippet_Call_NewProjectile(context,null,false,
								"[eax+4]","[eax+8]","[eax+12]","[eax+16]","[eax]",0,0f,context.MyPlayerIndex,0f,0f),
				}),
				(int)Height, false),
				(int)Width, true));
			snippet.Content.Add((Instruction)"popad");
			InlineHook.InjectAndWait(context.HContext, snippet,
				context.HContext.FunctionAddressHelper.FunctionsAddress["Terraria.Main::Update"], true);
			NativeFunctions.VirtualFreeEx(context.HContext.Handle, data, 0);

		}
		public void ToStream(Stream stream)
		{
			BinaryWriter bw = new BinaryWriter(stream);
			bw.Write(FileHead);
			bw.Write(FileVersion);
			bw.Write(Width);
			bw.Write(Height);
			for (int i = 0; i < Width; i++)
			{
				for (int j = 0; j < Height; j++)
				{
					bw.Write(Projs[i, j].ProjType);
					bw.Write(Projs[i, j].Location.X);
					bw.Write(Projs[i, j].Location.Y);
					bw.Write(Projs[i, j].Speed.X);
					bw.Write(Projs[i, j].Speed.Y);
				}
			}
		}
		public static ProjImage FromStream(Stream stream)
		{
			BinaryReader br = new BinaryReader(stream);
			if (br.ReadUInt16() != FileHead)
				throw new Exception("错误的文件格式");
			if (br.ReadUInt32() != FileVersion)
				throw new Exception("不支持的(老)文件版本");
			int Width, Height;
			Width = br.ReadInt32();
			Height = br.ReadInt32();
			ProjImage img = new ProjImage(Width, Height);
			for (int i = 0; i < Width; i++)
			{
				for (int j = 0; j < Height; j++)
				{
					img.Projs[i, j].ProjType = br.ReadInt32();
					float a = br.ReadSingle();
					float b = br.ReadSingle();
					img.Projs[i, j].Location = new PointF(a, b);
					float c = br.ReadSingle();
					float d = br.ReadSingle();
					img.Projs[i, j].Speed = new PointF(c, d);
				}
			}
			return img;
		}
		public static ProjImage FromImage(string file, int projType, int resolution)
		{
			Bitmap img = (Bitmap)Image.FromFile(file);
			ProjImage proj = new ProjImage(img.Width, img.Height);
			proj.Resolution = resolution;
			for (int i = 0; i < proj.Width; i++)
			{
				for (int j = 0; j < proj.Height; j++)
				{
					if (img.GetPixel(i, j).A != 0)
					{
						proj.Projs[i, j].ProjType = projType;
						proj.Projs[i, j].Location = new PointF(i * resolution, j * resolution);
					}
				}
			}
			return proj;
		}
	}
}
