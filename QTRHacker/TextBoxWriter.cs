/*
 * 由SharpDevelop创建。
 * 用户： lopi2
 * 日期: 2017/5/1
 * 时间: 13:54
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Text;

namespace Terraria_Hacker
{
	/// <summary>
	/// Description of TextBoxWriter.
	/// </summary>
	public class TextBoxWriter:TextWriter
	{
		private TextBox tb;
		public TextBoxWriter(TextBox tb)
		{
			this.tb=tb;
		}
		public override void Write(string v)
		{
			tb.AppendText(v);
		}
		public override void WriteLine(string v)
		{
			tb.AppendText(v);
			tb.AppendText(this.NewLine);
		}
		public override Encoding Encoding
		{
			get { return Encoding.Unicode; }
		}
	}
}
