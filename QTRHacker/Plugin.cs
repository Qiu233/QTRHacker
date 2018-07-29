using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Terraria_Hacker
{
	public abstract class Plugin
	{
		public virtual void Loaded()
		{

		}
		public Plugin()
		{

		}

		protected Button RegisterButton(string text, string tab, MainForm.HackFunc enable, MainForm.HackFunc cancel, bool closable = true, int rank = -1)//To create buttons
		{
			if (rank == -1)
			{
				return MainForm.mainWindow.AddButton(MainForm.mainWindow.tabs[tab], text, MainForm.mainWindow.indexes[tab]++, enable, cancel, closable);
			}
			else
			{
				var b = MainForm.mainWindow.AddButton(MainForm.mainWindow.tabs[tab], text, rank, enable, cancel, closable);
				if (rank + 1 > MainForm.mainWindow.indexes[tab])
					MainForm.mainWindow.indexes[tab] = rank + 1;
				return b;
			}
		}
		protected void RegisterTab(string Text)//To add a new tab
		{
			MainForm.mainWindow.RegisterTab(Text);
		}
		protected int GetTabCurrentIndex(string Text)
		{
			return MainForm.mainWindow.indexes[Text];
		}
	}
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public class PluginInfo : Attribute
	{
		public string Name { get; }
		public string Version { get; }
		public PluginInfo(string name, string version)
		{
			Name = name;
			Version = version;
		}
	}
}
