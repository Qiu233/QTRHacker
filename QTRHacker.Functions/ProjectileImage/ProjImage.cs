using QHackLib;
using QHackLib.Assemble;
using QHackLib.FunctionHelper;
using QTRHacker.Functions.GameObjects;
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
		public MPointF Speed
		{
			get;
			set;
		}
		public MPointF Location
		{
			get;
			set;
		}
	}
	public class ProjImage
	{
		public const uint FileVersion = 3;
		public const ushort FileHead = 0xA8;
		/// <summary>
		/// 只在加载图片时这个东西才有效
		/// </summary>
		public int Resolution
		{
			get;
			private set;
		}
		public List<Proj> Projs
		{
			get;
		}
		public string Description
		{
			get;
			private set;
		}
		public ProjImage()
		{
			Projs = new List<Proj>();
		}

		public void Emit(GameContext context, float X, float Y)
		{
			int data = NativeFunctions.VirtualAllocEx(context.HContext.Handle, 0, (int)(32 * Projs.Count), NativeFunctions.AllocationType.Commit, NativeFunctions.MemoryProtection.ExecuteReadWrite);
			NativeFunctions.WriteProcessMemory(context.HContext.Handle, data, BitConverter.GetBytes(Projs.Count), 4, 0);
			for (int i = 0; i < Projs.Count; i++)
			{
				int t = data + 8 + i * 32;
				NativeFunctions.WriteProcessMemory(context.HContext.Handle, t, BitConverter.GetBytes(Projs[i].ProjType), 4, 0);
				NativeFunctions.WriteProcessMemory(context.HContext.Handle, t + 4, BitConverter.GetBytes(context.MyPlayer.X + Projs[i].Location.X), 4, 0);
				NativeFunctions.WriteProcessMemory(context.HContext.Handle, t + 8, BitConverter.GetBytes(context.MyPlayer.Y + Projs[i].Location.Y), 4, 0);
				NativeFunctions.WriteProcessMemory(context.HContext.Handle, t + 12, BitConverter.GetBytes(Projs[i].Speed.X), 4, 0);
				NativeFunctions.WriteProcessMemory(context.HContext.Handle, t + 16, BitConverter.GetBytes(Projs[i].Speed.Y), 4, 0);
			}
			AssemblySnippet snippet = AssemblySnippet.FromCode(
				new AssemblyCode[] {
					(Instruction)$"pushad",
					(Instruction)$"mov ebx,{data}",
			});
			snippet.Content.Add(AssemblySnippet.Loop(
					AssemblySnippet.FromCode(
						new AssemblyCode[] {
							(Instruction)$"mov eax,[esp]",//i
							(Instruction)$"shl eax,5",
							(Instruction)$"lea eax,[ebx+8+eax]",
							Projectile.GetSnippet_Call_NewProjectile(context,null,false,
								"[eax+4]","[eax+8]","[eax+12]","[eax+16]","[eax]",0,0f,context.MyPlayerIndex,0f,0f),
				}),
				(int)Projs.Count, true));
			snippet.Content.Add((Instruction)"popad");
			InlineHook.InjectAndWait(context.HContext, snippet,
				context.HContext.MainAddressHelper["Terraria.Main", "Update"], true);
			NativeFunctions.VirtualFreeEx(context.HContext.Handle, data, 0);

		}
		public void ToStream(Stream stream)
		{
			BinaryWriter bw = new BinaryWriter(stream);
			bw.Write(FileHead);
			bw.Write(FileVersion);
			bw.Write(Description);
			bw.Write(Projs.Count);
			for (int i = 0; i < Projs.Count; i++)
			{
				bw.Write(Projs[i].ProjType);
				bw.Write(Projs[i].Location.X);
				bw.Write(Projs[i].Location.Y);
				bw.Write(Projs[i].Speed.X);
				bw.Write(Projs[i].Speed.Y);
			}
		}
		public static ProjImage FromStream(Stream stream)
		{
			BinaryReader br = new BinaryReader(stream);
			if (br.ReadUInt16() != FileHead)
				throw new Exception("错误的文件格式");
			if (br.ReadUInt32() != FileVersion)
				throw new Exception("不支持的(老)文件版本");
			string des = br.ReadString();
			int count = br.ReadInt32();
			ProjImage img = new ProjImage();
			img.Description = des;
			for (int i = 0; i < count; i++)
			{
				Proj p = new Proj();
				p.ProjType = br.ReadInt32();
				float a = br.ReadSingle();
				float b = br.ReadSingle();
				p.Location = new MPointF(a, b);
				float c = br.ReadSingle();
				float d = br.ReadSingle();
				p.Speed = new MPointF(c, d);
				img.Projs.Add(p);
			}
			return img;
		}
		public static ProjImage FromImage(string file, int projType, int resolution)
		{
			Bitmap img = (Bitmap)Image.FromFile(file);
			ProjImage proj = new ProjImage();
			proj.Resolution = resolution;
			for (int i = 0; i < img.Width; i++)
			{
				for (int j = 0; j < img.Height; j++)
				{
					if (img.GetPixel(i, j).A != 0)
					{
						Proj p = new Proj();
						p.ProjType = projType;
						p.Location = new MPointF(i * resolution, j * resolution);
						proj.Projs.Add(p);
					}
				}
			}
			return proj;
		}
	}
}
