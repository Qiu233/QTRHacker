using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using QTRHacker.Commands;
using QTRHacker.Core.ProjectileImage;
using QTRHacker.Core.ProjectileImage.RainbowImage;
using QTRHacker.Localization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml;
using System.Xml.Linq;

namespace QTRHacker.ViewModels.Advanced.RainbowFonts
{
	public class RainbowFontsMakerWindowViewModel : ViewModelBase, ILocalizationProvider
	{
		private string code;
		private IHighlightingDefinition syntaxHighlighting;
		private ProjImage image;
		private string fileName;
		private char character;
		private string titleRaw;

		public string Code
		{
			get => code;
			set
			{
				code = value;
				OnPropertyChanged(nameof(Code));
			}
		}

		public IHighlightingDefinition SyntaxHighlighting
		{
			get => syntaxHighlighting;
			set
			{
				syntaxHighlighting = value;
				OnPropertyChanged(nameof(SyntaxHighlighting));
			}
		}

		public ProjImage Image
		{
			get => image;
			set
			{
				image = value;
				OnPropertyChanged(nameof(Image));
			}
		}

		public ICommand PreviewCommand
		{
			get;
		}
		public ICommand NewCommand
		{
			get;
		}
		public ICommand OpenCommand
		{
			get;
		}
		public ICommand SaveCommand
		{
			get;
		}

		public string Title
		{
			get => titleRaw + (FileName != null ? $" - {FileName} - '{Character}'" : "");
		}

		public string FileName
		{
			get => fileName;
			set
			{
				fileName = value;
				OnPropertyChanged(nameof(FileName));
				OnPropertyChanged(nameof(Title));
			}
		}

		public char Character
		{
			get => character;
			set
			{
				character = value;
				OnPropertyChanged(nameof(Character));
				OnPropertyChanged(nameof(Title));
			}
		}

		public void OnCultureChanged(object sender, CultureChangedEventArgs args)
		{
			titleRaw = LocalizationManager.Instance.GetValue("UI.RainbowFonts.MakerTitle");
		}

		public void Preview()
		{
			try
			{
				XmlDocument xml = new();
				xml.LoadXml(Code);
				Image = CharactersLoader.ParseBody(xml["body"]);
			}
			catch (Exception e)
			{
				HackGlobal.Logging.Error("Exception occured when trying to preview rainbow font");
				HackGlobal.Logging.Exception(e);
			}
		}

		private static string Beautify(string doc) => XDocument.Parse(doc).ToString();

		private void NewFile()
		{
			FileName = null;
			Character = (char)0;
			using var s1 = Application.GetResourceStream(new Uri($"pack://application:,,,/Assets/Misc/RainbowFonts/Example.txt", UriKind.Absolute)).Stream;
			StreamReader sr = new(s1);
			Code = sr.ReadToEnd();
			Preview();
		}

		private void OpenFile()
		{
			Views.Advanced.RainbowFonts.OpenWindow window = new();
			window.DataContext = new OpenWindowViewModel();
			window.ViewModel.SelectedFile = FileName;
			if (window.ShowDialog() != true)
				return;
			FileName = window.ViewModel.SelectedFile;
			Character = window.ViewModel.SelectedCharacter.Character;
			Code = Beautify(window.ViewModel.SelectedCharacter.Body.OuterXml);
			Preview();
		}
		private void SaveFile()
		{
			if (FileName is null || Character == '\0')
			{
				Views.Advanced.RainbowFonts.SaveWindow window = new();
				window.DataContext = new SaveWindowViewModel();
				window.ViewModel.Code = Code;
				if (window.ShowDialog() != true)
					return;
				FileName = window.ViewModel.SelectedLib.Text;
				Character = window.ViewModel.CharText[0];
			}
			XmlDocument raw = new();
			string path = Path.Combine(HackGlobal.PATH_RAINBOWFONTS, FileName);
			try
			{
				raw.LoadXml(Code);
			}
			catch
			{
				MessageBox.Show(LocalizationManager.Instance.GetValue("UI.RainbowFonts.Messages.InvalidXMLCode"));
				return;
			}
			XmlDocument doc2 = new();
			try
			{
				doc2.LoadXml(File.ReadAllText(path));
			}
			catch
			{
				MessageBox.Show(LocalizationManager.Instance.GetValue("UI.RainbowFonts.Messages.FailedToWriteToFile"));
				return;
			}
			var dup = doc2["data"].ChildNodes.Cast<XmlElement>().Where(t =>
			{
				return t["type"].InnerText[0] == Character;
			});
			if (dup.Any())
			{
				var result = MessageBox.Show(LocalizationManager.Instance.GetValue("UI.RainbowFonts.Messages.OverrideCharacter"), "Override?", MessageBoxButton.YesNo);
				if (result == MessageBoxResult.Yes)
				{
					var el = dup.ElementAt(0);
					el.RemoveChild(el["body"]);
					el.AppendChild(doc2.ImportNode(raw["body"], true));
					using StreamWriter sw = new(File.Open(path, FileMode.Truncate), Encoding.UTF8);
					doc2.Save(sw);
					return;
				}
			}
			else//no duplicate
			{
				var ch = doc2.CreateElement("char");
				var type = doc2.CreateElement("type");
				type.InnerText = Character.ToString();
				ch.AppendChild(type);
				ch.AppendChild(doc2.ImportNode(raw["body"], true));
				doc2["data"].AppendChild(ch);
				using StreamWriter sw = new(File.Open(path, FileMode.Truncate), Encoding.UTF8);
				doc2.Save(sw);
			}
		}

		public RainbowFontsMakerWindowViewModel()
		{
			LocalizationManager.RegisterLocalizationProvider(this);
			PreviewCommand = new RelayCommand(o => true, o => { Preview(); });
			NewCommand = new RelayCommand(o => true, o =>
			{
				NewFile();
			});
			OpenCommand = new RelayCommand(o => true, o =>
			{
				OpenFile();
			});
			SaveCommand = new RelayCommand(o => true, o =>
			{
				SaveFile();
			});

			NewFile();
			using var s2 = Application.GetResourceStream(new Uri($"pack://application:,,,/XSHD/XML-Mode.xshd", UriKind.Absolute)).Stream;
			XmlReader xshd_reader = new XmlTextReader(s2);
			SyntaxHighlighting = HighlightingLoader.Load(xshd_reader, HighlightingManager.Instance);
		}
	}
}
