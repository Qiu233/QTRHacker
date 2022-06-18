using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace QTRHacker.ShaderEffects
{
	public class XNATintEffects : ShaderEffect
	{
		static XNATintEffects()
		{
			_pixelShader.UriSource = new Uri("pack://application:,,,/QTRHacker;component/ShaderEffects/Shaders/XNATintShader.ps");
		}

		private static readonly PixelShader _pixelShader = new();

		public XNATintEffects()
		{
			PixelShader = _pixelShader;
			UpdateShaderValue(InputProperty);
			UpdateShaderValue(ColorProperty);
		}

		public Brush Input
		{
			get => (Brush)GetValue(InputProperty);
			set => SetValue(InputProperty, value);
		}

		public static readonly DependencyProperty InputProperty =
			RegisterPixelShaderSamplerProperty(nameof(Input), typeof(XNATintEffects), 0);

		public Color Color
		{
			get => (Color)GetValue(ColorProperty);
			set => SetValue(ColorProperty, value);
		}

		public static readonly DependencyProperty ColorProperty =
			DependencyProperty.Register(nameof(Color), typeof(Color), typeof(XNATintEffects),
			  new UIPropertyMetadata(Colors.White, PixelShaderConstantCallback(0)));
	}
}
