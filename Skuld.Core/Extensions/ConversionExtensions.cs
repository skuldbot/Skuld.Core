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
            switch (data.ToLowerInvariant())
            {
                case "true":
                case "1":
                case "y":
                    return true;

                case "false":
                case "0":
                case "n":
                    return false;

                default:
                    throw new Exception("Cannot Convert from \"" + data + "\" to Boolean");
            }
        }

        public static MemoryStream ToMemoryStream(this string value)
            => new MemoryStream(Encoding.UTF8.GetBytes(value ?? ""));

        public static Uri ToUri(this string value)
            => new Uri(value);

        public static ConsoleColor SeverityToColor(this LogSeverity sev)
        {
            switch (sev)
            {
                case LogSeverity.Critical:
                    return ConsoleColor.Red;
                case LogSeverity.Error:
                    return ConsoleColor.Red;
                case LogSeverity.Info:
                    return ConsoleColor.Green;
                case LogSeverity.Warning:
                    return ConsoleColor.Yellow;
                case LogSeverity.Verbose:
                    return ConsoleColor.Cyan;

                default:
                    return ConsoleColor.White;
            }
        }

        public static double Remap(this double value, double min1, double max1, double min2, double max2)
            => min2 + (max2 - min2) * ((value - min1) / (max1 - min1));
    }
}