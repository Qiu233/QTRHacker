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

			public void Active(bool active)
			{
				if (active)
					STileHeader |= 32;
				else
					STileHeader = (short)(STileHeader & 0xFFDF);
			}
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

		public bool AimBot_HostileNPCsOnly
		{
			get => PatchHelper.GetStaticFieldValue<bool>("QTRHacker.Patches.AimBot", "HostileNPCsOnly");
			set => PatchHelper.SetStaticFieldValue("QTRHacker.Patches.AimBot", "HostileNPCsOnly", value);
		}
		public bool AimBot_HostilePlayersOnly
		{
			get => PatchHelper.GetStaticFieldValue<bool>("QTRHacker.Patches.AimBot", "HostilePlayersOnly");
			set => PatchHelper.SetStaticFieldValue("QTRHacker.Patches.AimBot", "HostilePlayersOnly", value);
		}
		public float AimBot_MaxDistance_NPC
		{
			get => PatchHelper.GetStaticFieldValue<float>("QTRHacker.Patches.AimBot", "MaxDistance_NPC");
			set => PatchHelper.SetStaticFieldValue("QTRHacker.Patches.AimBot", "MaxDistance_NPC", value);
		}
		public float AimBot_MaxDistance_Player
		{
			get => PatchHelper.GetStaticFieldValue<float>("QTRHacker.Patches.AimBot", "MaxDistance_Player");
			set => PatchHelper.SetStaticFieldValue("QTRHacker.Patches.AimBot", "MaxDistance_Player", value);
		}
		public int AimBot_TargetedPlayerIndex
		{
			get => PatchHelper.GetStaticFieldValue<int>("QTRHacker.Patches.AimBot", "TargetedPlayerIndex");
			set => PatchHelper.SetStaticFieldValue("QTRHacker.Patches.AimBot", "TargetedPlayerIndex", value);
		}
		/// <summary>
		/// This is an enum
		/// </summary>
		public int AimBot_Mode
		{
			get => PatchHelper.GetStaticFieldValue<int>("QTRHacker.Patches.AimBot", "Mode");
			set => PatchHelper.SetStaticFieldValue("QTRHacker.Patches.AimBot", "Mode", value);
		}
	}
}
