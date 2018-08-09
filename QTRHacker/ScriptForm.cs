using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using QTRHacker.Functions;
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

namespace QTRHacker
{
	public partial class ScriptForm : Form
	{
		public class Script
		{
			public static GameContext GetContext()
			{
				return MainForm.Context;
			}
			public static void Write(string str)
			{
				ScriptForm.Script.o.Write(str);
			}

			public static void WriteLine(string str)
			{
				ScriptForm.Script.o.WriteLine(str);
			}


			public static void Load(string name)
			{
				/*StreamReader streamReader = new StreamReader("./scripts/" + name + ".js");
				string text = streamReader.ReadToEnd();
				streamReader.Close();
				ScriptForm.Script.WriteLine(ScriptForm.ve.Execute(text, "V8.NET", false) + "\n");*/
			}

			// Token: 0x04000143 RID: 323
			public static TextWriter o;
		}

		public GameContext Context;
		private ScriptEngine Engine;
		private ScriptScope DefaultScope;

		public ScriptForm(GameContext Context)
		{
			this.Context = Context;
			InitializeComponent();

			ScriptRuntime runtime = Python.CreateRuntime();
			Engine = runtime.GetEngine("Python");
			DefaultScope = Engine.CreateScope();
			var paths = Engine.GetSearchPaths();
			paths.Add(Environment.CurrentDirectory);
			Engine.SetSearchPaths(paths);


			TextBox textBox = new TextBox();
			textBox.TabIndex = 1;
			textBox.Multiline = true;
			textBox.WordWrap = true;
			textBox.ScrollBars = ScrollBars.Vertical;
			textBox.Location = new Point(0, 0);
			textBox.Size = new Size(base.Width - 10, 410);
			textBox.KeyPress += delegate (object sender, KeyPressEventArgs e)
			{
				e.Handled = true;
			};
			base.Controls.Add(textBox);

			ScriptForm.Script.o = new TextBoxWriter(textBox);

			TextBox cmd = new TextBox();
			cmd.TabIndex = 0;
			cmd.Font = new Font("Arial", 10f);
			cmd.Location = new Point(0, 415);
			cmd.Size = new Size(base.Width - 20, 50);
			cmd.KeyDown += delegate (object sender, KeyEventArgs e)
			{
				if (e.KeyCode == Keys.Return)
				{
					e.Handled = true;
					string text = cmd.Text;
					cmd.Clear();
					var a = Engine.Execute(text, DefaultScope);
					if (a != null)
						ScriptForm.Script.WriteLine(a.ToString());
				}
			};
			base.Controls.Add(cmd);
			this.AllowDrop = true;

			base.DragEnter += delegate (object sender, DragEventArgs e)
			{
				if (e.Data.GetDataPresent(DataFormats.FileDrop))
				{
					e.Effect = DragDropEffects.Copy;
					return;
				}
				e.Effect = DragDropEffects.None;
			};
			base.DragDrop += delegate (object sender, DragEventArgs e)
			{
				string[] array = e.Data.GetData(DataFormats.FileDrop, false) as string[];
				foreach (var s in array)
				{
					Script.WriteLine("执行:" + s);
					try
					{
						Engine.Execute(File.ReadAllText(s), Engine.CreateScope());
					}
					catch(Exception err)
					{
						MessageBox.Show(err.Message);
					}
				}
			};
			cmd.Focus();
		}
	}
}
