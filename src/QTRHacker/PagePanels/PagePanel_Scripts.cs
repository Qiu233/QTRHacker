using QTRHacker.Functions.ProjectileMaker.Parse;
using QTRHacker.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QTRHacker.PagePanels
{
	public class PagePanel_Scripts : PagePanel
	{
		private readonly MListBox FilesBox;
		private readonly MButtonStrip ButtonStrip;
		public PagePanel_Scripts(int Width, int Height) : base(Width, Height)
		{
			FilesBox = new MListBox()
			{
				Bounds = new Rectangle(3, 3, 210, 364),
				ForeColor = Color.White,
			};
			UpdateList();
			Controls.Add(FilesBox);

			ButtonStrip = new MButtonStrip(80, 30)
			{
				Bounds = new Rectangle(215, 2, 80, 210),
			};
			Controls.Add(ButtonStrip);

			ButtonStrip.AddButton(HackContext.CurrentLanguage["Execute"]).Click += (s, e) =>
			{
				if (FilesBox.SelectedIndices.Count <= 0) return;
				var ctx = HackContext.GameContext;
				if (ctx == null)
				{
					MessageBox.Show(HackContext.CurrentLanguage["PleaseLockGame"]);
					return;
				}
				string h = Path.Combine(HackContext.PATH_SCRIPTS, $"{(string)FilesBox.SelectedItem}.qhscript");
				string t = File.ReadAllText(Path.Combine(HackContext.PATH_SCRIPTS, $"{(string)FilesBox.SelectedItem}.qhscript"));
				var scope = HackContext.CreateScriptScope(HackContext.QHScriptEngine);
				HackContext.QHScriptEngine.Execute(t, scope);
			};

			ButtonStrip.AddButton(HackContext.CurrentLanguage["Create"]).Click += (s, e) =>
			{
				MForm CreateNewMForm = new MForm
				{
					BackColor = Color.FromArgb(90, 90, 90),
					Text = HackContext.CurrentLanguage["Create"],
					StartPosition = FormStartPosition.CenterParent,
					ClientSize = new Size(245, 52)
				};

				Label NameTip = new Label()
				{
					Text = HackContext.CurrentLanguage["Name"] + "：",
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
				ConfirmButton.Text = HackContext.CurrentLanguage["Confirm"];
				ConfirmButton.FlatStyle = FlatStyle.Flat;
				ConfirmButton.Size = new Size(65, 20);
				ConfirmButton.Location = new Point(180, 0);
				ConfirmButton.Click += (s1, e1) =>
				{
					string str = Path.Combine(HackContext.PATH_SCRIPTS, $"{NameTextBox.Text}.qhscript");
					if (!File.Exists(str))
						File.Create(str).Close();
					else
						MessageBox.Show(HackContext.CurrentLanguage["NameRepeated"]);
					UpdateList();
					CreateNewMForm.Dispose();
				};
				CreateNewMForm.MainPanel.Controls.Add(ConfirmButton);
				CreateNewMForm.ShowDialog(this);
			};

			ButtonStrip.AddButton(HackContext.CurrentLanguage["Edit"]).Click += (s, e) =>
			{
				if (FilesBox.SelectedIndices.Count <= 0) return;
				ScriptEditorForm p = new ScriptEditorForm((string)FilesBox.SelectedItem);
				p.Show();
			};

			ButtonStrip.AddButton(HackContext.CurrentLanguage["Rename"]).Click += (s, e) =>
			{
				if (FilesBox.SelectedIndices.Count <= 0) return;
				MForm CreateNewMForm = new MForm
				{
					BackColor = Color.FromArgb(90, 90, 90),
					Text = HackContext.CurrentLanguage["Rename"],
					StartPosition = FormStartPosition.CenterParent,
					ClientSize = new Size(245, 52)
				};

				Label NewNameTip = new Label()
				{
					Text = HackContext.CurrentLanguage["NewName"] + "：",
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
				ConfirmButton.Text = HackContext.CurrentLanguage["Confirm"];
				ConfirmButton.FlatStyle = FlatStyle.Flat;
				ConfirmButton.Size = new Size(65, 20);
				ConfirmButton.Location = new Point(180, 0);
				ConfirmButton.Click += (s1, e1) =>
				{
					string str = Path.Combine(HackContext.PATH_SCRIPTS, $"{NewNameTextBox.Text}.qhscript");
					if (!File.Exists(str))
						File.Move(Path.Combine(HackContext.PATH_SCRIPTS, $"{(string)FilesBox.SelectedItem}.qhscript"), str);
					else
						MessageBox.Show(HackContext.CurrentLanguage["NameRepeated"]);
					UpdateList();
					CreateNewMForm.Dispose();
				};
				CreateNewMForm.MainPanel.Controls.Add(ConfirmButton);
				CreateNewMForm.ShowDialog(this);
			};

			ButtonStrip.AddButton(HackContext.CurrentLanguage["Delete"]).Click += (s, e) =>
			{
				if (FilesBox.SelectedIndices.Count <= 0) return;
				if (MessageBox.Show(HackContext.CurrentLanguage["SureToDelete"], "Warning", MessageBoxButtons.OKCancel) == DialogResult.Cancel) return;
				File.Delete(Path.Combine(HackContext.PATH_SCRIPTS, $"{(string)FilesBox.SelectedItem}.qhscript"));
				UpdateList();
			};

			ButtonStrip.AddButton(HackContext.CurrentLanguage["Refresh"]).Click += (s, e) =>
			{
				UpdateList();
			};
		}

		public void UpdateList()
		{
			FilesBox.Items.Clear();
			foreach (var f in Directory.EnumerateFiles(HackContext.PATH_SCRIPTS, "*.qhscript"))
			{
				FilesBox.Items.Add(Path.GetFileNameWithoutExtension(f));
			}
		}
	}
}
