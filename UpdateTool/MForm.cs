using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UpdateTool
{
	public partial class MForm : Form
	{
		private TextBox UpdateLog;
		private MListBox Updates;
		private Button RefreshInfo, Install, InstallNewest;
		private Label Tip;
		private string CurVersion;
		private string[] Versions;
		public MForm()
		{
			InitializeComponent();
			UpdateLog = new TextBox();
			UpdateLog.Multiline = true;
			UpdateLog.ScrollBars = ScrollBars.Vertical;
			UpdateLog.Bounds = new Rectangle(0, 0, 280, 300);


			Updates = new MListBox();
			Updates.Bounds = new Rectangle(280, 0, 170, 300);
			Updates.MultiColumn = false;

			Tip = new Label();
			Tip.Bounds = new Rectangle(450, 20, 150, 80);
			Tip.TextAlign = ContentAlignment.MiddleCenter;

			RefreshInfo = new Button();
			RefreshInfo.Click += (s, e) =>
			{
				Fetch();
			};
			RefreshInfo.Text = "刷新";
			RefreshInfo.Bounds = new Rectangle(450, 140, 150, 40);

			Install = new Button();
			Install.Click += Install_Click;
			Install.Text = "安装";
			Install.Bounds = new Rectangle(450, 180, 150, 40);


			InstallNewest = new Button();
			InstallNewest.Click += InstallNewest_Click;
			InstallNewest.Text = "安装最新版";
			InstallNewest.Bounds = new Rectangle(450, 220, 150, 40);
			
			this.Controls.Add(UpdateLog);
			this.Controls.Add(Updates);
			this.Controls.Add(Tip);
			this.Controls.Add(RefreshInfo);
			this.Controls.Add(Install);
			this.Controls.Add(InstallNewest);

		}

		protected override void OnShown(EventArgs e)
		{
			try
			{
				Fetch();
			}
			catch
			{
				MessageBox.Show("获取更新失败");
				Environment.Exit(0);
			}
		}
		private void Fetch()
		{
			if (File.Exists("QTRHacker.exe"))
			{
				byte[] filedata = File.ReadAllBytes("QTRHacker.exe");
				Assembly ass = Assembly.Load(filedata);
				CurVersion = ass.GetName().Version.ToString();
			}
			else
				CurVersion = null;
			WebClient client = new WebClient();
			var data = client.DownloadData("https://raw.githubusercontent.com/ZQiu233/QTRHackerUpdatesHistory/master/Update.txt");
			UpdateLog.Text = Encoding.UTF8.GetString(data);
			UpdateLog.Select(UpdateLog.Text.Length, 0);
			UpdateLog.ScrollToCaret();

			data = client.DownloadData("https://raw.githubusercontent.com/ZQiu233/QTRHackerUpdatesHistory/master/UpdateList.txt");
			Versions = Encoding.UTF8.GetString(data).Split(new string[] { "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
			Updates.Items.Clear();
			foreach (var e in Versions)
			{
				string s = "";
				s += e;
				if (File.Exists("./Versions/" + e + ".zip"))
					s += "[已下载]";
				Updates.Items.Add(s);
			}
			Updates.SelectedIndex = CurVersion == null ? -1 : Versions.ToList().IndexOf(CurVersion);

			data = client.DownloadData("https://raw.githubusercontent.com/ZQiu233/QTRHackerUpdatesHistory/master/version.txt");
			var newest = Encoding.UTF8.GetString(data);
			string TipText = "";
			if (CurVersion != null)
			{
				if (Updates.SelectedIndex < 0)
					TipText += "当前是内部版本\n";
				else
					TipText += "当前是发布版本\n";
				TipText += "版本号:" + CurVersion + "\n";
			}
			else
				TipText += "当前无版本\n";
			TipText += "\n";
			TipText += "最新版:" + newest + "\n";
			Tip.Text = TipText;
		}

		private void Install_Click(object sender, EventArgs e)
		{
			if (Process.GetProcessesByName("QTRHacker").Count() > 0)
			{
				MessageBox.Show("请先关闭修改器进程再进行安装");
				return;
			}
			if (!(Updates.SelectedIndex >= 0 && Updates.SelectedIndex < Versions.Length))
			{
				MessageBox.Show("请选择要下载的版本");
				return;
			}
			string ver = Versions[Updates.SelectedIndex];
			InstallFile(ver);
			Fetch();
			MessageBox.Show("安装完成");
		}

		private void InstallNewest_Click(object sender, EventArgs e)
		{
			if (Process.GetProcessesByName("QTRHacker").Count() > 0)
			{
				MessageBox.Show("请先关闭修改器进程再进行安装");
				return;
			}
			string ver = Versions[Updates.Items.Count - 1];
			InstallFile(ver);
			Fetch();
			MessageBox.Show("安装完成");
		}
		private void InstallFile(string ver)
		{
			if (!Directory.Exists("./Versions"))
				Directory.CreateDirectory("./Versions");
			WebClient client = new WebClient();
			string zipFile = $"./Versions/{ver}.zip";
			if (!File.Exists(zipFile))
			{
				string uri = "https://raw.githubusercontent.com/ZQiu233/QTRHackerUpdatesHistory/master/Updates/" + ver + ".zip";
				byte[] data = client.DownloadData(uri);
				File.WriteAllBytes(zipFile, data);
			}
			var file = ZipFile.OpenRead(zipFile);
			foreach (var f in file.Entries)
			{
				if (f.Name == "VersionManager.exe")
					continue;
				var p = Path.GetDirectoryName(Path.Combine(".", f.FullName));
				if (!Directory.Exists(p))
					Directory.CreateDirectory(p);
				f.ExtractToFile(f.FullName, true);
			}
		}
	}
}
