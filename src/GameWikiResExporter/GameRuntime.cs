using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace GameWikiResExporter;

internal sealed class GameRuntime : IDisposable
{
	private readonly string directory;
	public Assembly Assembly { get; }

	public GameRuntime(string executable)
	{
		directory = Path.GetDirectoryName(executable);
		AppDomain.CurrentDomain.AssemblyResolve += ResolveAssembly;
		Assembly = Assembly.LoadFrom(executable);
	}

	private Assembly ResolveAssembly(object sender, ResolveEventArgs args)
	{
		string name = new AssemblyName(args.Name).Name;
		if (Assembly != null && name == Assembly.GetName().Name)
			return Assembly;
		// Prefer libraries embedded in the selected game, since loose GameRefs may be older.
		string resource = Assembly?.GetManifestResourceNames()
			.SingleOrDefault(value => value.EndsWith("." + name + ".dll", StringComparison.OrdinalIgnoreCase));
		if (resource != null)
		{
			using var source = Assembly.GetManifestResourceStream(resource);
			using var buffer = new MemoryStream();
			source.CopyTo(buffer);
			return Assembly.Load(buffer.ToArray());
		}
		string path = Path.Combine(directory, name + ".dll");
		return File.Exists(path) ? Assembly.LoadFrom(path) : null;
	}

	public void Dispose() => AppDomain.CurrentDomain.AssemblyResolve -= ResolveAssembly;
}
