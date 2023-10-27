using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.ViewModels.Wiki;

public partial class WikiViewModel : ObservableObject
{
	public WikiItemsPageViewModel WikiItemsPageViewModel
	{
		get;
	}
	public WikiViewModel(WikiItemsPageViewModel items)
	{
		WikiItemsPageViewModel = items;
	}
}
