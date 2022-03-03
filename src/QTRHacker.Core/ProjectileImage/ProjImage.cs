using QHackLib;
using QHackLib.Assemble;
using QHackLib.FunctionHelper;
using QHackLib.Memory;
using QTRHacker.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Core.ProjectileImage
{
	public struct Proj
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

		public Proj(int projType, MPointF speed, MPointF location)
		{
			ProjType = projType;
			Speed = speed;
			Location = location;
		}

	}
	public class ProjImage : IEmmitable
	{
		public const uint FileVersion = 3;
		public const ushort FileHead = 0x2A_2A;

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

		public void DrawImage(ProjImage image, MPointF location)
		{
			foreach (var proj in image.Projs)
			{
				var newImg = proj;
				newImg.Location += location;
				Projs.Add(newImg);
			}
		}

		public void Emit(GameContext context, MPointF Location)
		{
			using MemoryAllocation alloc = new(context.HContext, 32 * (uint)Projs.Count + 64);
			RemoteMemoryStream stream = new(context.HContext, alloc.AllocationBase, 0);
			stream.Write<long>(Projs.Count);//8 bytes

			byte[] bs = new byte[12];
			for (int i = 0; i < Projs.Count; i++)
			{
				stream.Write(Projs[i].ProjType);
				stream.Write(Location.X + Projs[i].Location.X);
				stream.Write(Location.Y + Projs[i].Location.Y);
				stream.Write(Projs[i].Speed.X);
				stream.Write(Projs[i].Speed.Y);

				stream.Write(bs, (uint)bs.Length);
			}
			AssemblySnippet snippet = AssemblySnippet.FromCode(
				new AssemblyCode[] {
					(Instruction)$"pushad",
					(Instruction)$"mov ebx,{alloc.AllocationBase}",
			});
			snippet.Content.Add(AssemblySnippet.Loop(
					AssemblySnippet.FromCode(
						new AssemblyCode[] {
							(Instruction)$"mov eax,[esp]",		//i
							(Instruction)$"shl eax,5",			//*32
							(Instruction)$"lea eax,[ebx+8+eax]",

							(Instruction)$"xor ecx,ecx",		//SpawnSource:IProjectileSource
							(Instruction)$"mov edx,[eax+4]",	//X:float
							(Instruction)$"push [eax+8]",		//Y:float
							(Instruction)$"push [eax+12]",		//SpeedX:float
							(Instruction)$"push [eax+16]",		//SpeedY:float
							(Instruction)$"push [eax]",			//Type:int
							(Instruction)$"push 0",				//Damage:int
							(Instruction)$"push 0",				//KnockBack:float
							(Instruction)$"push {context.MyPlayerIndex}",//Owner:int
							(Instruction)$"push 0",				//ai0:float
							(Instruction)$"push 0",				//ai1:float
							(Instruction)$"call {context.GameModuleHelper.GetClrMethodBySignature("Terraria.Projectile","Terraria.Projectile.NewProjectile(Terraria.DataStructures.IProjectileSource, Single, Single, Single, Single, Int32, Int32, Single, Int32, Single, Single)").NativeCode}",

				}),
				Projs.Count, true));
			snippet.Content.Add((Instruction)"popad");
			context.RunByHookOnUpdate(snippet);
		}
		public void ToStream(Stream stream)
		{
			BinaryWriter bw = new(stream);
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
			BinaryReader br = new(stream);
			if (br.ReadUInt16() != FileHead)
				throw new Exception("错误的文件格式");
			if (br.ReadUInt32() != FileVersion)
				throw new Exception("不支持的(老)文件版本");
			string des = br.ReadString();
			int count = br.ReadInt32();
			ProjImage img = new();
			img.Description = des;
			for (int i = 0; i < count; i++)
			{
				Proj p = new();
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
			ProjImage proj = new();
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
