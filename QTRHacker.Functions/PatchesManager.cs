using QTRHacker.Contrast.Structs;
using QTRHacker.Functions.GameObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Functions
{
	public sealed class PatchesManager
	{
		public QHackLib.CLRHelper PatchHelper => Context.HContext.GetCLRHelper("QTRHacker.Patches");
		public QHackLib.CLRHelper InteropHelper => Context.HContext.GetCLRHelper("QTRHacker.Contrast");
		public GameContext Context { get; }
		public PatchesManager(GameContext context)
		{
			Context = context;
			if (Context.HContext.GetCLRHelper("QTRHacker.Patches") == null)
			{
				if (!context.LoadAssembly(Path.GetFullPath("./QTRHacker.Patches.dll"), "QTRHacker.Patches.Boot"))
					throw new InvalidOperationException("Cannot load patches");
			}
		}
		public GameObjectArray2DV<STile> WorldPainter_Tiles
			=> new(Context, PatchHelper.GetStaticHackObject("QTRHacker.Patches.WorldPainter", "Tiles"));

		public bool WorldPainter_EyeDropperActive
		{
			get => PatchHelper.GetStaticFieldValue<bool>("QTRHacker.Patches.WorldPainter", "EyeDropperActive");
			set => PatchHelper.SetStaticFieldValue("QTRHacker.Patches.WorldPainter", "EyeDropperActive", value);
		}
	}
}
