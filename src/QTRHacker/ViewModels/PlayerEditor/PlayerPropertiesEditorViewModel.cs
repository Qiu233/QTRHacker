using QTRHacker.Commands;
using QTRHacker.Core.GameObjects.Terraria;
using QTRHacker.Localization;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Media;

namespace QTRHacker.ViewModels.PlayerEditor;

public class PlayerPropertiesEditorViewModel : ViewModelBase
{
	private bool updating;
	private int maxLife;
	private int maxMana;
	private readonly Player player;
	private readonly HackCommand refreshCommand;
	private readonly HackCommand applyCommand;

	public HackCommand RefreshCommand => refreshCommand;
	public HackCommand ApplyCommand => applyCommand;

	public bool Updating
	{
		get => updating;
		set
		{
			updating = value;
			OnPropertyChanged(nameof(Updating));
		}
	}
	public Player Player
	{
		get => player;
	}
	public ObservableCollection<ColorViewModel> Colors { get; } = new();
	public int MaxLife
	{
		get => maxLife;
		set
		{
			if (value < 0)
				return;
			maxLife = value;
			OnPropertyChanged(nameof(MaxLife));
		}
	}
	public int MaxMana
	{
		get => maxMana;
		set
		{
			if (value < 0)
				return;
			maxMana = value;
			OnPropertyChanged(nameof(MaxMana));
		}
	}

	private void InitColors()
	{
		Colors.Add(new ColorViewModel("Hair", "HairColor"));
		Colors.Add(new ColorViewModel("Skin", "SkinColor"));
		Colors.Add(new ColorViewModel("Eyes", "EyeColor"));
		Colors.Add(new ColorViewModel("Shirt", "ShirtColor"));
		Colors.Add(new ColorViewModel("Undershirt", "UnderShirtColor"));
		Colors.Add(new ColorViewModel("Pants", "PantsColor"));
		Colors.Add(new ColorViewModel("Shoes", "ShoeColor"));
	}

	public PlayerPropertiesEditorViewModel(Player player)
	{
		this.player = player;
		InitColors();
		refreshCommand = new HackCommand(o => Update());
		applyCommand = new HackCommand(o => ApplyToGame());

		Update();
	}
	public void ApplyToGame()
	{
		foreach (var colorVModel in Colors)
		{
			var property = typeof(Player).GetProperty(colorVModel.PropertyName);
			var color = new Core.GameObjects.ValueTypeRedefs.Xna.Color
			{
				R = colorVModel.Color.R,
				G = colorVModel.Color.G,
				B = colorVModel.Color.B,
				A = colorVModel.Color.A
			};
			property.SetValue(Player, color);
		}
		Player.StatLifeMax = MaxLife;
		Player.StatManaMax = MaxMana;
	}
	public void Update()
	{
		foreach (var colorVModel in Colors)
		{
			var color = (Core.GameObjects.ValueTypeRedefs.Xna.Color)typeof(Player).GetProperty(colorVModel.PropertyName).GetValue(Player);
			colorVModel.ColorValue = color.R << 16 | color.G << 8 | color.B | unchecked((int)0xFF_000000);
		}
		MaxLife = Player.StatLifeMax;
		MaxMana = Player.StatManaMax;
	}
}
public class ColorViewModel : ViewModelBase, ILocalizationProvider
{
	private readonly string tipKey;
	private string propertyName;
	private int colorValue;
	public string Tip => LocalizationManager.Instance.GetValue($"UI.PlayerColors.{tipKey}");
	public string PropertyName
	{
		get => propertyName;
		set
		{
			propertyName = value;
			OnPropertyChanged(nameof(PropertyName));
		}
	}
	public string ColorInput
	{
		get => (ColorValue & 0xFFFFFF).ToString("X6");
		set
		{
			if (int.TryParse(value, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out int s)
				&& s >= 0
				&& s <= 0xFFFFFF)
				ColorValue = s | unchecked((int)0xFF_000000);
		}
	}
	public int ColorValue
	{
		get => colorValue;
		set
		{
			colorValue = value;
			OnPropertyChanged(nameof(ColorValue));
			OnPropertyChanged(nameof(ColorInput));
			OnPropertyChanged(nameof(Color));
		}
	}
	public Color Color
	{
		get
		{
			byte r = (byte)(ColorValue >> 16);
			byte g = (byte)(ColorValue >> 8);
			byte b = (byte)ColorValue;
			return Color.FromRgb(r, g, b);
		}
	}

	public ColorViewModel(string tip, string propertyName)
	{
		tipKey = tip;
		PropertyName = propertyName;
		LocalizationManager.RegisterLocalizationProvider(this);

	}

	public void OnCultureChanged(object sender, CultureChangedEventArgs args)
	{
		OnPropertyChanged(nameof(Tip));
	}
}
