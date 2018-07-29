/*
 * Created by SharpDevelop.
 * User: Qiu233
 * Date: 2016/7/28
 * Time: 17:26
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Text;
namespace Terraria_Hacker
{
	partial class SpecialForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			if(HasChildren)
			{
				foreach(var o in MdiChildren)
				{
					o.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.SuspendLayout();
			// 
			// SpecialForm
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ControlBox=false;
			this.MaximizeBox=false;
			this.MinimizeBox=false;
			this.Name = "SpecialForm";
			this.Text = Lang.more;
			this.ClientSize=new Size(300,MainForm.HEIGHT);
			this.FormBorderStyle=FormBorderStyle.FixedSingle;
		}
	}
}
