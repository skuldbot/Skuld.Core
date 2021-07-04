using Skuld.Core.Extensions.Verification;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Skuld.Core.Helpers
{
	public static class StringTimeHelper
	{
		public static TimeSpan? Parse(string input)
		{
			List<string> inputSplit = input.Split(" ").ToList();

			int d = 0, h = 0, m = 0, s = 0, ms = 0;

			if (inputSplit.Any(x => x.ContainsUpperedInvariant("days")))
			{
				var key = inputSplit.FirstOrDefault(x => x.ContainsUpperedInvariant("days"));

				if (key.IsSameUpperedInvariant("days"))
				{
					d = Convert.ToInt32(inputSplit[inputSplit.IndexOf(key) - 1], CultureInfo.InvariantCulture);
				}
				else
				{
					d = Convert.ToInt32(key.Replace("days", "").Replace("DAYS", ""), CultureInfo.InvariantCulture);
				}
			}
			else if (inputSplit.Any(x => x.ContainsUpperedInvariant("day")))
			{
				var key = inputSplit.FirstOrDefault(x => x.ContainsUpperedInvariant("day"));

				if (key.IsSameUpperedInvariant("day"))
				{
					d = Convert.ToInt32(inputSplit[inputSplit.IndexOf(key) - 1], CultureInfo.InvariantCulture);
				}
				else
				{
					d = Convert.ToInt32(key.Replace("day", "").Replace("DAY", ""), CultureInfo.InvariantCulture);
				}
			}
			else if (inputSplit.Any(x => x.ContainsUpperedInvariant("d")))
			{
				d = Convert.ToInt32(inputSplit.FirstOrDefault(x => x.ContainsUpperedInvariant("d")).Replace("d", "").Replace("D", ""), CultureInfo.InvariantCulture);
			}

			if (inputSplit.Any(x => x.ContainsUpperedInvariant("hours")))
			{
				var key = inputSplit.FirstOrDefault(x => x.ContainsUpperedInvariant("hours"));

				if (key.IsSameUpperedInvariant("hours"))
				{
					h = Convert.ToInt32(inputSplit[inputSplit.IndexOf(key) - 1], CultureInfo.InvariantCulture);
				}
				else
				{
					h = Convert.ToInt32(key.Replace("hours", "").Replace("HOURS", ""), CultureInfo.InvariantCulture);
				}
			}
			else if (inputSplit.Any(x => x.ContainsUpperedInvariant("hour")))
			{
				var key = inputSplit.FirstOrDefault(x => x.ContainsUpperedInvariant("hour"));

				if (key.IsSameUpperedInvariant("hour"))
				{
					h = Convert.ToInt32(inputSplit[inputSplit.IndexOf(key) - 1], CultureInfo.InvariantCulture);
				}
				else
				{
					h = Convert.ToInt32(key.Replace("hour", "").Replace("HOUR", ""), CultureInfo.InvariantCulture);
				}
			}
			else if (inputSplit.Any(x => x.ContainsUpperedInvariant("h")))
			{
				h = Convert.ToInt32(inputSplit.FirstOrDefault(x => x.ContainsUpperedInvariant("h")).Replace("h", "").Replace("H", ""), CultureInfo.InvariantCulture);
			}

			if (inputSplit.Any(x => x.ContainsUpperedInvariant("minutes")))
			{
				var key = inputSplit.FirstOrDefault(x => x.ContainsUpperedInvariant("minutes"));

				if (key.IsSameUpperedInvariant("minutes"))
				{
					m = Convert.ToInt32(inputSplit[inputSplit.IndexOf(key) - 1], CultureInfo.InvariantCulture);
				}
				else
				{
					m = Convert.ToInt32(key.Replace("minutes", "").Replace("MINUTES", ""), CultureInfo.InvariantCulture);
				}
			}
			else if (inputSplit.Any(x => x.ContainsUpperedInvariant("minute")))
			{
				var key = inputSplit.FirstOrDefault(x => x.ContainsUpperedInvariant("minute"));

				if (key.IsSameUpperedInvariant("minute"))
				{
					m = Convert.ToInt32(inputSplit[inputSplit.IndexOf(key) - 1], CultureInfo.InvariantCulture);
				}
				else
				{
					m = Convert.ToInt32(key.Replace("minute", "").Replace("MINUTE", ""), CultureInfo.InvariantCulture);
				}
			}
			else if (inputSplit.Any(x => x.ContainsUpperedInvariant("m") && !x.ContainsUpperedInvariant("ms")))
			{
				m = Convert.ToInt32(inputSplit.FirstOrDefault(x => x.ContainsUpperedInvariant("m")).Replace("m", "").Replace("M", ""), CultureInfo.InvariantCulture);
			}

			if (inputSplit.Any(x => x.ContainsUpperedInvariant("seconds")))
			{
				var key = inputSplit.FirstOrDefault(x => x.ContainsUpperedInvariant("seconds"));

				if (key.IsSameUpperedInvariant("seconds"))
				{
					s = Convert.ToInt32(inputSplit[inputSplit.IndexOf(key) - 1], CultureInfo.InvariantCulture);
				}
				else
				{
					s = Convert.ToInt32(key.Replace("seconds", "").Replace("SECONDS", ""), CultureInfo.InvariantCulture);
				}
			}
			else if (inputSplit.Any(x => x.ContainsUpperedInvariant("second")))
			{
				var key = inputSplit.FirstOrDefault(x => x.ContainsUpperedInvariant("second"));

				if (key.IsSameUpperedInvariant("second"))
				{
					s = Convert.ToInt32(inputSplit[inputSplit.IndexOf(key) - 1], CultureInfo.InvariantCulture);
				}
				else
				{
					s = Convert.ToInt32(key.Replace("second", "").Replace("SECOND", ""), CultureInfo.InvariantCulture);
				}
			}
			else if (inputSplit.Any(x => x.ContainsUpperedInvariant("s") && !x.ContainsUpperedInvariant("ms")))
			{
				s = Convert.ToInt32(inputSplit.FirstOrDefault(x => x.ContainsUpperedInvariant("s")).Replace("s", "").Replace("S", ""), CultureInfo.InvariantCulture);
			}

			if (inputSplit.Any(x => x.ContainsUpperedInvariant("milliseconds")))
			{
				var key = inputSplit.FirstOrDefault(x => x.ContainsUpperedInvariant("milliseconds"));

				if (key.IsSameUpperedInvariant("milliseconds"))
				{
					ms = Convert.ToInt32(inputSplit[inputSplit.IndexOf(key) - 1], CultureInfo.InvariantCulture);
				}
				else
				{
					ms = Convert.ToInt32(key.Replace("milliseconds", "").Replace("MILLISECONDS", ""), CultureInfo.InvariantCulture);
				}
			}
			else if (inputSplit.Any(x => x.ContainsUpperedInvariant("millisecond")))
			{
				var key = inputSplit.FirstOrDefault(x => x.ContainsUpperedInvariant("millisecond"));

				if (key.IsSameUpperedInvariant("millisecond"))
				{
					ms = Convert.ToInt32(inputSplit[inputSplit.IndexOf(key) - 1], CultureInfo.InvariantCulture);
				}
				else
				{
					ms = Convert.ToInt32(key.Replace("millisecond", "").Replace("MILLISECOND", ""), CultureInfo.InvariantCulture);
				}
			}
			else if (inputSplit.Any(x => x.ContainsUpperedInvariant("ms")))
			{
				ms = Convert.ToInt32(inputSplit.FirstOrDefault(x => x.ContainsUpperedInvariant("ms")).Replace("ms", "").Replace("MS", ""), CultureInfo.InvariantCulture);
			}

			if (d != 0 || h != 0 || m != 0 || s != 0 || ms != 0)
			{
				return new TimeSpan(d, h, m, s, ms);
			}

			return null;
		}

		public static bool TryParse(string input, out TimeSpan? output)
		{
			output = Parse(input);

			return output.HasValue;
		}


	}
}
