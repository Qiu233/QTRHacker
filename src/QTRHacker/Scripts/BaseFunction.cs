using QTRHacker.Functions;
using QTRHacker.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QTRHacker.Scripts
{
	public abstract class BaseFunction : ViewModels.ViewModelBase, ILocalizationProvider
	{
		private string name;
		private string tooltip;
		private bool isEnabled;
		private double progress;
		private bool isProgressing;

		public string Name
		{
			get => name;
			set
			{
				name = value;
				OnPropertyChanged(nameof(Name));
			}
		}
		public string Tooltip
		{
			get => tooltip;
			set
			{
				tooltip = value;
				OnPropertyChanged(nameof(Tooltip));
			}
		}
		/// <summary>
		/// Note: this property would/ can be changed asynchronously
		/// </summary>
		public bool IsEnabled
		{
			get => isEnabled;
			set
			{
				isEnabled = value;
				Application.Current.Dispatcher.Invoke(()
					=> OnPropertyChanged(nameof(IsEnabled)));
			}
		}
		/// <summary>
		/// Maximum is 100.0
		/// Note: this property would/ can be changed asynchronously
		/// </summary>
		public double Progress
		{
			get => progress;
			set
			{
				progress = value;
				Application.Current.Dispatcher.Invoke(()
					=> OnPropertyChanged(nameof(Progress)));
			}
		}
		public bool IsProgressing
		{
			get => isProgressing;
			set
			{
				isProgressing = value;
				OnPropertyChanged(nameof(IsProgressing));
			}
		}


		public virtual bool HasProgress => false;
		public abstract bool CanDisable { get; }

		public abstract void Enable(GameContext context);
		public abstract void Disable(GameContext context);

		public virtual void OnLoaded() { }

		public abstract void ApplyLocalization(string culture);

		public void OnCultureChanged(object sender, CultureChangedEventArgs args)
		{
			ApplyLocalization(args.Name);
		}

		protected BaseFunction()
		{
			LocalizationManager.RegisterLocalizationProvider(this);
		}
	}
}
