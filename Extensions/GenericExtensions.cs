using Discord;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Resources;
using System.Security.Cryptography;
using System.Text;

namespace Skuld.Core.Extensions
{
    public static class GenericExtensions
    {
        private static readonly string[] VideoExtensions = {
            ".webm",
            ".mkv",
            ".flv",
            ".vob",
            ".ogv",
            ".ogg",
            ".avi",
            ".mov",
            ".qt",
            ".wmv",
            ".mp4",
            ".m4v",
            ".mpg",
            ".mpeg"
        };

        private static readonly string[] ImageExtensions =
        {
            ".jpg",
            ".bmp",
            ".gif",
            ".png",
            ".apng"
        };

        //https://stackoverflow.com/a/1262619
        public static void Shuffle<T>(this IList<T> list)
        {
            using RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            int n = list.Count;
            while (n > 1)
            {
                byte[] box = new byte[1];
                do provider.GetBytes(box);
                while (!(box[0] < n * (Byte.MaxValue / n)));
                int k = (box[0] % n);
                n--;
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static T RandomValue<T>(this IEnumerable<T> entries, Random random = null) where T : class
        {
            if (random == null)
                random = new Random((int)Math.Clamp(Process.GetCurrentProcess().StartTime.ToEpoch(), 0, int.MaxValue));

            var list = entries.ToList();

            var index = random.Next(0, list.Count);

            return list[index];
        }

        #region Conversion

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

        #endregion Conversion

        #region Verification

        public static bool IsImageExtension(this string input)
        {
            foreach (var ext in ImageExtensions)
            {
                if (input.Contains(ext))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsVideoFile(this string input)
        {
            foreach (var x in VideoExtensions)
            {
                if (input.Contains(x) || input.EndsWith(x))
                    return true;
            }
            return false;
        }

        public static bool IsWebsite(this string input)
            => Uri.TryCreate(input, UriKind.RelativeOrAbsolute, out _);

        public static bool IsRecurring(this ulong val, int startLimit)
        {
            var str = Convert.ToString(val);
            var iarr = new List<int>();
            foreach (var ch in str)
            {
                iarr.Add(Convert.ToInt32(Convert.ToString(ch)));
            }

            var same = iarr.All(x => x == iarr[0]);

            return (same && iarr.Count() > startLimit);
        }

        public static bool IsBitSet(this ulong i, ulong shifted)
            => (i & shifted) != 0;

        #endregion Verification

        #region Localisation

        public static string CheckEmptyWithLocale(this int? val, ResourceManager loc)
        {
            if (val.HasValue)
            {
                return Convert.ToString(val);
            }
            return loc.GetString("SKULD_GENERIC_EMPTY");
        }

        public static string CheckEmptyWithLocale(this string[] val, string seperator, ResourceManager loc)
        {
            if (val.Length == 0)
            {
                return loc.GetString("SKULD_GENERIC_EMPTY");
            }
            else
            {
                string msg = "";
                foreach (var item in val)
                {
                    var itm = item.CheckEmptyWithLocale(loc);
                    if (itm != loc.GetString("SKULD_GENERIC_EMPTY"))
                    {
                        msg += itm + seperator;
                    }
                }
                msg = msg.Remove(msg.Length - seperator.Length);
                return msg;
            }
        }

        public static string CheckEmptyWithLocale(this string val, ResourceManager loc)
            => val ?? loc.GetString("SKULD_GENERIC_EMPTY");

        #endregion Localisation

        #region Pagination

        public static IList<string> PaginateList(this string[] list, int maxrows = 10)
        {
            var pages = new List<string>();
            string pagetext = "";

            for (int x = 0; x < list.Count(); x++)
            {
                pagetext += $"{list[x]}\n";

                if ((x + 1) % maxrows == 0 || (x + 1) == list.Count())
                {
                    pages.Add(pagetext);
                    pagetext = "";
                }
            }

            return pages;
        }

        public static IList<string> PaginateCodeBlockList(this string[] list, int maxrows = 10)
        {
            var pages = new List<string>();
            string pagetext = "```cs\n";

            for (int x = 0; x < list.Count(); x++)
            {
                pagetext += $"{list[x]}\n";

                if ((x + 1) % maxrows == 0 || (x + 1) == list.Count())
                {
                    pagetext += "```";
                    pages.Add(pagetext);
                    pagetext = "```cs\n";
                }
            }

            return pages;
        }

        #endregion Pagination

        #region Formatting

        public static string CapitaliseFirstLetter(this string input)
            => input switch
            {
                null => throw new ArgumentNullException(nameof(input)),
                "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
                _ => input.First().ToString().ToUpperInvariant() + input.Substring(1)
            };

        public static string LowercaseFirstLetter(this string input)
            => input switch
            {
                null => throw new ArgumentNullException(nameof(input)),
                "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
                _ => input.First().ToString().ToLowerInvariant() + input.Substring(1)
            };

        public static string GetStringfromOffset(this DateTimeOffset dateTimeOffset, DateTime dateTime)
        {
            var offset = dateTime - dateTimeOffset;

            var builder = new StringBuilder();

            builder.Append((int)Math.Floor(offset.TotalDays));
            builder.Append(" days ago");

            return builder.ToString();
        }

        public static string ToDMYString(this DateTime dateTime)
            => dateTime.ToString("dd'/'MM'/'yyyy HH:mm:ss");

        public static string ToDMYString(this DateTimeOffset dateTime)
            => dateTime.ToString("dd'/'MM'/'yyyy HH:mm:ss");

        //https://gist.github.com/starquake/8d72f1e55c0176d8240ed336f92116e3
        public static string StripHtml(this string value)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(value);

            if (htmlDoc == null)
                return value;

            return htmlDoc.DocumentNode.InnerText;
        }

        public static string PrettyLines(this List<string[]> lines, int padding = 1)
        {
            int elementCount = lines[0].Length;
            int[] maxValues = new int[elementCount];

            for (int i = 0; i < elementCount; i++)
                maxValues[i] = lines.Max(x => x[i].Length) + padding;

            var sb = new StringBuilder();
            bool isFirst = true;

            foreach (var line in lines)
            {
                if (!isFirst)
                    sb.AppendLine();

                isFirst = false;

                for (int i = 0; i < line.Length; i++)
                {
                    var value = line[i];
                    sb.Append(value.PadRight(maxValues[i]));
                }
            }
            return Convert.ToString(sb);
        }

        #endregion Formatting

        #region DateTime

        public static int MonthsBetween(this DateTime date1, DateTime date2)
            => (int)Math.Round(date1.Subtract(date2).Days / (365.25 / 12), MidpointRounding.AwayFromZero);

        public static DateTime FromEpoch(this ulong epoch)
            => new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(Convert.ToDouble(epoch));

        public static ulong ToEpoch(this DateTime dateTime)
            => (ulong)(dateTime.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

        #endregion DateTime
    }
}