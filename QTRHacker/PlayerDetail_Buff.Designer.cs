/*
 * 由SharpDevelop创建。
 * 用户： lopi2
 * 日期: 2017/1/24
 * 时间: 17:22
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
 
 using System;
 using System.Drawing;
 using System.Windows.Forms;
namespace Terraria_Hacker
{
	partial class PlayerDetail_Buff
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
			// PlayerDetail_Buff
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.MaximizeBox=false;
			this.MinimizeBox=false;
			
			this.Size=new Size(700,110);
			this.FormBorderStyle=FormBorderStyle.FixedSingle;
			this.Name = "PlayerDetail_Buff";
		}
	}
}
