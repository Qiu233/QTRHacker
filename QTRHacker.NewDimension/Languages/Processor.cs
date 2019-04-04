using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.NewDimension.Languages
{
	public class Processor
	{
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
		private Processor()
		{
			Words = new Dictionary<string, string>();
		}

		public static Processor GetLanguage(string tName)
		{
			var s = System.Reflection.Assembly.GetExecutingAssembly().
				GetManifestResourceStream("QTRHacker.NewDimension.Languages." + tName.ToLower() + ".txt");
			byte[] b = new byte[s.Length];
			s.Read(b, 0, (int)s.Length);
			string[] str = Encoding.UTF8.GetString(b).Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

			Processor n = new Processor();
			foreach (var tt in str)
			{
				string key = tt.Substring(0, tt.IndexOf('\t'));
				string content = tt.Substring(tt.IndexOf('\t') + 1);
				n.Words[key] = content;
			}
			return n;
		}
	}
}
