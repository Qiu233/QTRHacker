using QHackLib;
using QTRHacker.Functions;
using QTRHacker.Functions.GameObjects;
using QTRHacker.NewDimension.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QTRHacker.NewDimension.PagePanels
{
	public class InfoViewEx : InfoView
	{
		private static readonly Color EditBoxBackColor = Color.FromArgb(40, 40, 40);
		public InfoViewEx(int TipWidth) : base(
			new TextBox()
			{
				BackColor = EditBoxBackColor,
				ForeColor = Color.White,
				BorderStyle = BorderStyle.FixedSingle,
				TextAlign = HorizontalAlignment.Center,
				Font = new Font("Consolas", 8)
			}, TipDock.Left, false, TipWidth)
		{
			Tip.ForeColor = Color.White;
			Tip.BackColor = EditBoxBackColor;
		}
	}
	public class PagePanel_MainPage : PagePanel
	{
		[StructLayout(LayoutKind.Sequential)]
		public struct POINT
		{
			public int X;
			public int Y;
			public POINT(int x, int y)
			{
				this.X = x;
				this.Y = y;
			}
		}
		[DllImport("User32.dll")]
		public static extern bool GetCursorPos(out POINT lpPoint);
		[DllImport("User32.dll")]
		public static extern IntPtr WindowFromPoint(int xPoint, int yPoint);
		[DllImport("User32.dll")]
		public static extern int GetWindowThreadProcessId(IntPtr hwnd, out int ID);
		private bool Dragging = false;
		private readonly Font TextFont;
		private readonly Image CrossImage;
		private readonly Button RefreshButton;
		private readonly InfoView PlayerArrayBaseAddressInfoView, CurrentPlayerBaseAddressInfoView,
			CurrentPlayerInventoryBaseAddressInfoView, CurrentPlayerArmorBaseAddressInfoView,
			CurrentPlayerDyeBaseAddressInfoView, CurrentPlayerMiscBaseAddressInfoView,
			CurrentPlayerMiscDyeBaseAddressInfoView,
			CurrentPlayerBuffTypeBaseAddressInfoView,
			CurrentPlayerBuffTimeBaseAddressInfoView, CurrentPlayerFirstItemBaseAddressInfoView,
			Terraria_Main_Update_BaseAddressInfoView,
			SignHeadBaseAddressInfoView;
		public PagePanel_MainPage(int Width, int Height) : base(Width, Height)
		{
			TextFont = new Font("Arial", 10);
			using (var s = Assembly.GetExecutingAssembly().GetManifestResourceStream("QTRHacker.NewDimension.Res.Image.cross.png"))
				CrossImage = Image.FromStream(s);

			PlayerArrayBaseAddressInfoView = new InfoViewEx(220)
			{
				Bounds = new Rectangle(0, 60, Width, 20),
				Text = MainForm.CurrentLanguage["PlayerArrayAddress"]
			};
			Controls.Add(PlayerArrayBaseAddressInfoView);

			CurrentPlayerBaseAddressInfoView = new InfoViewEx(220)
			{
				Bounds = new Rectangle(0, 80, Width, 20),
				Text = MainForm.CurrentLanguage["MyPlayerAddress"]
			};
			Controls.Add(CurrentPlayerBaseAddressInfoView);

			CurrentPlayerInventoryBaseAddressInfoView = new InfoViewEx(220)
			{
				Bounds = new Rectangle(0, 100, Width, 20),
				Text = MainForm.CurrentLanguage["MyPlayerInvAddress"]
			};
			Controls.Add(CurrentPlayerInventoryBaseAddressInfoView);

			CurrentPlayerArmorBaseAddressInfoView = new InfoViewEx(220)
			{
				Bounds = new Rectangle(0, 120, Width, 20),
				Text = MainForm.CurrentLanguage["MyPlayerArmorAddress"]
			};
			Controls.Add(CurrentPlayerArmorBaseAddressInfoView);

			CurrentPlayerDyeBaseAddressInfoView = new InfoViewEx(220)
			{
				Bounds = new Rectangle(0, 140, Width, 20),
				Text = MainForm.CurrentLanguage["MyPlayerDyeAddress"]
			};
			Controls.Add(CurrentPlayerDyeBaseAddressInfoView);

			CurrentPlayerMiscBaseAddressInfoView = new InfoViewEx(220)
			{
				Bounds = new Rectangle(0, 160, Width, 20),
				Text = MainForm.CurrentLanguage["MyPlayerMiscAddress"]
			};
			Controls.Add(CurrentPlayerMiscBaseAddressInfoView);

			CurrentPlayerMiscDyeBaseAddressInfoView = new InfoViewEx(220)
			{
				Bounds = new Rectangle(0, 180, Width, 20),
				Text = MainForm.CurrentLanguage["MyPlayerMiscDyeAddress"]
			};
			Controls.Add(CurrentPlayerMiscDyeBaseAddressInfoView);

			CurrentPlayerBuffTypeBaseAddressInfoView = new InfoViewEx(220)
			{
				Bounds = new Rectangle(0, 200, Width, 20),
				Text = MainForm.CurrentLanguage["MyPlayerBuffAddress"]
			};
			Controls.Add(CurrentPlayerBuffTypeBaseAddressInfoView);

			CurrentPlayerBuffTimeBaseAddressInfoView = new InfoViewEx(220)
			{
				Bounds = new Rectangle(0, 220, Width, 20),
				Text = MainForm.CurrentLanguage["MyPlayerBuffTimeAddress"]
			};
			Controls.Add(CurrentPlayerBuffTimeBaseAddressInfoView);

			CurrentPlayerFirstItemBaseAddressInfoView = new InfoViewEx(220)
			{
				Bounds = new Rectangle(0, 240, Width, 20),
				Text = MainForm.CurrentLanguage["InvFirstItemAddress"]
			};
			Controls.Add(CurrentPlayerFirstItemBaseAddressInfoView);

			Terraria_Main_Update_BaseAddressInfoView = new InfoViewEx(220)
			{
				Bounds = new Rectangle(0, 260, Width, 20),
				Text = MainForm.CurrentLanguage["Terraria_Main_Update"]
			};
			Controls.Add(Terraria_Main_Update_BaseAddressInfoView);

			SignHeadBaseAddressInfoView = new InfoViewEx(220)
			{
				Bounds = new Rectangle(0, 280, Width, 20),
				Text = MainForm.CurrentLanguage["SignHead"]
			};
			Controls.Add(SignHeadBaseAddressInfoView);



			RefreshButton = new Button();
			RefreshButton.FlatStyle = FlatStyle.Flat;
			RefreshButton.ForeColor = Color.White;
			RefreshButton.Text = MainForm.CurrentLanguage["FetchAddressesAgain"];
			RefreshButton.Bounds = new Rectangle(Width - 130, Height - 40, 120, 30);
			RefreshButton.Click += (s, e) =>
			{
				if (HackContext.GameContext == null)
					return;
				InitializeAddresses();
			};
			Controls.Add(RefreshButton);
		}
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			if (!Dragging)
				e.Graphics.DrawImage(CrossImage, Width - 50, 15, 25, 25);
			e.Graphics.DrawString(MainForm.CurrentLanguage["DragTip"], TextFont, Brushes.White, Width - 280, 20);
		}
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (e.X >= Width - 50 && e.Y >= 15 && e.X <= Width - 50 + 25 && e.Y <= 15 + 25)
			{
				Dragging = true;
				Refresh();
			}
		}
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (e.X >= Width - 50 && e.Y >= 15 && e.X <= Width - 50 + 25 && e.Y <= 15 + 25)
			{
				this.Cursor = Cursors.Cross;
			}
			else
			{
				if (!Dragging)
					this.Cursor = Cursors.Default;
			}
		}

		private void InitGame(int pid)
		{
			HackContext.GameContext = GameContext.OpenGame(pid);
			HackContext.InitSign();
			//这个是获得最终信息的，必须最后执行
			InitializeAddresses();

		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			if (Dragging)
			{
				POINT p = new POINT();
				GetCursorPos(out p);
				IntPtr wnd;
				wnd = WindowFromPoint(p.X, p.Y);
				GetWindowThreadProcessId(wnd, out var processID);
				Dragging = false;
				Refresh();
				//获取游戏信息的操作最后执行，避免鼠标十字滞留
				InitGame(processID);

				MainForm.MainFormInstance.OnInitialized();
			}
		}
		public void InitializeAddresses()
		{
			var ctx = HackContext.GameContext;
			PlayerArrayBaseAddressInfoView.View.Text = ctx.Players.BaseAddress.ToString("X8");
			CurrentPlayerBaseAddressInfoView.View.Text = ctx.MyPlayer.BaseAddress.ToString("X8");
			CurrentPlayerInventoryBaseAddressInfoView.View.Text = ctx.MyPlayer.Inventory.BaseAddress.ToString("X8");
			CurrentPlayerArmorBaseAddressInfoView.View.Text = ctx.MyPlayer.Armor.BaseAddress.ToString("X8");
			CurrentPlayerDyeBaseAddressInfoView.View.Text = ctx.MyPlayer.Dye.BaseAddress.ToString("X8");
			CurrentPlayerMiscBaseAddressInfoView.View.Text = ctx.MyPlayer.Misc.BaseAddress.ToString("X8");
			CurrentPlayerMiscDyeBaseAddressInfoView.View.Text = ctx.MyPlayer.MiscDye.BaseAddress.ToString("X8");
			ctx.MyPlayer.ReadFromOffset(Player.OFFSET_BuffType, out int buffTypeBaseAddress);
			ctx.MyPlayer.ReadFromOffset(Player.OFFSET_BuffTime, out int buffTimeBaseAddress);
			CurrentPlayerBuffTypeBaseAddressInfoView.View.Text = buffTypeBaseAddress.ToString("X8");
			CurrentPlayerBuffTimeBaseAddressInfoView.View.Text = buffTimeBaseAddress.ToString("X8");
			CurrentPlayerFirstItemBaseAddressInfoView.View.Text = ctx.MyPlayer.Inventory[0].BaseAddress.ToString("X8");
			Terraria_Main_Update_BaseAddressInfoView.View.Text = ctx.HContext.MainAddressHelper["Terraria.Main", "Update"].ToString("X8");
			SignHeadBaseAddressInfoView.View.Text = HackContext.SignHead.ToString("X8");
		}
	}
}
