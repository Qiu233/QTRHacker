using Terraria;
using System.Collections.Generic;
using System.Linq;
using System;
using Terraria.ID;
using Terraria.GameInput;

namespace QTRHacker.Patches
{
	internal class AutoFishing
	{
		private enum AutoFishingMode : int
		{
			Disabled = 0,
			All = 1,
			Items = 2,
			NPCs = 3,
		}
		static AutoFishing()
		{
			HooksDef.DoUpdateHook.Pre += DoUpdateHook_Pre;
		}
#pragma warning disable IDE0044 // Add readonly modifier
		private static AutoFishingMode Mode = AutoFishingMode.Disabled;
#pragma warning restore IDE0044 // Add readonly modifier
		public static int Delay = 0;
		public static bool CratesOnly = false;
		public static bool QuestItemsOnly = false;

		private static bool Ready = false;
		private static int mouseX, mouseY;


		private static void UseItem()
		{
			var p = Main.LocalPlayer;
			p.controlUseItem = true;
			p.releaseUseItem = true;
			p.ItemCheck();
		}

		private static void DoUpdateHook_Pre()
		{
			if (Mode == AutoFishingMode.Disabled)
				return;

			var p = Main.LocalPlayer;

			if (p.HeldItem.fishingPole == 0) // not holding fishingpole
			{
				Ready = false;
				return;
			}

			bool did = false;

			var bobbers = Main.projectile.Where(t => t.active && t.owner == Main.myPlayer && t.bobber);
			if (bobbers.Any()) // bobber exists
			{
				Projectile bobber = bobbers.First();
				if (bobber.ai[1] < 0) // fish caught
				{
					if (bobber.localAI[1] > 0f && Mode == AutoFishingMode.Items) // item
					{
						if (CratesOnly)
						{
							if (ItemID.Sets.IsFishingCrate[(int)bobber.localAI[1]])
								DO();
						}
						else if (QuestItemsOnly)
						{
							if (Main.anglerQuestItemNetIDs[Main.anglerQuest] == (int)bobber.localAI[1])
								DO();
						}
						else
							DO();
					}
					else if (bobber.localAI[1] < 0f && Mode == AutoFishingMode.NPCs) // npc
					{
						DO();
					}
					else if (bobber.localAI[1] != 0f && Mode == AutoFishingMode.All)
					{
						DO();
					}
				}
				if (did && !Ready) // fish caught
				{
					mouseX = Main.mouseX;
					mouseY = Main.mouseY;
					Ready = true;
				}
			}
			else
			{
				if (Ready)
				{
					Main.mouseX = mouseX;
					Main.mouseY = mouseY;
					DO(); //cast
				}
			}

			if (PlayerInput.MouseInfo.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed
				&& !did && Ready && Main.hasFocus && InsideScreen(PlayerInput.MouseInfo.X, PlayerInput.MouseInfo.Y))
			{
				Ready = false;
			}


			void DO()
			{
				UseItem(); // cast
				did = true;
			}
		}

		private static bool InsideScreen(int x, int y)
		{
			return x > 0 && y > 0 && x < Main.screenWidth && y < Main.screenHeight;
		}
	}
}