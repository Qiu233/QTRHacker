using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using WinUIEx;
using Windows.Graphics;
using QTRHacker.ViewModels.PlayerEditor;
using StrongInject;
using QTRHacker.ViewModels.PlayerEditor.SlotsPages;
using QTRHacker.Containers.PlayerEditor;

namespace QTRHacker.Views.PlayerEditor;

//
public sealed partial class InventoryEditorWindow : WindowEx
{
	private InventoryEditorViewModel ViewModel { get; }
	public InventoryEditorWindow(InventoryEditorViewModel vm)
	{
		this.InitializeComponent();
		this.ExtendsContentIntoTitleBar = true;
		this.AppWindow.TitleBar.IconShowOptions = Microsoft.UI.Windowing.IconShowOptions.HideIconAndSystemMenu;
		this.SetTitleBar(TitleBar);

		this.ViewModel = vm;
	}
}
