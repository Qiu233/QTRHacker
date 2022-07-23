using QHackCLR.Common;
using QHackLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.ViewModels.Common.PropertyEditor
{
	public class PropertyArray : PropertyComplex
	{
		public override string Text => $"{Name}: {GetUFName(Entity.Type.Name)}-{(Entity is HackValue ? "V" : "R")} <- {ValueSig}";
		public PropertyArray(HackEntity entity, string name) : base(entity, name)
		{
		}
		public override void AddItems()
		{
			base.AddItems();
			if (!Entity.Type.IsArray || Entity is not HackObject ho)
				return;
			int len = ho.GetArrayLength();
			int[] indices = new int[1];
			for (int i = 0; i < len; i++)
			{
				indices[0] = i;
				Items.Add(New(ho.InternalGetIndex(indices), $"[{i}]"));
			}
		}
	}
}
