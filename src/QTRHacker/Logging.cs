using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker
{
	internal class Logging
	{
		public enum LogLevel
		{
			INFO,
			ALERT,
			WARNING,
			ERROR,
		}
		public IndentedTextWriter LogWriter { get; }

		private Logging(Stream stream) => LogWriter = new IndentedTextWriter(new StreamWriter(stream) { AutoFlush = true });

		public static Logging New(Stream stream)
		{
			Logging logging = new(stream);
			return logging;
		}

		public void Log(string msg, LogLevel level = LogLevel.INFO)
		{
			LogWriter.WriteLine($"[{DateTime.Now}][{level}] {msg}");
			LogWriter.Flush();
		}

		public void Alert(string msg) => Log(msg, LogLevel.ALERT);
		public void Error(string msg) => Log(msg, LogLevel.ERROR);
		public void Warn(string msg) => Log(msg, LogLevel.WARNING);

		public void Enter(string label = null)
		{
			if (label != null)
				LogWriter.WriteLine($"SECTION [{label}]");
			LogWriter.Indent++;
		}

		public void Exit()
		{
			if (LogWriter.Indent > 0)
				LogWriter.Indent--;
		}
	}
}
