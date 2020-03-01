using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Skuld.Core.Extensions.Formatting
{
    public static class FormattingExtensions
    {
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

            var days = (int)Math.Floor(offset.TotalDays);

            if(days > 0)
            {
                builder.Append(days);
            }

            if (days > 1)
            {
                builder.Append(" days ago");
            }
            else if (days == 1)
            {
                builder.Append(" day ago");
            }
            else
            {
                builder.Append("today");
            }

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
    }
}