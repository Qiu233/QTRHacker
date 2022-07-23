using QHackCLR.Common;
using QHackLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.ViewModels.Common.PropertyEditor
{
	public class PropertyComplex : PropertyBase
	{
		public ObservableCollection<PropertyBase> Items { get; } = new();
		public override string Text => $"{Name}: {GetUFName(Entity.Type.Name)}-{(Entity is HackValue ? "V" : "R")} <- {ValueSig}";
		public PropertyComplex(HackEntity entity, string name) : base(entity, name)
		{
			Items.Add(null);
			IsExpandedChanged += PropertyComplex_IsExpandedChanged;
		}

		protected bool IsNullRef => Entity.BaseAddress == 0;

		protected virtual string ValueSig
		{
			get
			{
				if (IsNullRef)
					return "null";
				return "0x" + Entity.BaseAddress.ToString(IntPtr.Size == 8 ? "X16" : "X8");
			}
		}

		private void AddItems(ClrType type, bool fullName)
		{
			var entries = type.EnumerateInstanceFields().GroupBy(t => t.Type.IsValueType);
			var values = entries.Where(t => t.Key).SelectMany(t => t).ToList();
			var refs = entries.Where(t => !t.Key).SelectMany(t => t).ToList();
			values.Sort((t1, t2) => t1.Name.CompareTo(t2.Name));
			refs.Sort((t1, t2) => t1.Name.CompareTo(t2.Name));
			values.ForEach(t => Items.Add(New(Entity.InternalGetMember(t.Name), $"{(fullName ? t.DeclaringType.Name + "." : "")}{t.Name}")));
			refs.ForEach(t => Items.Add(New(Entity.InternalGetMember(t.Name), $"{(fullName ? t.DeclaringType.Name + "." : "")}{t.Name}")));
		}

		public virtual void AddItems()
		{
			ClrType cur = Entity.Type;
			List<ClrType> types = new();
			while (true)
			{
				types.Add(cur);
				cur = cur.BaseType;
				if (cur == null)
					break;
			}
			types.Reverse();
			for (int i = 0; i < types.Count - 1; i++)
			{
				AddItems(types[i], true);
			}
			AddItems(types[types.Count - 1], false);
		}

		private void PropertyComplex_IsExpandedChanged(object sender, EventArgs e)
		{
			if (IsNullRef)
				return;
			if (IsExpanded)
			{
				Items.Clear();
				AddItems();
			}
			else
			{
				Items.Add(null);
			}
		}

	}
}
