using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Diagnostics;

namespace QTRHacker.Controls
{
	public partial class FunctionButton : UserControl
	{
		public class OnFunctionEnabledEventArgs : EventArgs
		{
			public bool Enabled = true;
		}
		public class OnFunctionDisabledEventArgs : EventArgs
		{
			public bool Disabled = true;
		}
		public new string Text { get; set; }
		public bool FunctionEnabled
		{
			get => _functionEnabled;
			set
			{
				_functionEnabled = value;
				if (!Closable)
					_functionEnabled = false;
				Invalidate();
			}
		}
		public static Color NormalColor = Color.FromArgb(20, 255, 255, 255);
		public static Color DefualtForeColor = Color.FromArgb(180, 180, 180);
		public static Color HoverColor = Color.FromArgb(40, 255, 255, 255);
		public static Color MouseDownColor = Color.FromArgb(90, 255, 255, 255);
		public static Color SelectedColor_Closable = Color.FromArgb(100, 0, 255, 0);
		public static Color SelectedColor = Color.FromArgb(100, 0, 0, 255);
		private bool _functionEnabled;
		public bool Closable { get; set; }
		public int Identity
		{
			get;
		}
		public event Action<object, OnFunctionEnabledEventArgs> OnEnable = (s, e) => { };
		public event Action<object, OnFunctionDisabledEventArgs> OnDisable = (s, e) => { };
		private readonly Func<object, bool> GetEnabled;
		public FunctionButton(int Identity, Func<object, bool> GetEnabled, bool Closable = true)
		{
			SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
			UpdateStyles();
			this.Identity = Identity;
			this.Closable = Closable;
			this.GetEnabled = GetEnabled;
			ForeColor = DefualtForeColor;
			BackColor = NormalColor;
			AutoScaleDimensions = new SizeF(6F, 12F);
			AutoScaleMode = AutoScaleMode.Font;
			Name = "FunctionButton";
			Size = new Size(230, 20);
		}
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			UpdateFunctionEnabled();
		}
		public void UpdateFunctionEnabled()
		{
			FunctionEnabled = GetEnabled(this);
		}
		protected override void OnClick(EventArgs e)
		{
			base.OnClick(e);
		}
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (e.Button == MouseButtons.Left)
			{
				BackColor = MouseDownColor;
				if (!Closable)
				{
					OnEnable(this, new OnFunctionEnabledEventArgs());
					return;
				}
				bool enabled = false;
				if (!FunctionEnabled)
				{
					var args = new OnFunctionEnabledEventArgs();
					OnEnable(this, args);
					enabled = args.Enabled;
				}
				else
				{
					var args = new OnFunctionDisabledEventArgs();
					OnDisable(this, args);
					enabled = !args.Disabled;
				}
				FunctionEnabled = enabled;
			}
		}
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			BackColor = HoverColor;
		}
		protected override void OnMouseEnter(EventArgs e)
		{
			base.OnMouseEnter(e);
			BackColor = HoverColor;
		}
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			BackColor = NormalColor;
		}
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			Graphics g = e.Graphics;
			g.SmoothingMode = SmoothingMode.AntiAlias;
			g.InterpolationMode = InterpolationMode.HighQualityBicubic;
			g.CompositingQuality = CompositingQuality.HighQuality;
			if (Closable)
				g.DrawString(Text, SystemFonts.DialogFont, Brushes.White, 22, 3);
			else
				g.DrawString(Text, SystemFonts.DialogFont, Brushes.White, 4, 3);
			if (Closable)
				g.DrawEllipse(Pens.White, 5, 3, 12, 12);
			if (FunctionEnabled && Closable)
				using (Brush b = new SolidBrush(SelectedColor_Closable))
					g.FillEllipse(b, 7, 5, 8, 8);
		}
	}
}
