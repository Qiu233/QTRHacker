using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace QTRHacker.Functions.ProjectileImage.RainbowImage
{
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
			Console.WriteLine(data.HasChildNodes);
			foreach (var c in data.ChildNodes)
			{
				var ch = c as XmlElement;
				result.Add(ch["type"].InnerText[0], ParseBody(ch["body"]));
			}
			return result;
		}
		private static ProjImage ParseBody(XmlElement body)
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
						drawer.DrawPoint(ParseMPointF(proj.Attributes["location"].InnerText), Convert.ToSingle(proj.Attributes["direction"].InnerText));
						break;
					case "arc":
						drawer.DrawArc(ParseMPointF(proj.Attributes["center"].InnerText),
							Convert.ToSingle(proj.Attributes["radius"].InnerText),
							Convert.ToSingle(proj.Attributes["start_radian"].InnerText),
							Convert.ToSingle(proj.Attributes["end_radian"].InnerText));
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
			return new MPointF(Convert.ToSingle(t[0]), Convert.ToSingle(t[1]));
		}
	}
}
