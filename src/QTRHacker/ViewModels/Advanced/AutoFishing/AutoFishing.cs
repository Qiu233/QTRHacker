using QTRHacker.Views.Advanced.AutoFishing;

namespace QTRHacker.ViewModels.Advanced.AutoFishing;

public class AutoFishing : AdvancedFunction
{
	private static AutoFishingWindow AutoFishingWindow = null;
	public override void ApplyLocalization(string culture)
	{
		Name = culture switch
		{
			"zh" => "自动钓鱼",
			_ => "Fishing Bot",
		};
	}

	public override void Run()
	{
		ShowWindow();
	}
	private static void ShowWindow()
	{
		if (AutoFishingWindow == null || !AutoFishingWindow.IsLoaded)
		{
			AutoFishingWindow = new();
			AutoFishingWindow.DataContext = new AutoFishingWindowViewModel();
		}
		AutoFishingWindow.Show();
	}
}