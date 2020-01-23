using Discord;
using System;
using System.IO;
using System.Text;

namespace Skuld.Core.Extensions.Conversion
{
    public static class ConversionExtensions
    {
        public static bool ToBool(this string data)
        {
            if (data.ToLowerInvariant() == "true")
                return true;
            if (data.ToLowerInvariant() == "false")
                return false;
            if (data == "1")
                return true;
            if (data == "0")
                return false;

            throw new Exception("Cannot Convert from \"" + data + "\" to Boolean");
        }

        public static MemoryStream ToMemoryStream(this string value)
            => new MemoryStream(Encoding.UTF8.GetBytes(value ?? ""));

        public static Uri ToUri(this string value)
            => new Uri(value);

        public static ConsoleColor SeverityToColor(this LogSeverity sev)
        {
            if (sev == LogSeverity.Critical)
                return ConsoleColor.Red;
            if (sev == LogSeverity.Error)
                return ConsoleColor.Red;
            if (sev == LogSeverity.Info)
                return ConsoleColor.Green;
            if (sev == LogSeverity.Warning)
                return ConsoleColor.Yellow;
            if (sev == LogSeverity.Verbose)
                return ConsoleColor.Cyan;
            return ConsoleColor.White;
        }

        public static double Remap(this double value, double min1, double max1, double min2, double max2)
            => min2 + (max2 - min2) * ((value - min1) / (max1 - min1));

    }
}
