using System.CodeDom.Compiler;
using System.IO;
using System.Reflection;

namespace QTRHacker;

public class Logging
{
	public enum LogLevel
	{
		INFO,
		ALERT,
		WARNING,
		ERROR,
	}
	public IndentedTextWriter LogWriter { get; }

	private readonly Stack<string> Sections = new();
	private readonly object _lock = new();

	private Logging(Stream stream) => LogWriter = new IndentedTextWriter(new StreamWriter(stream) { AutoFlush = true });

	public static Logging New(Stream stream)
	{
		Logging logging = new(stream);
		logging.Log($"Hack Version:\t{Assembly.GetExecutingAssembly().GetName().Version}-{HackGlobal.GameVersion}");
		logging.Log($"OS:\t{Environment.OSVersion}");
		logging.Log($"OS is X64:\t{Environment.Is64BitOperatingSystem}");
		return logging;
	}

	public void Log(string msg, LogLevel level = LogLevel.INFO)
	{
		lock (_lock)
		{
			LogWriter.WriteLine($"[{DateTime.Now}][{level}] {msg}");
			LogWriter.Flush();
		}
	}

	public void Alert(string msg) => Log(msg, LogLevel.ALERT);
	public void Error(string msg) => Log(msg, LogLevel.ERROR);
	public void Warn(string msg) => Log(msg, LogLevel.WARNING);

	public void Exception(Exception? e, int layer = 0)
	{
		if (e is null)
			return;
		string s = $"({layer}): {e.Message}\n{e.StackTrace}";
		Error(s);
		Exception(e.InnerException, layer + 1);
	}

	public void Enter(string? label = null)
	{
		lock (_lock)
		{
			Sections.Push(label);
			if (label != null)
				LogWriter.WriteLine($"SECTION [{label}]");
			LogWriter.Indent++;
		}
	}

	public void Exit()
	{
		lock (_lock)
		{
			if (LogWriter.Indent > 0)
				LogWriter.Indent--;
			string section = Sections.Pop();
			LogWriter.WriteLine($"END {section}");
		}
	}
}
