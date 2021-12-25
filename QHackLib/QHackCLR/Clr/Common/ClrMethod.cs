using QHackCLR.Clr.Builders.Helpers;
using QHackCLR.Dac;
using QHackCLR.Dac.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using QHackCLR.Metadata.Parse;
using QHackCLR.Metadata.Parse.Signature;
using QHackCLR.Dac.Interfaces;
using QHackCLR.Dac.Helpers;

namespace QHackCLR.Clr
{
	public unsafe class ClrMethod : ClrEntity, IHasMetadata
	{
		internal readonly DacpMethodDescData Data;
		protected IMethodHelper MethodHelper { get; }

		public string Signature { get; }
		public MethodAttributes MethodAttributes { get; }

		public int MDToken => Data.MDToken;

		public ClrMethod(IMethodHelper helper, nuint handle) : base(handle)
		{
			MethodHelper = helper;
			helper.SOSDac.GetMethodDescData(handle, 0u, out Data, 0, null, out uint _);
			Signature = helper.SOSDac.GetMethodDescName(handle);
		}


		public string Name
		{
			get
			{
				string signature = Signature;
				if (signature is null)
					return null;
				int last = signature.LastIndexOf('(');
				if (last > 0)
				{
					int first = signature.LastIndexOf('.', last - 1);
					if (first != -1 && signature[first - 1] == '.')
						first--;
					return signature.Substring(first + 1, last - first - 1);
				}
				return "{error}";
			}
		}

		#region
		public ClrType _DeclaringType;
		#endregion

		public ClrType DeclaringType => _DeclaringType ??= MethodHelper.TypeFactory.GetClrType(Data.MethodTablePtr);
		public nuint NativeCode => Data.NativeCodeAddr;

		/*public string GetSignature()
		{
			var module = DeclaringType.Module;
			var metadata = module.MetadataImport;
			string name = metadata.GetMethodName(MDToken);
			metadata.GetMethodProps(MDToken, out MethodAttributes attr, out SigParser parser);
			StringBuilder sigBuilder = new();
			MethodSig methodSig = new(parser);
			//sigBuilder.Append(methodSig.RetType.Type==null?():());
			sigBuilder.Append(' ');
			sigBuilder.Append(name);
			sigBuilder.Append('(');
			sigBuilder.Append(string.Join(',', 
				methodSig.Params.Zip(EnumerateParams().Select(t => t.Item2)).
				Select(s => $"{s.First.Type.Type} {s.Second}")));
			sigBuilder.Append(')');
			return sigBuilder.ToString();
		}*/

		private IEnumerable<(int, string, int)> EnumerateParams()
		{
			var metadata = DeclaringType.Module.MetadataImport;
			foreach (var token in metadata.EnumParams(MDToken))
			{
				string name = metadata.GetParamName(token);
				metadata.GetParamProps(token, out uint index, out _, out _);
				yield return ((int)index, name, token);
			}
		}
	}
}
