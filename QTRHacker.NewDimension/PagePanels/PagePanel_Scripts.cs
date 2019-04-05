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
	public class PagePanel_Scripts : PagePanel
	{
		private MListBox FilesBox;
		public PagePanel_Scripts(int Width, int Height) : base(Width, Height)
		{
			FilesBox = new MListBox()
			{
				Bounds = new Rectangle(3, 3, 200, 364)
			};
			UpdateList();
			Controls.Add(FilesBox);

			Button ExecuteButton = new Button()
			{
				Text = MainForm.CurrentLanguage["Execute"],
				Bounds = new Rectangle(204, 3, 90, 30),
				FlatStyle = FlatStyle.Flat,
				BackColor = Color.FromArgb(100, 150, 150, 150)
			};
			ExecuteButton.Click += (s, e) =>
			{
				if (FilesBox.SelectedIndices.Count <= 0) return;
				var ctx = HackContext.GameContext;
				if (ctx == null)
				{
					MessageBox.Show(MainForm.CurrentLanguage["PleaseLockGame"]);
					return;
				}
				string h = $"./Scripts/{(string)FilesBox.SelectedItem}.qhscript";
				string t = File.ReadAllText(($"./Scripts/{(string)FilesBox.SelectedItem}.qhscript"));
				var scope = HackContext.CreateScriptScope(MainForm.QHScriptEngine);
				MainForm.QHScriptEngine.Execute(t, scope);
			};
			Controls.Add(ExecuteButton);

			Button CreateNewButton = new Button()
			{
				Text = MainForm.CurrentLanguage["Create"],
				Bounds = new Rectangle(204, 33, 90, 30),
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
					string str = $"./Scripts/{NameTextBox.Text}.qhscript";
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
				Bounds = new Rectangle(204, 63, 90, 30),
				FlatStyle = FlatStyle.Flat,
				BackColor = Color.FromArgb(100, 150, 150, 150)
			};
			EditButton.Click += (s, e) =>
			{
				if (FilesBox.SelectedIndices.Count <= 0) return;
				ScriptEditorForm p = new ScriptEditorForm((string)FilesBox.SelectedItem);
				p.Show();
			};
			Controls.Add(EditButton);

			Button RenameButton = new Button()
			{
				Text = MainForm.CurrentLanguage["Rename"],
				Bounds = new Rectangle(204, 93, 90, 30),
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
					string str = $"./Scripts/{NewNameTextBox.Text}.qhscript";
					if (!File.Exists(str))
						File.Move($"./Scripts/{(string)FilesBox.SelectedItem}.qhscript", str);
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
				Bounds = new Rectangle(204, 123, 90, 30),
				FlatStyle = FlatStyle.Flat,
				BackColor = Color.FromArgb(100, 150, 150, 150)
			};
			DeleteButton.Click += (s, e) =>
			{
				if (FilesBox.SelectedIndices.Count <= 0) return;
				if (MessageBox.Show(MainForm.CurrentLanguage["SureToDelete"], "Warning", MessageBoxButtons.OKCancel) == DialogResult.Cancel) return;
				File.Delete(($"./Scripts/{(string)FilesBox.SelectedItem}.qhscript"));
				UpdateList();
			};
			this.Controls.Add(DeleteButton);


			Button RefreshButton = new Button()
			{
				Text = MainForm.CurrentLanguage["Refresh"],
				Bounds = new Rectangle(204, 153, 90, 30),
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
			foreach (var f in Directory.EnumerateFiles("./Scripts/", "*.qhscript"))
			{
				FilesBox.Items.Add(Path.GetFileNameWithoutExtension(f));
			}
		}
	}
}
