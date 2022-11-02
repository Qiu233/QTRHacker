using System;
using System.Collections.Generic;

namespace QTRHacker.Core.ProjectileImage.RainbowImage;

/// <summary>
/// 约定：每个英文字母占位最多150*200
/// </summary>
public class RainbowTextDrawer : IEmmitable
{
	public const int LetterWidth = 150;
	public const int LetterHeight = 200;
	public RainbowDrawer SourceDrawer
	{
		get;
	}
	public Dictionary<char, ProjImage> DefinedCharacters
	{
		get;
	}
	/// <summary>
	/// 
	/// </summary>
	/// <param name="Characters">null for empty</param>
	public RainbowTextDrawer(Dictionary<char, ProjImage> Characters) : this(new RainbowDrawer(), Characters)
	{
	}

	public RainbowTextDrawer(RainbowDrawer sourceDrawer, Dictionary<char, ProjImage> Characters)
	{
		SourceDrawer = sourceDrawer ?? throw new ArgumentNullException(nameof(sourceDrawer));
		DefinedCharacters = Characters ?? new Dictionary<char, ProjImage>();
	}

	public void DrawBorder(MPointF center)
	{
		var left_up = center + new MPointF(-100, -100);
		var right_up = center + new MPointF(100, -100);
		var right_down = center + new MPointF(100, 100);
		var left_down = center + new MPointF(-100, 100);
		SourceDrawer.DrawLine(left_up, right_up);
		SourceDrawer.DrawLine(right_up, right_down);
		SourceDrawer.DrawLine(right_down, left_down);
		SourceDrawer.DrawLine(left_down, left_up);
	}
	public void DrawChar(char c, MPointF center)
	{
		//DrawBorder(center);
		if (!DefinedCharacters.ContainsKey(c))
		{
			return;
		}
		ProjImage img = DefinedCharacters[c];
		SourceDrawer.Image.DrawImage(img, center);
	}

	public void DrawString(string s, MPointF center)
	{
		int width = LetterWidth * s.Length;
		int height = LetterHeight;
		MPointF left_up = new MPointF(center.X - width / 2, center.Y - height / 2);
		MPointF fc = left_up + new MPointF(LetterWidth / 2, LetterHeight / 2);
		for (int i = 0; i < s.Length; i++)
		{
			DrawChar(s[i], fc);
			fc += new MPointF(LetterWidth, 0);
		}
	}

	public void Emit(GameContext Context, MPointF Location)
	{
		SourceDrawer.Emit(Context, Location);
	}
}
