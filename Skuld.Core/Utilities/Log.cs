﻿using Discord;
using Skuld.Core.Extensions.Conversion;
using Skuld.Core.Extensions.Formatting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Skuld.Core.Utilities
{
    public class Log
    {
        public readonly static string CurrentLogFileName = DateTime.UtcNow.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + ".log";

        private static StreamWriter LogFile;
        private static bool hasBeenConfigured = false;

        public static void Configure()
        {
            if (Directory.Exists(SkuldAppContext.LogDirectory))
            {
                if (!hasBeenConfigured)
                {
                    LogFile = new StreamWriter(
                        File.Open(
                            Path.Combine(SkuldAppContext.LogDirectory, CurrentLogFileName),
                            FileMode.Append,
                            FileAccess.Write,
                            FileShare.Read)
                        )
                    {
                        AutoFlush = true,
                        NewLine = "\n"
                    };
                    hasBeenConfigured = true;
                }
            }
        }

        private static string Message(string source, string message, LogSeverity severity)
        {
            Configure();

            var lines = new List<string[]>
            {
                new[]
                {
                    string.Format("{0:dd/MM/yyyy HH:mm:ss}", DateTime.UtcNow),
                    "[" + source + "]",
                    "[" + severity.ToString()[0] + "]",
                    message??""
                }
            };

            var prettied = lines.PrettyLines(2);

            Console.ForegroundColor = severity.SeverityToColor();

            return prettied;
        }

        public static void Critical(string source, string message, Exception exception = null)
        {
            var msg = Message(source, message, LogSeverity.Critical);

            if (exception != null)
            {
                var m = msg + "EXTRA INFORMATION:\n" + exception.ToString();

                if (LogFile != null)
                {
                    LogFile.WriteLine(m);
                }

                if (SkuldAppContext.GetLogLevel() >= LogSeverity.Critical)
                    Console.Out.WriteLine(m);
            }
            else
            {
                if(LogFile != null)
                {
                    LogFile.WriteLine(msg);
                }

                if (SkuldAppContext.GetLogLevel() >= LogSeverity.Critical)
                    Console.Out.WriteLine(msg);
            }

            Console.ForegroundColor = ConsoleColor.White;

            if (LogFile != null)
            {
                LogFile.Flush();
            }
        }

        public static void Debug(string source, string message, Exception exception = null)
        {
            var msg = Message(source, message, LogSeverity.Debug);

            if (exception != null)
            {
                var m = msg + "EXTRA INFORMATION:\n" + exception.ToString();

                if (SkuldAppContext.GetLogLevel() >= LogSeverity.Debug)
                {
                    Console.Out.WriteLine(m);

                    if (LogFile != null)
                    {
                        LogFile.WriteLine(m);
                    }
                }
            }
            else
            {
                if (SkuldAppContext.GetLogLevel() >= LogSeverity.Debug)
                {
                    Console.Out.WriteLine(msg);
                    
                    if (LogFile != null)
                    {
                        LogFile.WriteLine(msg);
                    }
                }
            }

            Console.ForegroundColor = ConsoleColor.White;

            if (LogFile != null)
            {
                LogFile.Flush();
            }
        }

        public static void Error(string source, string message, Exception exception = null)
        {
            var msg = Message(source, message, LogSeverity.Error);

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

            if (SkuldAppContext.GetLogLevel() >= LogSeverity.Error)
                Console.Out.WriteLine(msg);

            Console.ForegroundColor = ConsoleColor.White;

            if (LogFile != null)
            {
                LogFile.Flush();
            }
        }

        public static void Verbose(string source, string message, Exception exception = null)
        {
            var msg = Message(source, message, LogSeverity.Verbose);

            if (SkuldAppContext.GetLogLevel() >= LogSeverity.Verbose)
            {
                Console.Out.WriteLine(msg);

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
            }

            Console.ForegroundColor = ConsoleColor.White;

            if (LogFile != null)
            {
                LogFile.Flush();
            }
        }

        public static void Warning(string source, string message, Exception exception = null)
        {
            var msg = Message(source, message, LogSeverity.Warning);

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

            if (SkuldAppContext.GetLogLevel() >= LogSeverity.Warning)
                Console.Out.WriteLine(msg);

            Console.ForegroundColor = ConsoleColor.White;

            if (LogFile != null)
            {
                LogFile.Flush();
            }
        }

        public static void Info(string source, string message)
        {
            var msg = Message(source, message, LogSeverity.Info);

            if (LogFile != null)
            {
                LogFile.WriteLine(msg);
            }

            if (SkuldAppContext.GetLogLevel() >= LogSeverity.Info)
                Console.Out.WriteLine(msg);

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