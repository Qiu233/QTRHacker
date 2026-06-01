using Microsoft.Diagnostics.Runtime;
using System;
using System.Collections.Concurrent;
using System.Linq;
using QHackClrMethod = QHackCLR.Common.ClrMethod;
using ClrMdMethod = Microsoft.Diagnostics.Runtime.ClrMethod;
using ClrMdRuntime = Microsoft.Diagnostics.Runtime.ClrRuntime;

namespace QHackLib
{
	internal static class ClrMdNativeMethodResolver
	{
		private static readonly ConcurrentDictionary<int, Lazy<RuntimeCache>> Caches = new();

		public static nuint Resolve(QHackContext context, QHackClrMethod method)
		{
			if (method is null)
				return 0;

			var cache = Caches.GetOrAdd(context.ProcessID, pid => new Lazy<RuntimeCache>(() => new RuntimeCache(pid)));
			return cache.Value.Resolve(method);
		}

		private sealed class RuntimeCache : IDisposable
		{
			private readonly DataTarget _target;
			private readonly ClrMdRuntime _runtime;
			private readonly ConcurrentDictionary<ulong, ulong> _methodsByDesc = new();
			private readonly object _scanLock = new();
			private bool _allMethodsScanned;

			public RuntimeCache(int pid)
			{
				_target = DataTarget.AttachToProcess(pid, suspend: false);
				_runtime = _target.ClrVersions[0].CreateRuntime();
			}

			public nuint Resolve(QHackClrMethod method)
			{
				ulong methodDesc = method.ClrHandle.ToUInt64();
				if (methodDesc == 0)
					return 0;

				if (_methodsByDesc.TryGetValue(methodDesc, out ulong cached))
					return (nuint)cached;

				ClrMdMethod resolved = _runtime.GetMethodByHandle(methodDesc);
				if (IsCallable(resolved))
				{
					_methodsByDesc[methodDesc] = resolved.NativeCode;
					return (nuint)resolved.NativeCode;
				}

				ScanAllMethods();
				return _methodsByDesc.TryGetValue(methodDesc, out cached) ? (nuint)cached : 0;
			}

			private void ScanAllMethods()
			{
				if (_allMethodsScanned)
					return;

				lock (_scanLock)
				{
					if (_allMethodsScanned)
						return;

					foreach (ClrMdMethod method in _runtime.EnumerateModules()
						.SelectMany(m => m.EnumerateTypeDefToMethodTableMap())
						.Select(entry => _runtime.GetTypeByMethodTable(entry.MethodTable))
						.Where(t => t != null)
						.SelectMany(t => t.Methods))
					{
						if (IsCallable(method))
							_methodsByDesc.TryAdd(method.MethodDesc, method.NativeCode);
					}

					_allMethodsScanned = true;
				}
			}

			private static bool IsCallable(ClrMdMethod method)
			{
				return method != null && method.NativeCode != 0 && method.NativeCode != uint.MaxValue;
			}

			public void Dispose()
			{
				_target.Dispose();
			}
		}
	}
}
