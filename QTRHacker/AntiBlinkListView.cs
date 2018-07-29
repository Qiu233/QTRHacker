using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Terraria_Hacker
{
	class AntiBlinkListView : ListView
	{
		public AntiBlinkListView()
		{
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);

			this.SetStyle(ControlStyles.EnableNotifyMessage, true);
		}
		protected override void OnNotifyMessage(Message m)
		{
			if (m.Msg != 0x14)
			{
				base.OnNotifyMessage(m);
			}

		}
	}
}
