using System.Windows;

namespace QTRHacker.Themes;

public static class ThemeManager
{
	private const string ThemePath = "/QTRHacker;component/Styles/Themes/";

	public static string CurrentTheme { get; private set; } = "Dark";

	public static void Apply(string theme)
	{
		string name = theme == "Light" ? "Light" : "Dark";
		var dictionaries = Application.Current.Resources.MergedDictionaries;
		var replacement = new ResourceDictionary
		{
			Source = new Uri($"{ThemePath}{name}.xaml", UriKind.Relative)
		};

		for (int index = 0; index < dictionaries.Count; index++)
		{
			if (dictionaries[index].Source?.OriginalString.StartsWith(ThemePath, StringComparison.Ordinal) == true)
			{
				dictionaries[index] = replacement;
				CurrentTheme = name;
				return;
			}
		}

		dictionaries.Insert(0, replacement);
		CurrentTheme = name;
	}
}
