﻿using QTRHacker.Views.Advanced.AimBot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.ViewModels.Advanced.AimBot
{
	public class AimBot : AdvancedFunction
	{
		private static AimBotWindow AimBotWindow = null;
		public override void ApplyLocalization(string culture)
		{
			Name = culture switch
			{
				"zh" => "自瞄",
				_ => "Aimbot",
			};
		}

		public override void Run()
		{
			ShowWindow();
		}

		private static void ShowWindow()
		{
			if (AimBotWindow == null || !AimBotWindow.IsLoaded)
			{
				AimBotWindow = new();
				AimBotWindow.DataContext = new AimBotWindowViewModel();
			}
			AimBotWindow.Show();
		}
	}
}
