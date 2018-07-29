/*
 * Created by SharpDevelop.
 * User: jianqiu
 * Date: 2015/9/12
 * Time: 15:22
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
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		public static int HEIGHT=470;
		
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
			if(Extra!=null)
				Extra.Dispose();
			//HackFunctions.stop=true;
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// MainForm
			// 
			this.Icon=(Icon)resource.res.GetObject("Icon");
			this.StartPosition = FormStartPosition.CenterScreen;
			this.ClientSize=new Size(300,MainForm.HEIGHT);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Text = Version;
			this.Name = "MainForm";
			this.MaximizeBox=false;
			this.MinimizeBox=false;
			this.FormBorderStyle=FormBorderStyle.FixedSingle;
		}
	}
}
