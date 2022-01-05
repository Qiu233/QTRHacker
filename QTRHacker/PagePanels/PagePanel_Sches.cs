using QHackLib;
using QTRHacker.Functions;
using QTRHacker.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using STile = QTRHacker.Contrast.Structs.STile;

namespace QTRHacker.PagePanels
{
	public class PagePanel_Sches : PagePanel
	{
		private readonly MListBox FilesBox;
		private readonly MButtonStrip ButtonStrip;
		private static STile[,] LoadTilesFromFile(string file)
		{
			var fs = File.Open(file, FileMode.Open);
			BinaryReader br = new BinaryReader(fs);
			int maxX = br.ReadInt32();
			int maxY = br.ReadInt32();
			var tiles = new STile[maxX, maxY];

			for (int x = 0; x < tiles.GetLength(0); x++)
			{
				for (int y = 0; y < tiles.GetLength(1); y++)
				{
					tiles[x, y] = new STile()
					{
						Type = br.ReadUInt16(),
						Wall = br.ReadByte(),
						Liquid = br.ReadByte(),
						BTileHeader = br.ReadByte(),
						BTileHeader2 = br.ReadByte(),
						BTileHeader3 = br.ReadByte(),
						FrameX = br.ReadInt16(),
						FrameY = br.ReadInt16(),
						STileHeader = br.ReadInt16()
					};
				}
			}
			fs.Close();
			return tiles;
		}

		private static byte[] SerializeTiles(STile[,] tiles)
		{
			int unitSize = Marshal.SizeOf(typeof(STile));
			int memorySize = 4 + 4 + tiles.GetLength(0) * tiles.GetLength(1) * unitSize;
			byte[] bs = new byte[memorySize];
			byte[] tmpS = new byte[unitSize];
			MemoryStream ms = new MemoryStream(bs);
			BinaryWriter bw = new BinaryWriter(ms);
			bw.Write(tiles.GetLength(0));
			bw.Write(tiles.GetLength(1));
			for (int x = 0; x < tiles.GetLength(0); x++)
			{
				for (int y = 0; y < tiles.GetLength(1); y++)
				{
					IntPtr ptr = Marshal.AllocHGlobal(unitSize);
					Marshal.StructureToPtr(tiles[x, y], ptr, false);
					Marshal.Copy(ptr, tmpS, 0, unitSize);
					Marshal.FreeHGlobal(ptr);
					bw.Write(tmpS);
				}
			}
			ms.Close();
			return bs;
		}

		public PagePanel_Sches(int Width, int Height) : base(Width, Height)
		{
			ButtonStrip = new MButtonStrip(80, 30);
			ButtonStrip.Bounds = new Rectangle(215, 2, 80, 210);
			ButtonStrip.Enabled = false;
			Controls.Add(ButtonStrip);

			FilesBox = new MListBox()
			{
				Bounds = new Rectangle(3, 3, 210, 364)
			};
			UpdateList();
			Controls.Add(FilesBox);


			Button GenerateButton = ButtonStrip.AddButton(HackContext.CurrentLanguage["Generate"]);
			GenerateButton.Click += (s, e) =>
			{
				if (FilesBox.SelectedIndices.Count <= 0) return;
				var ctx = HackContext.GameContext;
				if (ctx == null)
				{
					MessageBox.Show(HackContext.CurrentLanguage["PleaseLockGame"]);
					return;
				}
				string h = Path.Combine(HackContext.PATH_SCHES, $"{FilesBox.SelectedItem}.sche");

				var bs = SerializeTiles(LoadTilesFromFile(h));


			};

			Button CreateNewButton = ButtonStrip.AddButton(HackContext.CurrentLanguage["Create"]);
			CreateNewButton.Click += (s, e) =>
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
					string str = Path.Combine(HackContext.PATH_SCHES, $"{NameTextBox.Text}.sche");
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

			Button EditButton = ButtonStrip.AddButton(HackContext.CurrentLanguage["Edit"]);
			EditButton.Click += (s, e) =>
			{
				if (FilesBox.SelectedIndices.Count <= 0) return;
				ScriptEditorForm p = new ScriptEditorForm((string)FilesBox.SelectedItem);
				p.Show();
			};

			Button RenameButton = ButtonStrip.AddButton(HackContext.CurrentLanguage["Rename"]);
			RenameButton.Click += (s, e) =>
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
					string str = Path.Combine(HackContext.PATH_SCHES, $"{NewNameTextBox.Text}.sche");
					if (!File.Exists(str))
						File.Move(Path.Combine(HackContext.PATH_SCHES, $"{(string)FilesBox.SelectedItem}.sche"), str);
					else
						MessageBox.Show(HackContext.CurrentLanguage["NameRepeated"]);
					UpdateList();
					CreateNewMForm.Dispose();
				};
				CreateNewMForm.MainPanel.Controls.Add(ConfirmButton);
				CreateNewMForm.ShowDialog(this);
			};

			Button DeleteButton = ButtonStrip.AddButton(HackContext.CurrentLanguage["Delete"]);
			DeleteButton.Click += (s, e) =>
			{
				if (FilesBox.SelectedIndices.Count <= 0) return;
				if (MessageBox.Show(HackContext.CurrentLanguage["SureToDelete"], "Warning", MessageBoxButtons.OKCancel) == DialogResult.Cancel) return;
				File.Delete(Path.Combine(HackContext.PATH_SCHES, $"{(string)FilesBox.SelectedItem}.sche"));
				UpdateList();
			};


			Button RefreshButton = ButtonStrip.AddButton(HackContext.CurrentLanguage["Refresh"]);
			RefreshButton.Click += (s, e) =>
			{
				UpdateList();
			};
		}

		public void UpdateList()
		{
			FilesBox.Items.Clear();
			foreach (var f in Directory.EnumerateFiles(HackContext.PATH_SCHES, "*.sche"))
			{
				FilesBox.Items.Add(Path.GetFileNameWithoutExtension(f));
			}
		}
	}
}
