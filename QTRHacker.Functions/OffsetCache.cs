using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Functions
{
	/// <summary>
	/// Contains actual offset.
	/// </summary>
	public class OffsetCache
	{
		private readonly ConcurrentDictionary<string, uint> Cache = new();
		public GameContext Context { get; }

		public OffsetCache(GameContext context)
		{
			Context = context;
		}

		public void Flush()
		{
			Cache.Clear();
		}

		private static string GetComposedName(string typeName, string fieldName)
		{
			return $"{typeName}.{fieldName}";
		}

		public uint GetInstanceFieldOffset(string typeName, string fieldName)
		{
			string name = GetComposedName(typeName, fieldName);
			if (Cache.TryGetValue(name, out uint v))
				return v;
			return Cache[name] = Context.GameModuleHelper.GetInstanceFieldOffset(typeName, fieldName);
		}
	}
}
