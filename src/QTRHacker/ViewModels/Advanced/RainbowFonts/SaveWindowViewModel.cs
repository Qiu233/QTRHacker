using QTRHacker.Commands;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Xml;

namespace QTRHacker.ViewModels.Advanced.RainbowFonts;

public class SaveWindowViewModel : WorkspaceViewModel
{
	private FontLib selectedLib;
	private string charText;
	private string code;

	public ObservableCollection<FontLib> FontsLibs
	{
		get;
	} = new();

	public FontLib SelectedLib
	{
		get => selectedLib;
		set
		{
			selectedLib = value;
			OnPropertyChanged(nameof(SelectedLib));
		}
	}

	public string CharText
	{
		get => charText;
		set
		{
			charText = value;
			OnPropertyChanged(nameof(CharText));
		}
	}

	public string Code
	{
		get => code;
		set
		{
			code = value;
			OnPropertyChanged(nameof(Code));
		}
	}

	public RelayCommand NewCommand
	{
		get;
	}
	public RelayCommand SaveCommand
	{
		get;
	}

	private void GetAllFiles(string path, List<string> files)
	{
		files.AddRange(Directory.EnumerateFiles(path));
		foreach (var dir in Directory.EnumerateDirectories(path))
			GetAllFiles(dir, files);
	}

	public void ApplyNew(NewLibViewModel model)
	{
		if (!FontsLibs.Contains(model))
			return;
		if (model.Text == null || model.Text == "")
		{
			FontsLibs.Remove(model);
			return;
		}
		string path;
		try
		{
			path = Path.Combine(HackGlobal.PATH_RAINBOWFONTS, model.Text);
		}
		catch (Exception e)
		{
			MessageBox.Show(Localization.LocalizationManager.Instance.GetValue("UI.RainbowFonts.Messages.FailedToNewFile"));
			FontsLibs.Remove(model);
			HackGlobal.Logging.Error("Failed to create new file");
			HackGlobal.Logging.Exception(e);
			return;
		}
		if (File.Exists(path))
		{
			FontsLibs.Remove(model);
			return;
		}
		XmlDocument doc = new();
		var dec = doc.CreateXmlDeclaration("1.0", "utf-8", null);
		var empdata = doc.CreateElement("data");
		doc.AppendChild(dec);
		doc.AppendChild(empdata);
		using StreamWriter sw = new(File.Open(path, FileMode.Create), Encoding.UTF8);
		doc.Save(sw);
		bool selected = SelectedLib == model;
		FontsLibs.Remove(model);
		var lib = new ExistingLibViewModel() { Text = model.Text };
		FontsLibs.Add(lib);
		if (selected)
			SelectedLib = lib;
	}

	private static bool IsValidChar(char c)
	{
		return !char.IsWhiteSpace(c) && !char.IsControl(c);
	}

	public SaveWindowViewModel()
	{
		PropertyChanged += SaveWindowViewModel_PropertyChanged;
		NewCommand = new RelayCommand(o => true, o =>
		{
			var lib = new NewLibViewModel() { };
			FontsLibs.Add(lib);
			SelectedLib = lib;
		});
		SaveCommand = new RelayCommand(o =>
		{
			return SelectedLib is ExistingLibViewModel && CharText != null && CharText.Length == 1 && IsValidChar(CharText[0]);
		}, o =>
		{
			SetDialogResult(true);
			Close();
		});

		List<string> files = new();
		GetAllFiles(HackGlobal.PATH_RAINBOWFONTS, files);
		files.ForEach(t => FontsLibs.Add(new ExistingLibViewModel() { Text = Path.GetRelativePath(HackGlobal.PATH_RAINBOWFONTS, t) }));
	}

	private void SaveWindowViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
	{
		if (e.PropertyName == nameof(SelectedLib))
		{
			SaveCommand.TriggerCanExecuteChanged();
		}
		else if (e.PropertyName == nameof(CharText))
		{
			SaveCommand.TriggerCanExecuteChanged();
		}
	}

	public abstract class FontLib : ViewModelBase
	{
		private string text;

		public string Text
		{
			get => text;
			set
			{
				text = value;
				OnPropertyChanged(nameof(Text));
			}
		}
	}

	public class NewLibViewModel : FontLib
	{
	}
	public class ExistingLibViewModel : FontLib
	{
	}
}
