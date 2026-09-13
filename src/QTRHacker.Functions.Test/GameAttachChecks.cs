using QTRHacker.Core.GameObjects;
using QTRHacker.ViewModels.Common;
using QTRHacker.ViewModels.Common.PropertyEditor;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Threading;

namespace QTRHacker.Functions.Test;

// Read-only integration check for the path used after dropping the cross on Terraria.
// Use the real player-list ViewModel and its DispatcherTimer, including string conversions.
internal static class GameAttachChecks
{
	public static void Verify()
	{
		// The console runner is the entry assembly. Match the application's resource owner
		// for its unqualified pack URIs; WPF's public setter only permits unmanaged hosts.
		var resourceAssembly = typeof(HackGlobal).Assembly;
		const BindingFlags flags = BindingFlags.Static | BindingFlags.NonPublic;
		typeof(System.Windows.Application).GetField("_resourceAssembly", flags).SetValue(null, resourceAssembly);
		typeof(System.Windows.Navigation.BaseUriHelper).GetProperty("ResourceAssembly", flags).SetValue(null, resourceAssembly);
		string originalDirectory = Environment.CurrentDirectory;
		string scratch = Path.Combine(Path.GetTempPath(), "QTRHacker-attach-check-" + Guid.NewGuid().ToString("N"));
		Directory.CreateDirectory(scratch);
		// HackGlobal initializes logging in the current directory; keep existing user logs untouched.
		Environment.CurrentDirectory = scratch;
		var timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(30) };
		var timeout = new DispatcherTimer { Interval = TimeSpan.FromSeconds(15) };
		try
		{
			using var process = Process.GetProcessesByName("Terraria").First();
			HackGlobal.Initialize(process.Id);
			var context = HackGlobal.GameContext;
			var obj = context.GameModuleHelper.GetStaticHackObject("Terraria.Main", "versionNumber");
			var version = new GameString(context, obj);
			string versionText = version.GetValue();
			if (versionText.Length == 0 || version.Length != versionText.Length)
				throw new InvalidOperationException("Could not read the game version after attaching.");
			if (new PropertyString(obj, "Version").Value != versionText)
				throw new InvalidOperationException("The property editor read a different string.");

			var viewModel = new PlayersListViewViewModel(timer);
			var frame = new DispatcherFrame();
			int updates = 0;
			timer.Tick += (_, _) =>
			{
				if (++updates == 20)
					frame.Continue = false;
			};
			timeout.Tick += (_, _) => frame.Continue = false;
			timer.Start();
			timeout.Start();
			Dispatcher.PushFrame(frame);
			timer.Stop();
			timeout.Stop();
			if (updates != 20)
				throw new TimeoutException("Player-list timer did not finish the integration check.");
			if (viewModel.Players.Count == 0)
				throw new InvalidOperationException("Enter a world before running the player-list check.");
			foreach (var player in viewModel.Players)
			{
				var name = context.Players[player.ID].Name_obj;
				if (player.Name != name.GetValue() || player.Name != GameString.GetString(name.TypedInternalObject))
					throw new InvalidOperationException("Player-list name differs from the property-editor string.");
			}
			PlayerEditorChecks.Verify(context.Players[viewModel.Players[0].ID]);
			Console.WriteLine($"Attach UI checks passed: game version, property editor, {viewModel.Players.Count} active players, and {updates} player-list timer updates.");
			GC.KeepAlive(viewModel);
		}
		finally
		{
			timer.Stop();
			timeout.Stop();
			HackGlobal.GameContext?.Dispose();
			Environment.CurrentDirectory = originalDirectory;
		}
	}
}
