/*
 * Created by SharpDevelop.
 * User: Qiu233
 * Date: 2016/7/28
 * Time: 9:27
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
	partial class ExtraForm
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
			if(specialForm!=null)
				specialForm.Dispose();
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
			// ExtraForm
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.MaximizeBox=false;
			this.MinimizeBox=false;
			this.Text = Lang.extra;
			this.Name = "ExtraForm";
			this.ClientSize=new Size(260,MainForm.HEIGHT);
			this.FormBorderStyle=FormBorderStyle.FixedSingle;
			this.ControlBox=false;
		}
	}
}
