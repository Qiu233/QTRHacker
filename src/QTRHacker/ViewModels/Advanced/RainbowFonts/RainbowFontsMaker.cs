using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.ViewModels.Advanced.RainbowFonts
{
	public class RainbowFontsMaker : AdvancedFunction
	{
		private static Views.Advanced.RainbowFonts.RainbowFontsMakerWindow RainbowFontsMakerWindow = null;
		public override void ApplyLocalization(string culture)
		{
			Name = culture switch
			{
				"zh" => "彩虹字设计器",
				_ => "Rainbow Fonts Maker",
			};
		}

		public override void Run()
		{
			if (RainbowFontsMakerWindow == null || !RainbowFontsMakerWindow.IsLoaded)
			{
				RainbowFontsMakerWindow = new();
				RainbowFontsMakerWindow.DataContext = new RainbowFontsMakerWindowViewModel();
			}
			RainbowFontsMakerWindow.Show();
		}
	}
}
