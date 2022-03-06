using Microsoft.Xna.Framework.Graphics;
using QTRHacker.Controls;
using QTRHacker.EventManagers;
using QTRHacker.ViewModels.Advanced.Schematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace QTRHacker.Views.Advanced.Schematics
{
	/// <summary>
	/// ScheWindow.xaml 的交互逻辑
	/// </summary>
	public partial class ScheWindow : MWindow
	{
		public ScheWindowViewModel ViewModel => DataContext as ScheWindowViewModel;
		public ScheWindow()
		{
			InitializeComponent();
		}
	}
}
