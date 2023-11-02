using QTRHacker.Assets;
using QTRHacker.ViewModels.Wiki;
using QTRHacker.Views.Wiki;
using StrongInject;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Containers;

[Register<WikiViewModel>(Scope.SingleInstance)]
[Register<WikiItemsPageViewModel>(Scope.SingleInstance)]
public partial class Wiki : IContainer<WikiViewModel>
{
	private readonly Owned<WikiViewModel> owned;
	public WikiWindow WikiWindow { get; private set; }
	public Wiki()
	{
		owned = this.Resolve();
		WikiWindow = new WikiWindow(owned.Value);
		WikiWindow.AppWindow.Destroying += AppWindow_Destroying;
	}

	private void AppWindow_Destroying(Microsoft.UI.Windowing.AppWindow sender, object args)
	{
		owned.Dispose();
		this.Dispose();
	}

	public async Task Show()
	{
		WikiWindow.Activate();
		while (WikiWindow.Content.XamlRoot is null)
			await Task.Delay(100);
	}
}
