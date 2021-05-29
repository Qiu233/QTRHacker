using QHackLib;
using QTRHacker.Functions;
using QTRHacker.NewDimension.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QTRHacker.NewDimension.PagePanels
{
	public class PagePanel_Sches : PagePanel
	{
		[StructLayout(LayoutKind.Sequential)]
		private struct CTile
		{
			public ushort Type;
			public byte Wall;
			public byte Liquid;
			public byte BTileHeader;
			public byte BTileHeader2;
			public byte BTileHeader3;
			public short FrameX;
			public short FrameY;
			public short STileHeader;
		}
		private MListBox FilesBox;

		private CTile[,] LoadTilesFromFile(string file)
		{
			var fs = File.Open(file, FileMode.Open);
			BinaryReader br = new BinaryReader(fs);
			int maxX = br.ReadInt32();
			int maxY = br.ReadInt32();
			var tiles = new CTile[maxX, maxY];

			for (int x = 0; x < tiles.GetLength(0); x++)
			{
				for (int y = 0; y < tiles.GetLength(1); y++)
				{
					tiles[x, y] = new CTile()
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

		private byte[] SerializeTiles(CTile[,] tiles)
		{
			int unitSize = Marshal.SizeOf(typeof(CTile));
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
			FilesBox = new MListBox()
			{
				Bounds = new Rectangle(3, 3, 200, 364)
			};
			UpdateList();
			Controls.Add(FilesBox);

			Button ExecuteButton = new Button()
			{
				Text = HackContext.CurrentLanguage["Generate"],
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
					MessageBox.Show(HackContext.CurrentLanguage["PleaseLockGame"]);
					return;
				}
				string h = Path.Combine(HackContext.PATH_SCHES, $"{FilesBox.SelectedItem}.sche");

				var bs = SerializeTiles(LoadTilesFromFile(h));
				int maddr = NativeFunctions.VirtualAllocEx(ctx.HContext.Handle, 0, bs.Length, NativeFunctions.AllocationType.MEM_COMMIT, NativeFunctions.ProtectionType.PAGE_EXECUTE_READWRITE);
				NativeFunctions.WriteProcessMemory(ctx.HContext.Handle, maddr, bs, bs.Length, 0);
				CLRFunctionCaller.Call(ctx, "TRInjections.dll", "TRInjections.ScheMaker.ScheMaker", "LoadTiles",
					ctx.HContext.MainAddressHelper.GetFunctionAddress("Terraria.Main", "DoUpdate"), maddr);
				ctx.HContext.GetAddressHelper("TRInjections.dll").SetStaticFieldValue("TRInjections.ScheMaker.ScheMaker", "BrushActive", true);
				NativeFunctions.VirtualFreeEx(ctx.HContext.Handle, maddr, 0);


			};
			Controls.Add(ExecuteButton);

			Button CreateNewButton = new Button()
			{
				Text = HackContext.CurrentLanguage["Create"],
				Bounds = new Rectangle(204, 33, 90, 30),
				FlatStyle = FlatStyle.Flat,
				BackColor = Color.FromArgb(100, 150, 150, 150)
			};
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
			Controls.Add(CreateNewButton);

			Button EditButton = new Button()
			{
				Text = HackContext.CurrentLanguage["Edit"],
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
				Text = HackContext.CurrentLanguage["Rename"],
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
			Controls.Add(RenameButton);

			Button DeleteButton = new Button()
			{
				Text = HackContext.CurrentLanguage["Delete"],
				Bounds = new Rectangle(204, 123, 90, 30),
				FlatStyle = FlatStyle.Flat,
				BackColor = Color.FromArgb(100, 150, 150, 150)
			};
			DeleteButton.Click += (s, e) =>
			{
				if (FilesBox.SelectedIndices.Count <= 0) return;
				if (MessageBox.Show(HackContext.CurrentLanguage["SureToDelete"], "Warning", MessageBoxButtons.OKCancel) == DialogResult.Cancel) return;
				File.Delete(Path.Combine(HackContext.PATH_SCHES, $"{(string)FilesBox.SelectedItem}.sche"));
				UpdateList();
			};
			this.Controls.Add(DeleteButton);


			Button RefreshButton = new Button()
			{
				Text = HackContext.CurrentLanguage["Refresh"],
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
			foreach (var f in Directory.EnumerateFiles(HackContext.PATH_SCHES, "*.sche"))
			{
				FilesBox.Items.Add(Path.GetFileNameWithoutExtension(f));
			}
		}
	}
}
