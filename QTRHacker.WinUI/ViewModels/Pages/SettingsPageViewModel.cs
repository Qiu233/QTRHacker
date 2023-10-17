using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Globalization;
using Microsoft.UI.Xaml.Media.Animation;
using QTRHacker.Localization;
using QTRHacker.ViewModels.Settings;

namespace QTRHacker.ViewModels.Pages;

public partial class SettingsPageViewModel : PageViewModel
{
	public LanguageSelectionViewModel LanguageSelectionViewModel { get; }

	public SettingsPageViewModel(LanguageSelectionViewModel language)
	{
		LanguageSelectionViewModel = language;
	}
}
