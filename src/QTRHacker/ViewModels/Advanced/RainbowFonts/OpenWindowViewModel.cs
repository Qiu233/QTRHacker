using QTRHacker.Commands;
using QTRHacker.Core.ProjectileImage;
using QTRHacker.Core.ProjectileImage.RainbowImage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml;

namespace QTRHacker.ViewModels.Advanced.RainbowFonts
{
	public class OpenWindowViewModel : WorkspaceViewModel
	{
		public record CharEntry(char Character, XmlElement Body);
		private string selectedFile;
		private CharEntry selectedCharacter;
		private ProjImage image;


		public ObservableCollection<string> Files
		{
			get;
		} = new();

		public ObservableCollection<CharEntry> Characters
		{
			get;
		} = new();

		public RelayCommand OpenCommand
		{
			get;
		}

		public string SelectedFile
		{
			get => selectedFile;
			set
			{
				selectedFile = value;
				OnPropertyChanged(nameof(SelectedFile));
			}
		}

		public CharEntry SelectedCharacter
		{
			get => selectedCharacter;
			set
			{
				selectedCharacter = value;
				OnPropertyChanged(nameof(SelectedCharacter));
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

		private void GetAllFiles(string path, List<string> files)
		{
			files.AddRange(Directory.EnumerateFiles(path));
			foreach (var dir in Directory.EnumerateDirectories(path))
				GetAllFiles(dir, files);
		}

		private void TryConfirm()
		{
			SetDialogResult(true);
			Close();
		}

		public OpenWindowViewModel()
		{
			PropertyChanged += OpenWindowViewModel_PropertyChanged;
			OpenCommand = new RelayCommand(o => SelectedCharacter is not null, o =>
			{
				TryConfirm();
			});
			List<string> files = new();
			GetAllFiles(HackGlobal.PATH_RAINBOWFONTS, files);
			files.ForEach(t => Files.Add(Path.GetRelativePath(HackGlobal.PATH_RAINBOWFONTS, t)));
		}

		private void OpenWindowViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(SelectedFile))
			{
				SelectedCharacter = null;
				Characters.Clear();
				if (SelectedFile == null)
					return;
				XmlDocument xmlFileContent = new();
				xmlFileContent.LoadXml(File.ReadAllText(Path.Combine(HackGlobal.PATH_RAINBOWFONTS, SelectedFile)));
				if (xmlFileContent["data"] is not XmlElement data)
					return;
				foreach (var group in data.ChildNodes.Cast<XmlElement>())
					Characters.Add(new(group["type"].InnerText[0], group["body"]));
			}
			else if (e.PropertyName == nameof(SelectedCharacter))
			{
				if (SelectedCharacter is null) return;
				try
				{
					Image = CharactersLoader.ParseBody(SelectedCharacter.Body);
				}
				catch (Exception ex)
				{
					HackGlobal.Logging.Error("Exception occured when trying to preview rainbow font in OpenWindow");
					HackGlobal.Logging.Exception(ex);
				}
				OpenCommand.TriggerCanExecuteChanged();
			}
		}
	}
}
