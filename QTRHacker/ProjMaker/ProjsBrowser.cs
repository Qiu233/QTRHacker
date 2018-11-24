using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QTRHacker.ProjMaker
{
	public partial class ProjsBrowser : Form
	{
		private MListBox FilesBox;
		public ProjsBrowser()
		{
			InitializeComponent();
			FilesBox = new MListBox()
			{
				Bounds = new Rectangle(2, 2, 300, 200)
			};
			UpdateList();
			this.Controls.Add(FilesBox);

			Button Compile = new Button()
			{
				Text = "编译",
				Bounds = new Rectangle(303, 2, 95, 30),
				FlatStyle = FlatStyle.Flat
			};
			Compile.Click += (s, e) =>
			{
				if (FilesBox.SelectedIndices.Count <= 0) return;
				var ctx = MainForm.Context;
				if (ctx == null)
				{
					MessageBox.Show("请先锁定游戏");
					return;
				}
				ProjMaker.Parse.Parser p = new Parse.Parser(File.ReadAllText(($"./Projs/{(string)FilesBox.SelectedItem}.projimg")));
				try
				{
					var img = p.Parse();
					img.Emit(ctx, ctx.MyPlayer.X, ctx.MyPlayer.Y);
				}
				catch (Parse.ParseException ex)
				{
					MessageBox.Show("请检查代码\n错误：\n" + ex.Message, "出现编译错误");
				}
				catch (Exception ex)
				{
					MessageBox.Show("出现未知错误", "出现未知错误");
				}
			};
			this.Controls.Add(Compile);

			Button CreateNew = new Button()
			{
				Text = "新建",
				Bounds = new Rectangle(303, 32, 95, 30),
				FlatStyle = FlatStyle.Flat
			};
			CreateNew.Click += (s, e) =>
			{
				Form f = new Form();
				TextBox FileName = new TextBox()
				{
					Text = ""
				};
				Button et = new Button();

				f.Text = "新建";
				f.StartPosition = FormStartPosition.CenterParent;
				f.FormBorderStyle = FormBorderStyle.FixedSingle;
				f.MaximizeBox = false;
				f.MinimizeBox = false;
				f.Size = new Size(265, 60);

				Label tip1 = new Label()
				{
					Text = "名称:",
					Location = new Point(0, 5),
					Size = new Size(80, 20)
				};
				f.Controls.Add(tip1);

				FileName.Location = new Point(85, 0);
				FileName.Size = new Size(95, 20);
				f.Controls.Add(FileName);



				et.Text = Lang.confirm;
				et.Size = new Size(65, 20);
				et.Location = new Point(180, 0);
				et.Click += delegate (object sender1, EventArgs e1)
				{
					string str = $"./Projs/{FileName.Text}.projimg";
					if (!File.Exists(str))
						File.Create(str).Close();
					else
						MessageBox.Show("该名称的弹幕已存在");
					UpdateList();
					f.Dispose();
				};
				f.Controls.Add(et);
				f.StartPosition = FormStartPosition.CenterParent;
				f.ShowDialog(this);
			};
			this.Controls.Add(CreateNew);

			Button Edit = new Button()
			{
				Text = "编辑",
				Bounds = new Rectangle(303, 62, 95, 30),
				FlatStyle = FlatStyle.Flat
			};
			Edit.Click += (s, e) =>
			{
				if (FilesBox.SelectedIndices.Count <= 0) return;
				ProjMakerForm p = new ProjMakerForm((string)FilesBox.SelectedItem);
				p.ShowDialog(this);
			};
			this.Controls.Add(Edit);


			Button Delete = new Button()
			{
				Text = "删除",
				Bounds = new Rectangle(303, 92, 95, 30),
				FlatStyle = FlatStyle.Flat
			};
			Delete.Click += (s, e) =>
			{
				if (FilesBox.SelectedIndices.Count <= 0) return;
				if (MessageBox.Show("确定删除吗？", "警告", MessageBoxButtons.OKCancel) == DialogResult.Cancel) return;
				File.Delete(($"./Projs/{(string)FilesBox.SelectedItem}.projimg"));
				UpdateList();
			};
			this.Controls.Add(Delete);
		}

		public void UpdateList()
		{
			FilesBox.Items.Clear();
			foreach (var f in Directory.EnumerateFiles("./Projs/"))
			{
				FilesBox.Items.Add(Path.GetFileNameWithoutExtension(f));
			}
		}
	}
}
