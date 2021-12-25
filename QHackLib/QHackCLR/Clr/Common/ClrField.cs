using QHackCLR.Clr.Builders.Helpers;
using QHackCLR.Dac;
using QHackCLR.Dac.Interfaces;
using QHackCLR.Dac.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.Clr
{
	public abstract class ClrField : ClrEntity, IHasMetadata
	{
		public readonly DacpFieldDescData Data;
		protected IFieldHelper FieldHelper { get; }

		private readonly string _Name;
		private readonly FieldAttributes _Attributes;

		public string Name => _Name;
		public FieldAttributes Attributes => _Attributes;

		/// <summary>
		/// The type of this field's definition.
		/// </summary>
		public ClrType Type { get; }

		/// <summary>
		/// The type declaring this field.
		/// </summary>
		public ClrType DeclaringType { get; }


		public bool IsStatic => Data.BIsStatic != 0;

		protected ClrField(ClrType decType, IFieldHelper helper, nuint handle) : base(handle)
		{
			DeclaringType = decType;
			FieldHelper = helper;
			helper.SOSDac.GetFieldDescData(handle, out Data);
			Type = helper.TypeFactory.GetClrType(Data.MTOfType);
			helper.GetFieldProps(decType, Data.FieldToken, out _Name, out _Attributes, out _);
		}

		/// <summary>
		/// <para>Offset of an instance field is calculated from offset_base but is relative to its ref_base.<br/>
		/// Static field's offset is relative to its DomainLocalModule's [Non]GCStaticDataStart.</para>
		/// </summary>
		public uint Offset => Data.DwOffset;

		public CorElementType CorElementType => Data.Type;

		public int MDToken => Data.FieldToken;
	}
}
