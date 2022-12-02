using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;

namespace QTRHacker.Core.ProjectileImage.RainbowImage;

public class CharactersLoader
{
	public static void LoadCharacters(Dictionary<char, ProjImage> chs, string xml)
	{
		var r = LoadCharacters(xml);
		foreach (var pair in r)
			if (!chs.ContainsKey(pair.Key))
				chs.Add(pair.Key, pair.Value);
	}
	public static Dictionary<char, ProjImage> LoadCharacters(string xml)
	{
		Dictionary<char, ProjImage> result = new Dictionary<char, ProjImage>();
		XmlDocument doc = new XmlDocument();
		doc.LoadXml(xml);
		var data = doc["data"];
		foreach (var c in data.ChildNodes)
		{
			var ch = ParseChar(c as XmlElement);
			result.Add(ch.Item1, ch.Item2);
		}
		return result;
	}
	public static Tuple<char, ProjImage> ParseChar(XmlElement ch)
	{
		return new Tuple<char, ProjImage>(ch["type"].InnerText[0], ParseBody(ch["body"]));
	}
	public static ProjImage ParseBody(XmlElement body)
	{
		RainbowDrawer drawer = new RainbowDrawer();
		foreach (var c in body.ChildNodes)
		{
			var proj = c as XmlElement;
			switch (proj.Name)
			{
				case "line":
					drawer.DrawLine(ParseMPointF(proj.Attributes["start"].InnerText), ParseMPointF(proj.Attributes["end"].InnerText));
					break;
				case "point":
					drawer.DrawPoint(ParseMPointF(proj.Attributes["location"].InnerText), Convert.ToSingle(proj.Attributes["direction"].InnerText, CultureInfo.InvariantCulture));
					break;
				case "arc":
					drawer.DrawArc(ParseMPointF(proj.Attributes["center"].InnerText),
						Convert.ToSingle(proj.Attributes["radius"].InnerText, CultureInfo.InvariantCulture),
						Convert.ToSingle(proj.Attributes["start_radian"].InnerText, CultureInfo.InvariantCulture),
						Convert.ToSingle(proj.Attributes["end_radian"].InnerText, CultureInfo.InvariantCulture));
					break;
				case "arcf":
					drawer.DrawArc(ParseMPointF(proj.Attributes["center"].InnerText),
						Convert.ToSingle(proj.Attributes["radius"].InnerText, CultureInfo.InvariantCulture),
						(float)Math.PI * Convert.ToSingle(proj.Attributes["start_radian_factor"].InnerText, CultureInfo.InvariantCulture),
						(float)Math.PI * Convert.ToSingle(proj.Attributes["end_radian_factor"].InnerText, CultureInfo.InvariantCulture));
					break;
				default:
					break;
			}
		}
		return drawer.Image;
	}

	private static MPointF ParseMPointF(string s)
	{
		string[] t = s.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
		return new MPointF(Convert.ToSingle(t[0], CultureInfo.InvariantCulture), Convert.ToSingle(t[1], CultureInfo.InvariantCulture));
	}
}
