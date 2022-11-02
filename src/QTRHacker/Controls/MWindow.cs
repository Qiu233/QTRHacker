using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QTRHacker.Controls;

public class MWindow : Window
{
	public static readonly DependencyProperty MinimizeBoxProperty =
		DependencyProperty.Register(nameof(MinimizeBox), typeof(bool), typeof(MWindow), new PropertyMetadata(false));

	private ContentControl PART_Content;

	public bool MinimizeBox
	{
		get => (bool)GetValue(MinimizeBoxProperty);
		set => SetValue(MinimizeBoxProperty, value);
	}

	static MWindow()
	{
		DefaultStyleKeyProperty.OverrideMetadata(typeof(MWindow), new FrameworkPropertyMetadata(typeof(MWindow)));
	}

	public MWindow()
	{
		CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand,
			(s, e) => SystemCommands.MinimizeWindow(this)));
		CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand,
			(s, e) => SystemCommands.CloseWindow(this)));
	}

	public override void OnApplyTemplate()
	{
		base.OnApplyTemplate();
		TextBlock title = Template.FindName("PART_TitleText", this) as TextBlock;
		title.MouseDown += TitleMouseDown;
		PART_Content = GetTemplateChild(nameof(PART_Content)) as ContentControl;
	}

	private void TitleMouseDown(object sender, MouseButtonEventArgs e)
	{
		if (e.LeftButton == MouseButtonState.Pressed)
			DragMove();
	}
}
