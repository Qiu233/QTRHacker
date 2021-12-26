using QTRHacker.Res;
using System.Drawing;
using System.IO;

namespace QTRHacker.Wiki
{
	partial class WikiForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.MaximizeBox = false;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.ClientSize = new System.Drawing.Size(740, 480);
			this.Text = "Wiki";
			using (Stream st = new MemoryStream(GameResLoader.ItemImageData["Item_3628"]))
				this.Icon = MainForm.ConvertToIcon(Image.FromStream(st));
		}

		#endregion
	}
}