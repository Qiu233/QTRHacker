using QTRHacker.Functions.ProjectileMaker.Parse;
using QTRHacker.NewDimension.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QTRHacker.NewDimension.PagePanels
{
	public class PagePanel_Projectile : PagePanel
	{
		private MListBox FilesBox;
		public PagePanel_Projectile(int Width, int Height) : base(Width, Height)
		{
			FilesBox = new MListBox()
			{
				Bounds = new Rectangle(3, 3, 210, 364)
			};
			UpdateList();
			Controls.Add(FilesBox);

			Button CompileButton = new Button()
			{
				Text = MainForm.CurrentLanguage["Compile"],
				Bounds = new Rectangle(214, 3, 80, 30),
				FlatStyle = FlatStyle.Flat,
				BackColor = Color.FromArgb(100, 150, 150, 150)
			};
			CompileButton.Click += (s, e) =>
			{
				if (FilesBox.SelectedIndices.Count <= 0) return;
				var ctx = HackContext.GameContext;
				if (ctx == null)
				{
					MessageBox.Show(MainForm.CurrentLanguage["PleaseLockGame"]);
					return;
				}
				Parser p = new Parser(File.ReadAllText(($"./Projs/{(string)FilesBox.SelectedItem}.projimg")));
				try
				{
					var img = p.Parse();
					img.Emit(ctx, new Functions.ProjectileImage.MPointF(ctx.MyPlayer.X, ctx.MyPlayer.Y));
				}
				catch (ParseException ex)
				{
					MessageBox.Show($"{MainForm.CurrentLanguage["PleaseCheckCode"]}\n{MainForm.CurrentLanguage["Error"]}：\n" + ex.Message, $"{MainForm.CurrentLanguage["CompilationError"]}");
				}
				catch (Exception)
				{
					MessageBox.Show(MainForm.CurrentLanguage["UnknownError"], MainForm.CurrentLanguage["UnknownError"]);
				}
			};
			Controls.Add(CompileButton);

			Button CreateNewButton = new Button()
			{
				Text = MainForm.CurrentLanguage["Create"],
				Bounds = new Rectangle(214, 33, 80, 30),
				FlatStyle = FlatStyle.Flat,
				BackColor = Color.FromArgb(100, 150, 150, 150)
			};
			CreateNewButton.Click += (s, e) =>
			{
				MForm CreateNewMForm = new MForm
				{
					BackColor = Color.FromArgb(90, 90, 90),
					Text = MainForm.CurrentLanguage["Create"],
					StartPosition = FormStartPosition.CenterParent,
					ClientSize = new Size(245, 52)
				};

				Label NameTip = new Label()
				{
					Text = MainForm.CurrentLanguage["Name"] + "：",
					Location = new Point(0, 0),
					Size = new Size(80, 20),
					TextAlign = ContentAlignment.MiddleCenter
				};
				CreateNewMForm.MainPanel.Controls.Add(NameTip);

				TextBox NameTextBox = new TextBox
				{
					BorderStyle = BorderStyle.FixedSingle,
					BackColor = Color.FromArgb(120, 120, 120),
					Text = "",
					Location = new Point(85, 0),
					Size = new Size(95, 20)
				};
				CreateNewMForm.MainPanel.Controls.Add(NameTextBox);

				Button ConfirmButton = new Button();
				ConfirmButton.Text = MainForm.CurrentLanguage["Confirm"];
				ConfirmButton.FlatStyle = FlatStyle.Flat;
				ConfirmButton.Size = new Size(65, 20);
				ConfirmButton.Location = new Point(180, 0);
				ConfirmButton.Click += (s1, e1) =>
				{
					string str = $"./Projs/{NameTextBox.Text}.projimg";
					if (!File.Exists(str))
						File.Create(str).Close();
					else
						MessageBox.Show(MainForm.CurrentLanguage["NameRepeated"]);
					UpdateList();
					CreateNewMForm.Dispose();
				};
				CreateNewMForm.MainPanel.Controls.Add(ConfirmButton);
				CreateNewMForm.ShowDialog(this);
			};
			Controls.Add(CreateNewButton);

			Button EditButton = new Button()
			{
				Text = MainForm.CurrentLanguage["Edit"],
				Bounds = new Rectangle(214, 63, 80, 30),
				FlatStyle = FlatStyle.Flat,
				BackColor = Color.FromArgb(100, 150, 150, 150)
			};
			EditButton.Click += (s, e) =>
			{
				if (FilesBox.SelectedIndices.Count <= 0) return;
				ProjMakerForm p = new ProjMakerForm((string)FilesBox.SelectedItem);
				p.ShowDialog(this);
			};
			Controls.Add(EditButton);

			Button RenameButton = new Button()
			{
				Text = MainForm.CurrentLanguage["Rename"],
				Bounds = new Rectangle(214, 93, 80, 30),
				FlatStyle = FlatStyle.Flat,
				BackColor = Color.FromArgb(100, 150, 150, 150)
			};
			RenameButton.Click += (s, e) =>
			{
				if (FilesBox.SelectedIndices.Count <= 0) return;
				MForm CreateNewMForm = new MForm
				{
					BackColor = Color.FromArgb(90, 90, 90),
					Text = MainForm.CurrentLanguage["Rename"],
					StartPosition = FormStartPosition.CenterParent,
					ClientSize = new Size(245, 52)
				};

				Label NewNameTip = new Label()
				{
					Text = MainForm.CurrentLanguage["NewName"] + "：",
					Location = new Point(0, 0),
					Size = new Size(80, 20),
					TextAlign = ContentAlignment.MiddleCenter
				};
				CreateNewMForm.MainPanel.Controls.Add(NewNameTip);

				TextBox NewNameTextBox = new TextBox
				{
					BorderStyle = BorderStyle.FixedSingle,
					BackColor = Color.FromArgb(120, 120, 120),
					Text = "",
					Location = new Point(85, 0),
					Size = new Size(95, 20)
				};
				NewNameTextBox.Text = (string)FilesBox.SelectedItem;
				CreateNewMForm.MainPanel.Controls.Add(NewNameTextBox);

				Button ConfirmButton = new Button();
				ConfirmButton.Text = MainForm.CurrentLanguage["Confirm"];
				ConfirmButton.FlatStyle = FlatStyle.Flat;
				ConfirmButton.Size = new Size(65, 20);
				ConfirmButton.Location = new Point(180, 0);
				ConfirmButton.Click += (s1, e1) =>
				{
					string str = $"./Projs/{NewNameTextBox.Text}.projimg";
					if (!File.Exists(str))
						File.Move($"./Projs/{(string)FilesBox.SelectedItem}.projimg", str);
					else
						MessageBox.Show(MainForm.CurrentLanguage["NameRepeated"]);
					UpdateList();
					CreateNewMForm.Dispose();
				};
				CreateNewMForm.MainPanel.Controls.Add(ConfirmButton);
				CreateNewMForm.ShowDialog(this);
			};
			Controls.Add(RenameButton);

			Button DeleteButton = new Button()
			{
				Text = MainForm.CurrentLanguage["Delete"],
				Bounds = new Rectangle(214, 123, 80, 30),
				FlatStyle = FlatStyle.Flat,
				BackColor = Color.FromArgb(100, 150, 150, 150)
			};
			DeleteButton.Click += (s, e) =>
			{
				if (FilesBox.SelectedIndices.Count <= 0) return;
				if (MessageBox.Show(MainForm.CurrentLanguage["SureToDelete"], "Warning", MessageBoxButtons.OKCancel) == DialogResult.Cancel) return;
				File.Delete(($"./Projs/{(string)FilesBox.SelectedItem}.projimg"));
				UpdateList();
			};
			this.Controls.Add(DeleteButton);


			Button RefreshButton = new Button()
			{
				Text = MainForm.CurrentLanguage["Refresh"],
				Bounds = new Rectangle(214, 153, 80, 30),
				FlatStyle = FlatStyle.Flat,
				BackColor = Color.FromArgb(100, 150, 150, 150)
			};
			RefreshButton.Click += (s, e) =>
			{
				UpdateList();
			};
			Controls.Add(RefreshButton);
		}

		public void UpdateList()
		{
			FilesBox.Items.Clear();
			foreach (var f in Directory.EnumerateFiles("./Projs/", "*.projimg"))
			{
				FilesBox.Items.Add(Path.GetFileNameWithoutExtension(f));
			}
		}
	}
}
