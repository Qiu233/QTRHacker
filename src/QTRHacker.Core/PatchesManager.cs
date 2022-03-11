using QTRHacker.Core.GameObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Core
{
	public sealed class PatchesManager
	{
		[StructLayout(LayoutKind.Sequential)]
		public struct STile
		{
			public ushort Type;
			public ushort Wall;
			public byte Liquid;
			public short STileHeader;
			public byte BTileHeader;
			public byte BTileHeader2;
			public byte BTileHeader3;
			public short FrameX;
			public short FrameY;

			public bool Active()
			{
				return (STileHeader & 0x20) == 0x20;
			}
			public int WallFrameX()
			{
				return (BTileHeader2 & 0xF) * 36;
			}
			public int WallFrameY()
			{
				return (BTileHeader3 & 7) * 36;
			}
		}
		public QHackLib.CLRHelper PatchHelper => Context.HContext.GetCLRHelper("QTRHacker.Patches");
		public GameContext Context { get; }
		public PatchesManager(GameContext context)
		{
			Context = context;
		}

		public bool IsInitialized => PatchHelper != null;

		public void Init()
		{
			if (IsInitialized)
				return;
			if (!Context.LoadAssemblyAsBytes(Path.GetFullPath("./QTRHacker.Patches.dll"), "QTRHacker.Patches.Boot"))
				throw new InvalidOperationException("Couldn't load patches");
		}

		public GameObjectArray2DV<STile> WorldPainter_ClipBoard
			=> new(Context, PatchHelper.GetStaticHackObject("QTRHacker.Patches.WorldPainter", "ClipBoard"));

		public bool WorldPainter_EyeDropperActive
		{
			get => PatchHelper.GetStaticFieldValue<bool>("QTRHacker.Patches.WorldPainter", "EyeDropperActive");
			set => PatchHelper.SetStaticFieldValue("QTRHacker.Patches.WorldPainter", "EyeDropperActive", value);
		}
		public bool WorldPainter_BrushActive
		{
			get => PatchHelper.GetStaticFieldValue<bool>("QTRHacker.Patches.WorldPainter", "BrushActive");
			set => PatchHelper.SetStaticFieldValue("QTRHacker.Patches.WorldPainter", "BrushActive", value);
		}
		public bool WorldPainter_Loading
		{
			get => PatchHelper.GetStaticFieldValue<bool>("QTRHacker.Patches.WorldPainter", "Loading");
			set => PatchHelper.SetStaticFieldValue("QTRHacker.Patches.WorldPainter", "Loading", value);
		}
		public nuint WorldPainter_Buffer
		{
			get => PatchHelper.GetStaticFieldValue<nuint>("QTRHacker.Patches.WorldPainter", "Buffer");
			set => PatchHelper.SetStaticFieldValue("QTRHacker.Patches.WorldPainter", "Buffer", value);
		}
	}
}
