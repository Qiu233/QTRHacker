﻿using CommunityToolkit.Mvvm.ComponentModel;
using QTRHacker.Assets;
using QTRHacker.Core.GameObjects.Terraria;
using QTRHacker.Models;
using QTRHacker.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.ViewModels.PlayerEditor.SlotsPages;

public abstract class SlotsPageViewModel : ObservableObject
{
	public abstract string Header { get; }
	public SlotsPageViewModel()
	{
	}

	public abstract Task Update();

	public abstract Task<Item> GetItem(int id);

	public abstract IEnumerable<ItemSlotViewModel> Slots { get; }

	protected static async void UpdateItemStack(ItemSlotViewModel vm, int type, int stack)
	{
		await vm.SetItem(type, stack);
	}
}