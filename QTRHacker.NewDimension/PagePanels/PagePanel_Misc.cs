using QTRHacker.Functions.GameObjects;
using QTRHacker.NewDimension.Controls;
using QTRHacker.NewDimension.Res;
using QTRHacker.NewDimension.Wiki;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QTRHacker.NewDimension.PagePanels
{
	public class PagePanel_Misc : PagePanel
	{
		public int ButtonsCount { get; set; }
		public PagePanel_Misc(int Width, int Height) : base(Width, Height)
		{
			ButtonsCount = 0;
			Image img_Wiki, img_Player;
			using (Stream st = new MemoryStream(GameResLoader.ItemImageData["Item_3628"]))
				img_Wiki = Image.FromStream(st);
			using (Stream st = Assembly.GetExecutingAssembly().GetManifestResourceStream("QTRHacker.NewDimension.Res.Image.player.png"))
				img_Player = Image.FromStream(st);
			AddButton(img_Wiki, MainForm.CurrentLanguage["ItemWiki"], () => { new WikiForm().Show(); });
			AddButton(img_Player, MainForm.CurrentLanguage["SummonNPC"], () =>
			{
				MForm SummonNPCMForm = new MForm
				{
					BackColor = Color.FromArgb(90, 90, 90),
					Text = MainForm.CurrentLanguage["SummonNPC"],
					StartPosition = FormStartPosition.CenterParent,
					ClientSize = new Size(245, 72)
				};

				Label NPCTypeTip = new Label()
				{
					Text = MainForm.CurrentLanguage["NPCType"],
					Location = new Point(0, 0),
					Size = new Size(80, 20),
					TextAlign = ContentAlignment.MiddleCenter
				};
				SummonNPCMForm.MainPanel.Controls.Add(NPCTypeTip);

				TextBox NPCID = new TextBox
				{
					BorderStyle = BorderStyle.FixedSingle,
					BackColor = Color.FromArgb(120, 120, 120),
					Text = "50",
					Location = new Point(85, 0),
					Size = new Size(95, 20)
				};
				NPCID.KeyPress += (s1, e1) => e1.Handled = e1.Handled || (!Char.IsNumber(e1.KeyChar) && e1.KeyChar != 8 && e1.KeyChar != '-');
				SummonNPCMForm.MainPanel.Controls.Add(NPCID);


				Label TimesTip = new Label()
				{
					Text = MainForm.CurrentLanguage["Number"],
					Location = new Point(0, 20),
					Size = new Size(80, 20),
					TextAlign = ContentAlignment.MiddleCenter
				};
				SummonNPCMForm.MainPanel.Controls.Add(TimesTip);

				TextBox Times = new TextBox
				{
					BorderStyle = BorderStyle.FixedSingle,
					BackColor = Color.FromArgb(120, 120, 120),
					Text = "10",
					Location = new Point(85, 20),
					Size = new Size(95, 20)
				};
				Times.KeyPress += (s1, e1) => e1.Handled = e1.Handled || (!Char.IsNumber(e1.KeyChar) && e1.KeyChar != 8 && e1.KeyChar != '-');
				SummonNPCMForm.MainPanel.Controls.Add(Times);

				Button ConfirmButton = new Button();
				ConfirmButton.Text = MainForm.CurrentLanguage["Confirm"];
				ConfirmButton.FlatStyle = FlatStyle.Flat;
				ConfirmButton.Size = new Size(65, 40);
				ConfirmButton.Location = new Point(180, 0);
				ConfirmButton.Click += (s1, e1) =>
				{
					var ctx = HackContext.GameContext;
					var plr = ctx.MyPlayer;
					for (int i = 0; i < Convert.ToInt32(Times.Text); i++)
					{
						NPC.NewNPC(ctx, (int)plr.X, (int)plr.Y - 50, Convert.ToInt32(NPCID.Text));
					}
					SummonNPCMForm.Dispose();
				};
				SummonNPCMForm.MainPanel.Controls.Add(ConfirmButton);
				SummonNPCMForm.ShowDialog(this);
			});
		}
		public virtual ImageButton AddButton(Image img, string txt, Action onclick)
		{
			ImageButton btn = new ImageButton();
			btn.BorderStyle = BorderStyle.FixedSingle;
			btn.Image = img;
			btn.Text = txt;
			btn.Click += (s, e) => onclick();

			btn.Location = new Point(20 + ButtonsCount % 2 * 150, 10 + ButtonsCount / 2 * 30);
			ButtonsCount++;
			Controls.Add(btn);
			return btn;
		}
	}
}
