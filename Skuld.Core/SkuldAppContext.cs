using Discord;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Skuld.Core
{
	public static class SkuldAppContext
	{
		public static string BaseDirectory => AppContext.BaseDirectory;
		public static string TargetFrameworkName => AppContext.TargetFrameworkName;
		public static string ConfigurationId { get; private set; }

		public const int PLACEIMAGESIZE = 500;

		public const string Website = "https://skuldbot.uk";
		public const string WebsiteLeaderboard = Website + "/leaderboard";
		public const string LeaderboardExperience = WebsiteLeaderboard + "/experience";
		public const string LeaderboardMoney = WebsiteLeaderboard + "/money";
		public const string ConfigEnvVar = "SKULD_CONFIGID";
		public const string ConStrEnvVar = "SKULD_CONNSTR";
		public const string LogLvlEnvVar = "SKULD_LOGLEVEL";
		public const string SentryIOEnvVar = "SKULD_SENTRYIO";
		private static readonly Regex regex = new(@"((http|ftp|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?)", RegexOptions.IgnoreCase);
		public static Regex LinkRegex = regex;

		public static string GetCaller([CallerMemberName] string caller = null)
			=> caller;

		public static void SetConfigurationId(string inId) => ConfigurationId = inId;

		public static LogSeverity GetLogLevel()
		{
			var logLevel = GetEnvVar(LogLvlEnvVar);

			if (logLevel != null)
			{
				return (LogSeverity)Enum.Parse(typeof(LogSeverity), logLevel);
			}

			return LogSeverity.Debug;
		}

		public static object GetData(string name) => AppContext.GetData(name);

		public static string GetEnvVar(string envvar, EnvironmentVariableTarget target = EnvironmentVariableTarget.Process) => Environment.GetEnvironmentVariable(envvar, target);

		public static void SetSwitch(string switchName, bool isEnabled) => AppContext.SetSwitch(switchName, isEnabled);

		public static bool TryGetSwitch(string switchName, out bool isEnabled) => AppContext.TryGetSwitch(switchName, out isEnabled);

		public static string StorageDirectory = Path.Combine(BaseDirectory, "storage");

		public static string LogDirectory = Path.Combine(BaseDirectory, "logs");

		public static string FontDirectory = Path.Combine(StorageDirectory, "fonts");

		public static readonly OperatingSystem WindowsVersion = Environment.OSVersion;

		public static readonly KeyValuePair<AssemblyName, GitRepoStruct> Skuld = new(
			Assembly.GetEntryAssembly().GetName(),
			new GitRepoStruct("Skuldbot", "Skuld"));

		//https://medium.com/@jackwild/getting-cpu-usage-in-net-core-7ef825831b8b
		public static async Task<double> GetCurrentCPUUsage()
		{
			var startTime = DateTime.UtcNow;
			var startCpuUsage = Process.GetCurrentProcess().TotalProcessorTime;
			await Task.Delay(500);

			var endTime = DateTime.UtcNow;
			var endCpuUsage = Process.GetCurrentProcess().TotalProcessorTime;
			var cpuUsedMs = (endCpuUsage - startCpuUsage).TotalMilliseconds;
			var totalMsPassed = (endTime - startTime).TotalMilliseconds;
			var cpuUsageTotal = cpuUsedMs / (Environment.ProcessorCount * totalMsPassed);

			return cpuUsageTotal * 100;
		}

		public static IEnumerable<Assembly> GetRequiredAssemblies(Assembly analyzedAssembly)
		{
			return AppDomain.CurrentDomain.GetAssemblies()
				.Where(a => a.GetReferencedAssemblies().ToList().Contains(analyzedAssembly.GetName()));
		}

		//https://stackoverflow.com/a/8850495
		public static IEnumerable<Assembly> GetDependentAssemblies(Assembly analyzedAssembly)
		{
			return AppDomain.CurrentDomain.GetAssemblies()
				.Where(a => GetNamesOfAssembliesReferencedBy(a)
									.Contains(analyzedAssembly.FullName));
		}

		public static IEnumerable<string> GetNamesOfAssembliesReferencedBy(Assembly assembly)
		{
			return assembly.GetReferencedAssemblies()
				.Select(assemblyName => assemblyName.FullName);
		}

		public static class MemoryStats
		{
			public static long GetKBUsage
				=> Process.GetCurrentProcess().WorkingSet64 / 1024;

			public static long GetMBUsage
				=> GetKBUsage / 1024;

			public static long GetGBUsage
				=> GetMBUsage / 1024;
		}
	}

	public struct GitRepoStruct
	{
		public string Owner { get; set; }
		public string Repo { get; set; }

		public GitRepoStruct(string o, string r)
		{
			Owner = o;
			Repo = r;
		}

		public override string ToString()
		{
			return $"https://github.com/{Owner}/{Repo}";
		}
	}
}