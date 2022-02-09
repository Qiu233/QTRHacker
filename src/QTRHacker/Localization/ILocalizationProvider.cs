using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Localization
{
	public interface ILocalizationProvider
	{
		void OnCultureChanged(object sender, CultureChangedEventArgs args);
	}
}
