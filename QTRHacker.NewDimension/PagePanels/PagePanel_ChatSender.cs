using QTRHacker.Functions;
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
	public class PagePanel_ChatSender : PagePanel
	{
		public struct ChatSnippet
		{
			public int Color
			{
				get;
				set;
			}
			public string Content
			{
				get;
				set;
			}
			public ChatSnippet(int color, string content)
			{
				Color = color;
				Content = content;
			}
		}
		private MListBox FilesBox;
		private Panel ContentPanel;
		private Button AddSnippetButton;
		public PagePanel_ChatSender(int Width, int Height) : base(Width, Height)
		{
			FilesBox = new MListBox()
			{
				Bounds = new Rectangle(63, 3, 170, 150)
			};
			UpdateList();
			Controls.Add(FilesBox);

			ContentPanel = new Panel();
			ContentPanel.Bounds = new Rectangle(3, 150, 290, 215);
			ContentPanel.BackColor = Color.FromArgb(60, 60, 60);
			ContentPanel.AutoScroll = true;
			Controls.Add(ContentPanel);

			AddSnippetButton = new Button();
			AddSnippetButton.FlatStyle = FlatStyle.Flat;
			AddSnippetButton.Text = "添加";
			AddSnippetButton.ForeColor = Color.White;
			AddSnippetButton.Size = new Size(40, 20);
			AddSnippetButton.Font = new Font(AddSnippetButton.Font.Name, 7);

			AddSnippetButton.Click += (s, e) =>
			{
				AddNewSnippet();
			};

			FilesBox.SelectedIndexChanged += FilesBox_SelectedIndexChanged;

			Button CreateButton = new Button()
			{
				Text = MainForm.CurrentLanguage["Create"],
				Bounds = new Rectangle(1, 2, 60, 30),
				FlatStyle = FlatStyle.Flat,
				BackColor = Color.FromArgb(100, 150, 150, 150)
			};
			Controls.Add(CreateButton);
			CreateButton.Click += (s, e) =>
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
					string str = $"./ChatTemplates/{NameTextBox.Text}.chat";
					if (!File.Exists(str))
						File.Create(str).Close();
					else
					{
						MessageBox.Show(MainForm.CurrentLanguage["NameRepeated"]);
						return;
					}
					using (var bw = new BinaryWriter(File.Open(str, FileMode.Open)))
					{
						bw.Write(0);
					}
					UpdateList();
					CreateNewMForm.Dispose();
				};
				CreateNewMForm.MainPanel.Controls.Add(ConfirmButton);
				CreateNewMForm.ShowDialog(this);
			};
			Button RenameButton = new Button()
			{
				Text = MainForm.CurrentLanguage["Rename"],
				Bounds = new Rectangle(1, 32, 60, 30),
				FlatStyle = FlatStyle.Flat,
				BackColor = Color.FromArgb(100, 150, 150, 150)
			};
			Controls.Add(RenameButton);
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
					string str = $"./ChatTemplates/{NewNameTextBox.Text}.chat";
					if (!File.Exists(str))
						File.Move($"./ChatTemplates/{(string)FilesBox.SelectedItem}.chat", str);
					else
						MessageBox.Show(MainForm.CurrentLanguage["NameRepeated"]);
					UpdateList();
					CreateNewMForm.Dispose();
				};
				CreateNewMForm.MainPanel.Controls.Add(ConfirmButton);
				CreateNewMForm.ShowDialog(this);
			};

			Button DeleteButton = new Button()
			{
				Text = MainForm.CurrentLanguage["Delete"],
				Bounds = new Rectangle(1, 62, 60, 30),
				FlatStyle = FlatStyle.Flat,
				BackColor = Color.FromArgb(100, 150, 150, 150)
			};
			Controls.Add(DeleteButton);
			DeleteButton.Click += (s, e) =>
			{
				if (FilesBox.SelectedIndices.Count <= 0) return;
				if (MessageBox.Show(MainForm.CurrentLanguage["SureToDelete"], "Warning", MessageBoxButtons.OKCancel) == DialogResult.Cancel) return;
				File.Delete(($"./ChatTemplates/{(string)FilesBox.SelectedItem}.chat"));
				UpdateList();
			};

			Button RefreshButton = new Button()
			{
				Text = MainForm.CurrentLanguage["Refresh"],
				Bounds = new Rectangle(1, 92, 60, 30),
				FlatStyle = FlatStyle.Flat,
				BackColor = Color.FromArgb(100, 150, 150, 150)
			};
			Controls.Add(RefreshButton);
			RefreshButton.Click += (s, e) =>
			{
				UpdateList();
			};


			Button SaveButton = new Button()
			{
				Text = MainForm.CurrentLanguage["Save"],
				Bounds = new Rectangle(233, 2, 60, 30),
				FlatStyle = FlatStyle.Flat,
				BackColor = Color.FromArgb(100, 150, 150, 150)
			};
			Controls.Add(SaveButton);
			SaveButton.Click += (s, e) =>
			{
				if (FilesBox.SelectedIndices.Count > 0)
					SaveFile("./ChatTemplates/" + FilesBox.SelectedItem.ToString() + ".chat");
			};


			Button SendButton = new Button()
			{
				Text = MainForm.CurrentLanguage["Send"],
				Bounds = new Rectangle(233, 32, 60, 30),
				FlatStyle = FlatStyle.Flat,
				BackColor = Color.FromArgb(100, 150, 150, 150)
			};
			Controls.Add(SendButton);
			SendButton.Click += (s, e) =>
			{
				if (FilesBox.SelectedIndices.Count > 0)
					SendChat();
			};
		}

		private void SendChat()
		{
			Utils.SendChat(HackContext.GameContext, GenerateText(GenerateSnippets()));
		}

		private static string GenerateText(List<ChatSnippet> snippets)
		{
			StringBuilder sb = new StringBuilder();
			foreach (var s in snippets)
			{
				if (s.Color == 0xFFFFFF)
					sb.Append($"{s.Content}");
				else
					sb.Append($"[C/{s.Color.ToString("X8")}:{s.Content}]");
			}
			return sb.ToString();
		}

		private List<ChatSnippet> GenerateSnippets()
		{
			List<ChatSnippet> snippets = new List<ChatSnippet>();
			foreach (var c in ContentPanel.Controls)
			{
				if (c is ChatSnippetBox cs)
				{
					snippets.Add(new ChatSnippet(Convert.ToInt32(cs.ColorBox.Text, 16), cs.ContentBox.Text));
				}
			}
			return snippets;
		}

		private void SaveFile(string file)
		{
			WriteToFile(file, GenerateSnippets());
		}

		private ChatSnippetBox AddNewSnippet()
		{
			ChatSnippetBox b = new ChatSnippetBox();
			b.Location = AddSnippetButton.Location;
			b.OnDeleteClicked += s =>
			{
				DeleteSnippet(s);
			};
			ContentPanel.Controls.Add(b);
			AddSnippetButton.Location = new Point(AddSnippetButton.Location.X, AddSnippetButton.Location.Y + 20);
			return b;
		}

		private void DeleteSnippet(ChatSnippetBox b)
		{
			int index = ContentPanel.Controls.IndexOf(b);
			AddSnippetButton.Location = ContentPanel.Controls[ContentPanel.Controls.Count - 1].Location;
			for (int i = index + 1; i < ContentPanel.Controls.Count; i++)
			{
				var c = ContentPanel.Controls[i];
				c.Location = new Point(c.Location.X, c.Location.Y - 20);
			}
			ContentPanel.Controls.RemoveAt(index);
		}

		private void ClearSnippet()
		{
			ContentPanel.Controls.Clear();
			if (FilesBox.SelectedIndices.Count > 0)
				ContentPanel.Controls.Add(AddSnippetButton);
			AddSnippetButton.Location = new Point(0, 0);
		}

		private void FilesBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			ClearSnippet();
			if (FilesBox.SelectedIndices.Count > 0)
			{
				var Snippets = ReadFromFile("./ChatTemplates/" + FilesBox.SelectedItem.ToString() + ".chat");
				foreach (var s in Snippets)
				{
					var box = AddNewSnippet();
					box.ColorBox.Text = s.Color.ToString("X6");
					box.ContentBox.Text = s.Content;
				}
			}
		}

		private List<ChatSnippet> ReadFromFile(string file)
		{
			FileStream s = File.Open(file, FileMode.Open);
			BinaryReader br = new BinaryReader(s);
			var Snippets = new List<ChatSnippet>(br.ReadInt32());
			for (int i = 0; i < Snippets.Capacity; i++)
			{
				int color = br.ReadInt32();
				string content = br.ReadString();
				Snippets.Add(new ChatSnippet(color, content));
			}
			br.Close();
			s.Close();
			return Snippets;
		}

		private void WriteToFile(string file, List<ChatSnippet> Snippets)
		{
			FileStream s = File.Open(file, FileMode.Open);
			BinaryWriter bw = new BinaryWriter(s);
			bw.Write(Snippets.Count);
			foreach (var snippet in Snippets)
			{
				bw.Write(snippet.Color);
				bw.Write(snippet.Content);
			}
			bw.Close();
			s.Close();
		}


		public void UpdateList()
		{
			FilesBox.Items.Clear();
			foreach (var f in Directory.EnumerateFiles("./ChatTemplates/", "*.chat"))
			{
				FilesBox.Items.Add(Path.GetFileNameWithoutExtension(f));
			}
		}
	}
}
