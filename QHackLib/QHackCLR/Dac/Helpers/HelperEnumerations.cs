using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.Dac.Helpers
{
	public sealed unsafe class GeneralEnumerator<T> : IEnumerator<T>
	{
		public sealed class Options
		{
			public string EnumStartPrefix;
			public string EnumDoPrefix;
			public string EnumEndPrefix;
			public string EnumStartSuffix;
			public string EnumDoSuffix;
			public string EnumEndSuffix;

			public Options()
			{

			}

			public Options(string enumStartPrefix, string enumDoPrefix, string enumEndPrefix,
				string enumStartSuffix, string enumDoSuffix, string enumEndSuffix)
			{
				EnumStartPrefix = enumStartPrefix;
				EnumDoPrefix = enumDoPrefix;
				EnumEndPrefix = enumEndPrefix;
				EnumStartSuffix = enumStartSuffix;
				EnumDoSuffix = enumDoSuffix;
				EnumEndSuffix = enumEndSuffix;
			}
		}

		public const string DefaultEnumStartPrefix = "StartEnum";
		public const string DefaultEnumDoPrefix = "Enum";
		public const string DefaultEnumEndPrefix = "EndEnum";
		public const string DefaultEnumStartSuffix = "s";
		public const string DefaultEnumDoSuffix = "";
		public const string DefaultEnumEndSuffix = "s";

		public static readonly Options DefaultOptions = new("StartEnum", "Enum", "EndEnum", "s", "", "s");

		public delegate HRESULT EnumStart(out nuint handle);
		public delegate HRESULT EnumDo(ref nuint handle, out T obj);
		public delegate HRESULT EnumEnd(nuint handle);

		private EnumStart Start { get; }
		private EnumDo Do { get; }
		private EnumEnd End { get; }

		private T _Value;
		private nuint _Handle = 0;

		public T Current => _Value;
		object IEnumerator.Current => Current;

		public static GeneralEnumerator<T> Create(EnumStart start, EnumDo @do, EnumEnd end) => new(start, @do, end);

		public GeneralEnumerator(EnumStart start, EnumDo @do, EnumEnd end)
		{
			Start = start;
			Do = @do;
			End = end;
		}

		public IEnumerable<T> GetAll()
		{
			while (MoveNext())
				yield return Current;
		}

		public bool MoveNext()
		{
			if (_Handle == 0)
				Start(out _Handle);
			Do(ref _Handle, out _Value);
			return _Handle != 0 && _Value is not null;
		}

		public void Reset()
		{
			_Handle = 0;
		}

		public void Dispose()
		{
			End(_Handle);
		}
	}
}
