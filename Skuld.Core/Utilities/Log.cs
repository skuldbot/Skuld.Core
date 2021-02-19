using Discord;
using Discord.Commands;
using Sentry;
using Skuld.Core.Extensions;
using Skuld.Core.Extensions.Conversion;
using Skuld.Core.Extensions.Formatting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Skuld.Core.Utilities
{
	public static class Log
	{
		public readonly static string CurrentLogFileName =
			DateTime.UtcNow.ToString(
				"yyyy-MM-dd",
				CultureInfo.InvariantCulture
			) + ".log";

		private static StreamWriter LogFile;
		private static bool hasBeenConfigured = false;
		private static ISentryClient sentryClient;

		public static void Configure(ISentryClient sentry)
		{
			if (!Directory.Exists(SkuldAppContext.LogDirectory))
			{
				Directory.CreateDirectory(SkuldAppContext.LogDirectory);
			}

			if (!hasBeenConfigured)
			{
				try
				{
					LogFile = new StreamWriter(
						File.Open(
							Path.Combine(
							  SkuldAppContext.LogDirectory,
							  CurrentLogFileName
							),
							FileMode.Append,
							FileAccess.Write,
							FileShare.Read
						)
					)
					{
						AutoFlush = true,
						NewLine = "\n"
					};
					hasBeenConfigured = true;
				}
				catch (Exception ex)
				{
					Console.WriteLine(
						Message(
							"Log",
							ex.Message,
							LogSeverity.Critical
						)
					);
				}

				sentryClient = sentry;
			}
		}

		private static string Message(string source,
									  string message,
									  LogSeverity severity)
		{
			var lines = new List<string[]>
			{
				new[]
				{
					string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy HH:mm:ss}", DateTime.UtcNow),
					"[" + source + "]",
					"[" + severity.ToString()[0] + "]",
					message??""
				}
			};

			var prettied = lines.PrettyLines(2);

			Console.ForegroundColor = severity.SeverityToColor();

			return prettied;
		}

		public static void Critical(string source,
									string message,
									ICommandContext context,
									Exception exception = null)
		{
			var msg = Message(source, message, LogSeverity.Critical);

			if (exception != null)
			{
				var m = msg + "EXTRA INFORMATION:\n" + exception.ToString();

				if (sentryClient != null)
				{
					SentryEvent @event = null;

					if (context != null)
					{
						@event = context.ToSentryEvent(exception);
					}
					else if (exception != null)
					{
						@event = exception.ToSentryEvent();
					}

					@event.Level = SentryLevel.Fatal;
					@event.SetTag("Source", source);

					if (@event != null)
					{
						sentryClient.CaptureEvent(@event);
					}
				}

				if (LogFile != null)
				{
					LogFile.WriteLine(m);
				}

				if (SkuldAppContext.GetLogLevel() >= LogSeverity.Critical)
				{
					Console.Out.WriteLine(m);
				}
			}
			else
			{
				if (LogFile != null)
				{
					LogFile.WriteLine(msg);
				}

				if (SkuldAppContext.GetLogLevel() >= LogSeverity.Critical)
				{
					Console.Out.WriteLine(msg);
				}
			}

			Console.ForegroundColor = ConsoleColor.White;

			if (LogFile != null)
			{
				LogFile.Flush();
			}
		}

		public static void Debug(string source,
								 string message,
								 ICommandContext context,
								 Exception exception = null)
		{
			var msg = Message(source, message, LogSeverity.Debug);

			if (sentryClient != null)
			{
				SentryEvent @event = null;

				if (context != null)
				{
					@event = context.ToSentryEvent(exception);
				}
				else if (exception != null)
				{
					@event = exception.ToSentryEvent();
				}

				@event.Level = SentryLevel.Debug;
				@event.SetTag("Source", source);

				if (@event != null)
				{
					sentryClient.CaptureEvent(@event);
				}
			}

			if (exception != null)
			{
				var m = msg + "EXTRA INFORMATION:\n" + exception.ToString();

				if (SkuldAppContext.GetLogLevel() >= LogSeverity.Debug)
				{
					Console.Out.WriteLine(m);
				}

				if (LogFile != null)
				{
					LogFile.WriteLine(m);
				}
			}
			else
			{
				if (SkuldAppContext.GetLogLevel() >= LogSeverity.Debug)
				{
					Console.Out.WriteLine(msg);
				}

				if (LogFile != null)
				{
					LogFile.WriteLine(msg);
				}
			}

			Console.ForegroundColor = ConsoleColor.White;

			if (LogFile != null)
			{
				LogFile.Flush();
			}
		}

		public static void Error(string source,
								 string message,
								 ICommandContext context,
								 Exception exception = null)
		{
			var msg = Message(source, message, LogSeverity.Error);

			if (SkuldAppContext.GetLogLevel() >= LogSeverity.Error)
			{
				Console.Out.WriteLine(msg);
			}

			if (exception != null)
			{
				var m = msg + "EXTRA INFORMATION:\n" + exception.ToString();

				if (sentryClient != null)
				{
					SentryEvent @event = null;

					if (context != null)
					{
						@event = context.ToSentryEvent(exception);
					}
					else if (exception != null)
					{
						@event = exception.ToSentryEvent();
					}

					@event.Level = SentryLevel.Error;
					@event.SetTag("Source", source);

					if (@event != null)
					{
						sentryClient.CaptureEvent(@event);
					}
				}


				if (LogFile != null)
				{
					LogFile.WriteLine(m);
				}
			}
			else
			{
				if (LogFile != null)
				{
					LogFile.WriteLine(msg);
				}
			}

			Console.ForegroundColor = ConsoleColor.White;

			if (LogFile != null)
			{
				LogFile.Flush();
			}
		}

		public static void Verbose(string source,
								   string message,
								   ICommandContext context,
								   Exception exception = null)
		{
			var msg = Message(source, message, LogSeverity.Verbose);

			if (SkuldAppContext.GetLogLevel() >= LogSeverity.Verbose)
			{
				Console.Out.WriteLine(msg);

				if (sentryClient != null)
				{
					SentryEvent @event = null;

					if (context != null)
					{
						@event = context.ToSentryEvent(exception);
					}
					else if (exception != null)
					{
						@event = exception.ToSentryEvent();
					}

					@event.Level = SentryLevel.Info;
					@event.SetTag("Source", source);

					if (@event != null)
					{
						sentryClient.CaptureEvent(@event);
					}
				}
			}

			if (exception != null)
			{
				var m = msg + "EXTRA INFORMATION:\n" + exception.ToString();

				if (LogFile != null)
				{
					LogFile.WriteLine(m);
				}
			}
			else
			{
				if (LogFile != null)
				{
					LogFile.WriteLine(msg);
				}
			}

			Console.ForegroundColor = ConsoleColor.White;

			if (LogFile != null)
			{
				LogFile.Flush();
			}
		}

		public static void Warning(string source,
								   string message,
								   ICommandContext context,
								   Exception exception = null)
		{
			var msg = Message(source, message, LogSeverity.Warning);

			if (SkuldAppContext.GetLogLevel() >= LogSeverity.Warning)
			{
				Console.Out.WriteLine(msg);
			}

			if (exception != null)
			{
				var m = msg + "EXTRA INFORMATION:\n" + exception.ToString();

				if (sentryClient != null)
				{
					SentryEvent @event = null;

					if (context != null)
					{
						@event = context.ToSentryEvent(exception);
					}
					else if (exception != null)
					{
						@event = exception.ToSentryEvent();
					}

					@event.Level = SentryLevel.Warning;
					@event.SetTag("Source", source);

					if (@event != null)
					{
						sentryClient.CaptureEvent(@event);
					}
				}

				if (LogFile != null)
				{
					LogFile.WriteLine(m);
				}
			}
			else
			{
				if (LogFile != null)
				{
					LogFile.WriteLine(msg);
				}
			}

			Console.ForegroundColor = ConsoleColor.White;

			if (LogFile != null)
			{
				LogFile.Flush();
			}
		}

		public static void Info(string source,
								string message)
		{
			var msg = Message(source, message, LogSeverity.Info);

			if (SkuldAppContext.GetLogLevel() >= LogSeverity.Info)
			{
				Console.Out.WriteLine(msg);
			}

			if (LogFile != null)
			{
				LogFile.WriteLine(msg);
			}

			Console.ForegroundColor = ConsoleColor.White;

			if (LogFile != null)
			{
				LogFile.Flush();
			}
		}

		public static void FlushNewLine()
		{
			LogFile.WriteLine("-------------------------------------------");

			LogFile.Close();
		}
	}
}