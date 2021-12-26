using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Languages
{
	public class Language
	{
		public string Name
		{
			get;
		}
		public IDictionary<string, string> Words
		{
			get;
		}
		public string this[string n]
		{
			get
			{
				return Words[n];
			}
		}
		private Language(string name)
		{
			Name = name;
			Words = new Dictionary<string, string>();
		}

		public static Language GetLanguage(string tName)
		{
			var s = System.Reflection.Assembly.GetExecutingAssembly().
				GetManifestResourceStream("QTRHacker.Languages.Languages.json");
			byte[] b = new byte[s.Length];
			s.Read(b, 0, (int)s.Length);
			JObject src = JsonConvert.DeserializeObject<JObject>(Encoding.UTF8.GetString(b))[tName] as JObject;

			Language n = new Language(tName);
			foreach (var tt in src)
				n.Words[tt.Key] = tt.Value.ToString();
			s.Close();
			return n;
		}
	}
}
