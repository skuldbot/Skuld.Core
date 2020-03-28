using Skuld.Core.Extensions.Verification;
using System;
using System.Linq;

namespace Skuld.Core.Helpers
{
    public static class StringTimeHelper
    {
        public static TimeSpan? Parse(string input)
        {
            string[] inputSplit = inputSplit = input.Split(" ");

            int d = 0, h = 0, m = 0, s = 0;

            if (inputSplit.Any(x => x.ContainsUpperedInvariant("d")))
            {
                d = Convert.ToInt32(inputSplit.FirstOrDefault(x => x.ContainsUpperedInvariant("d")).Replace("d", "").Replace("D", ""));
            }

            if(inputSplit.Any(x => x.ContainsUpperedInvariant("h")))
            {
                h = Convert.ToInt32(inputSplit.FirstOrDefault(x => x.ContainsUpperedInvariant("h")).Replace("h", "").Replace("H", ""));
            }

            if (inputSplit.Any(x => x.ContainsUpperedInvariant("m")))
            {
                m = Convert.ToInt32(inputSplit.FirstOrDefault(x => x.ContainsUpperedInvariant("m")).Replace("m", "").Replace("M", ""));
            }

            if (inputSplit.Any(x => x.ContainsUpperedInvariant("s")))
            {
                s = Convert.ToInt32(inputSplit.FirstOrDefault(x => x.ContainsUpperedInvariant("s")).Replace("s", "").Replace("S", ""));
            }

            if (d != 0 || h != 0 || m != 0 || s != 0)
            {
                return new TimeSpan(d, h, m, s);
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
