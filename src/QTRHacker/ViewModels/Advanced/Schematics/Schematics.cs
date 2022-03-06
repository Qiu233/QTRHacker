using QTRHacker.Views.Advanced.Schematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.ViewModels.Advanced.Schematics
{
	public class Schematics : AdvancedFunction
	{
		private static ScheWindow ScheWindow = null;
		public override void ApplyLocalization(string culture)
		{
			Name = culture switch
			{
				"zh" => "建筑",
				_ => "Schematics",
			};
		}

		public override void Run()
		{
			ShowWindow();
		}

		private static void ShowWindow()
		{
			if (ScheWindow == null || !ScheWindow.IsLoaded)
			{
				ScheWindow = new();
				ScheWindow.DataContext = new ScheWindowViewModel();
			}
			ScheWindow.Show();
		}
	}
}
