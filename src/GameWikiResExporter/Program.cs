using System;
using System.IO;

namespace GameWikiResExporter;

internal static class Program
{
	[STAThread]
	private static int Main(string[] args)
	{
		try
		{
			string gameDirectory = null;
			string outputDirectory = AppDomain.CurrentDomain.BaseDirectory;
			bool noImages = false;
			for (int i = 0; i < args.Length; i++)
			{
				switch (args[i])
				{
					case "--help":
					case "-h":
						Console.WriteLine("GameWikiResExporter [--game-dir <directory>] [--output <directory>] [--no-images]");
						Console.WriteLine("Default source: repository GameRefs; default output: executable directory.");
						Console.WriteLine("Use --game-dir with a Terraria installation to also export Item/NPC XNB images.");
						Console.WriteLine("Outputs: WikiRes.zip, Localization.zip, GameConstants.cs, Items.bin, NPCs.bin.");
						return 0;
					case "--game-dir":
						gameDirectory = ReadValue(args, ref i);
						break;
					case "--output":
						outputDirectory = ReadValue(args, ref i);
						break;
					case "--no-images":
						noImages = true;
						break;
					default:
						throw new ArgumentException($"Unknown argument: {args[i]}. Use --help for usage.");
				}
			}

			bool explicitGameDirectory = gameDirectory != null;
			gameDirectory = Path.GetFullPath(gameDirectory ?? FindGameRefs());
			outputDirectory = Path.GetFullPath(outputDirectory);
			string executable = Path.Combine(gameDirectory, "Terraria.exe");
			if (!File.Exists(executable))
				throw new FileNotFoundException("Terraria.exe was not found. Pass --game-dir <Terraria directory>.", executable);
			string imagesDirectory = Path.Combine(gameDirectory, "Content", "Images");
			if (!noImages && !Directory.Exists(imagesDirectory))
			{
				if (explicitGameDirectory)
					throw new DirectoryNotFoundException($"Missing {imagesDirectory}. Use --no-images for data only.");
				Console.WriteLine("GameRefs has no Content/Images; exporting data and localization only. Use --game-dir for images.");
				noImages = true;
			}

			Console.WriteLine($"Source: {executable}");
			Console.WriteLine($"Output: {outputDirectory}");
			Directory.CreateDirectory(outputDirectory);
			using (var runtime = new GameRuntime(executable))
			{
				Console.WriteLine($"Terraria version: {runtime.Assembly.GetName().Version}");
				// Set this before Terraria.Main's static initializer; never use the player's saves.
				runtime.Assembly.GetType("Terraria.Program", true).GetField("SavePath")
					.SetValue(null, outputDirectory);
				WikiExporter.Export(outputDirectory, noImages ? null : imagesDirectory);
			}
			return 0;
		}
		catch (Exception exception)
		{
			Console.Error.WriteLine(exception);
			return 1;
		}
	}

	private static string ReadValue(string[] args, ref int index)
	{
		string option = args[index];
		if (++index == args.Length || string.IsNullOrWhiteSpace(args[index]) || args[index].StartsWith("--"))
			throw new ArgumentException($"{option} requires a directory.");
		return args[index];
	}

	private static string FindGameRefs()
	{
		for (var directory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory); directory != null; directory = directory.Parent)
		{
			string candidate = Path.Combine(directory.FullName, "GameRefs");
			if (File.Exists(Path.Combine(candidate, "Terraria.exe")))
				return candidate;
		}
		throw new DirectoryNotFoundException("Cannot locate GameRefs. Pass --game-dir <Terraria directory>.");
	}
}
