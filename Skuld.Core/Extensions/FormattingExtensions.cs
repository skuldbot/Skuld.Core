using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Globalization;
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
				_ => input.First().ToString().ToUpperInvariant() + input[1..]
			};

		public static string LowercaseFirstLetter(this string input)
			=> input switch
			{
				null => throw new ArgumentNullException(nameof(input)),
				"" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
				_ => input.First().ToString().ToLowerInvariant() + input[1..]
			};

		public static string GetStringFromOffset(this DateTimeOffset dateTimeOffset, DateTime dateTime)
		{
			var offset = dateTime - dateTimeOffset;

			StringBuilder builder = new();

			int days = (int)Math.Floor(offset.TotalDays);

			int years = days / 365;

			if (years > 0)
			{
				builder.Append(years.ToFormattedString());

				if (years > 2)
				{
					builder.Append(" years");
				}
				else
				{
					builder.Append(" year");
				}

				builder.Append(" and ");

				days -= years * 365;
			}

			if (days > 0)
			{
				builder.Append(days.ToFormattedString());
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
			=> dateTime.ToString("dd'/'MM'/'yyyy HH:mm:ss", CultureInfo.InvariantCulture);

		public static string ToDMYString(this DateTimeOffset dateTime)
			=> dateTime.ToString("dd'/'MM'/'yyyy HH:mm:ss", CultureInfo.InvariantCulture);

		public static string ToDifferenceString(this TimeSpan difference)
		{
			StringBuilder message = new();

			if (difference.Days > 0)
			{
				message.Append($"{difference.Days} day{(difference.Days > 1 ? "s" : "")} ");
			}

			if (difference.Hours > 0)
			{
				message.Append($"{difference.Hours} hour{(difference.Hours > 1 ? "s" : "")} ");
			}

			if (difference.Minutes > 0)
			{
				message.Append($"{difference.Minutes} minute{(difference.Minutes > 1 ? "s" : "")} ");
			}

			if (difference.Seconds > 0)
			{
				message.Append($"{difference.Seconds} second{(difference.Seconds > 1 ? "s" : "")} ");
			}

			return message.ToString()[0..^1];
		}

		//https://gist.github.com/starquake/8d72f1e55c0176d8240ed336f92116e3
		public static string StripHtml(this string value)
		{
			HtmlDocument htmlDoc = new();
			htmlDoc.LoadHtml(value);

			if (htmlDoc is null)
				return value;

			return htmlDoc.DocumentNode.InnerText;
		}


		//http://dev.flauschig.ch/wordpress/?p=387
		public static string PrettyLines(this List<string[]> lines, int padding = 1)
		{
			int elementCount = lines[0].Length;
			int[] maxValues = new int[elementCount];

			for (int i = 0; i < elementCount; i++)
				maxValues[i] = lines.Max(x => x[i].Length) + padding;

			StringBuilder sb = new();
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
			return Convert.ToString(sb, CultureInfo.InvariantCulture);
		}
	}
}