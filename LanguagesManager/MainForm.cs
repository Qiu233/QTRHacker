using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LanguagesManager
{
	public partial class MainForm : Form
	{
		public TextBox AddBox;
		public TextBox SearchBox;
		public CDataGridView LanguageData;
		public Config CFG;
		public const string CFG_FILE = "./config.json";
		public Button SaveButton, SearchButton, AddButton, DeleteButton;
		public Dictionary<string, Dictionary<string, string>> Data;
		public MainForm()
		{
			InitializeComponent();

			Data = new Dictionary<string, Dictionary<string, string>>();

			LanguageData = new CDataGridView();
			LanguageData.Bounds = new Rectangle(0, 0, 550, 500);
			Controls.Add(LanguageData);

			Button ReloadButton = new Button()
			{
				Text = "Reload",
				Bounds = new Rectangle(552, 0, 100, 30),
				FlatStyle = FlatStyle.Flat
			};
			ReloadButton.Click += (s, e) =>
			{
				if (MessageBox.Show("将从语言文件读取内容\n未保存的数据将会永久丢失\n是否继续", "警告", MessageBoxButtons.OKCancel) == DialogResult.OK)
				{
					LoadData();
				}
			};
			Controls.Add(ReloadButton);

			SaveButton = new Button()
			{
				Text = "Save",
				Bounds = new Rectangle(652, 0, 100, 30),
				FlatStyle = FlatStyle.Flat
			};
			SaveButton.Click += (s, e) =>
			{
				if (MessageBox.Show("将会覆盖现有语言文件，是否继续？", "警告", MessageBoxButtons.OKCancel) == DialogResult.OK)
				{
					Save();
					MessageBox.Show("保存成功");
				}
			};
			Controls.Add(SaveButton);

			SearchBox = new CTextBox();
			SearchBox.TextAlign = HorizontalAlignment.Center;
			SearchBox.Bounds = new Rectangle(551, 30, 201, 0);
			SearchBox.KeyPress += (s, e) =>
			{
				if (e.KeyChar == (char)13)
				{
					SearchButton.PerformClick();
					e.Handled = true;
				}
			};
			Controls.Add(SearchBox);

			SearchButton = new Button()
			{
				Text = "Search Index",
				Bounds = new Rectangle(552, 56, 199, 30),
				FlatStyle = FlatStyle.Flat
			};
			SearchButton.Click += (s, e) =>
			{
				int snum = LanguageData.SelectedRows.Count;
				int i = snum > 0 ? LanguageData.SelectedRows[0].Index + 1 : 0;
				int maxnum = LanguageData.Rows.Count;
				bool flag = false;
				flag = false;
				for (; i < maxnum; i++)
				{
					if ((LanguageData.Rows[i].HeaderCell.Value as string).Contains(SearchBox.Text.Trim()))
					{
						LanguageData.ClearSelection();
						LanguageData.Rows[i].Selected = true;
						LanguageData.CurrentCell = LanguageData.Rows[i].Cells[0];
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					LanguageData.ClearSelection();
				}
			};
			Controls.Add(SearchButton);

			AddBox = new CTextBox();
			AddBox.TextAlign = HorizontalAlignment.Center;
			AddBox.Bounds = new Rectangle(551, 86, 201, 0);
			Controls.Add(AddBox);
			AddBox.KeyPress += (s, e) =>
			{
				if (e.KeyChar == (char)13)
				{
					AddButton.PerformClick();
					e.Handled = true;
				}
			};

			AddButton = new Button()
			{
				Text = "Add Index",
				Bounds = new Rectangle(552, 112, 199, 30),
				FlatStyle = FlatStyle.Flat
			};
			AddButton.Click += (s, e) =>
			{
				string key = AddBox.Text.Trim();
				if (LanguageData.ExistRow(key))
				{
					MessageBox.Show("Keys duplicated:" + key);
					return;
				}
				int t = LanguageData.AddRow(key);
				LanguageData.ClearSelection();
				LanguageData.Rows[t].Selected = true;
				LanguageData.CurrentCell = LanguageData.Rows[t].Cells[0];

			};
			Controls.Add(AddButton);

			DeleteButton = new Button()
			{
				Text = "Delete Index",
				Bounds = new Rectangle(552, 142, 199, 30),
				FlatStyle = FlatStyle.Flat
			};
			DeleteButton.Click += (s, e) =>
			{
				if (LanguageData.SelectedRows.Count == 0) return;
				if (MessageBox.Show("将会删除选中行，是否继续？", "警告", MessageBoxButtons.OKCancel) != DialogResult.OK)
				{
					return;
				}
				int id = LanguageData.SelectedRows[0].Index;
				LanguageData.Rows.RemoveAt(id);
			};
			Controls.Add(DeleteButton);
		}

		private void InitCFG()
		{
			if (!File.Exists(CFG_FILE))
				File.WriteAllText(CFG_FILE, JsonConvert.SerializeObject(new Config(), Formatting.Indented));
			CFG = JsonConvert.DeserializeObject<Config>(File.ReadAllText(CFG_FILE));
		}
		private void Reset()
		{
			Data.Clear();
			LanguageData.Columns.Clear();
			LanguageData.Rows.Clear();
		}
		private void LoadData()
		{
			Reset();
			JObject obj = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(CFG.Path));
			foreach (var t in obj)
			{
				string lgg = t.Key;
				LanguageData.AddColumn(lgg);
				Data[lgg] = new Dictionary<string, string>();
				var rst = Data[lgg];
				foreach (var item in t.Value as JObject)
				{
					rst[item.Key] = item.Value.ToString();
				}
			}
			if (Data.Count == 0) return;
			//check
			List<Dictionary<string, string>> tmpData = Data.Values.ToList();
			Dictionary<string, string> std = tmpData[0];
			List<IEnumerable<string>> tmp = new List<IEnumerable<string>>();
			for (int i = 1; i < tmpData.Count; i++)
			{
				tmp.Add(tmpData[i].Keys.Except(std.Keys));
				tmp.Add(std.Keys.Except(tmpData[i].Keys));
			}
			if (!tmp.TrueForAll(t => t.Count() == 0))
			{
				throw new Exception("Keys not mached");
			}
			//
			foreach (var k in std.Keys)
			{
				int id = LanguageData.AddRow(k);
				foreach (var lg in Data)
				{
					LanguageData.Rows[id].Cells[lg.Key].Value = lg.Value[k];
				}
			}
			LanguageData.ClearSelection();
		}
		private void Save()
		{
			Data.Clear();
			foreach (DataGridViewColumn cl in LanguageData.Columns)
			{
				Data[cl.HeaderText] = new Dictionary<string, string>();
				foreach (DataGridViewRow row in LanguageData.Rows)
				{
					Data[cl.HeaderText][row.HeaderCell.Value as string] = row.Cells[cl.HeaderText].Value as string;
				}
			}
			JObject obj = new JObject();
			foreach (var lgg in Data)
			{
				obj[lgg.Key] = new JObject();
				var tmp = obj[lgg.Key];
				foreach (var item in lgg.Value)
				{
					tmp[item.Key] = item.Value as string;
				}
			}
			File.WriteAllText(CFG.Path, JsonConvert.SerializeObject(obj, Formatting.Indented));
			int cr = LanguageData.CurrentRow.Index;
			LoadData();
			LanguageData.Rows[cr].Cells[0].Selected = true;
		}
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			InitCFG();
			LoadData();
		}
		protected override void OnClosing(CancelEventArgs e)
		{
			base.OnClosing(e);
			if (MessageBox.Show("将会失去所有未保存的数据，是否继续？", "警告", MessageBoxButtons.YesNo) != DialogResult.Yes)
			{
				e.Cancel = true;
			}
		}
	}
}
