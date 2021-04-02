using Discord;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Skuld.Core.Extensions.Conversion
{
	public static class ConversionExtensions
	{
		public static bool ToBool(this string data)
			=> (data.ToLowerInvariant()) switch
			{
				"true" or "1" or "y" => true,
				"false" or "0" or "n" => false,
				_ => throw new Exception("Cannot Convert from \"" + data + "\" to Boolean"),
			};

		public static string CreateMD5(this string input, bool lowered = false)
		{
			using (MD5 md5 = MD5.Create())
			{
				byte[] inputBytes = Encoding.ASCII.GetBytes(input);
				byte[] hashBytes = md5.ComputeHash(inputBytes);

				// Convert the byte array to hexadecimal string
				StringBuilder sb = new();
				for (int i = 0; i < hashBytes.Length; i++)
				{
					sb.Append(hashBytes[i].ToString("X2"));
				}

				string result = sb.ToString();

				if (lowered)
				{
					result = result.ToLowerInvariant();
				}

				return result;
			}
		}

		public static MemoryStream ToMemoryStream(this string value)
			=> new(Encoding.UTF8.GetBytes(value ?? ""));

		public static Uri ToUri(this string value)
			=> new(value);

		public static ConsoleColor SeverityToColor(this LogSeverity sev)
			=> sev switch
			{
				LogSeverity.Critical => ConsoleColor.Red,
				LogSeverity.Error => ConsoleColor.Red,
				LogSeverity.Info => ConsoleColor.Green,
				LogSeverity.Warning => ConsoleColor.Yellow,
				LogSeverity.Verbose => ConsoleColor.Cyan,
				_ => ConsoleColor.White,
			};

		public static double Remap(this double value, double min1, double max1, double min2, double max2)
			=> min2 + (max2 - min2) * ((value - min1) / (max1 - min1));

		public static Color Lerp(this Color a, Color b, double t)
			=> a.Lerp(b, (float)t);

		public static Color Lerp(this Color a, Color b, float t)
			=> new(
					Math.Clamp(a.R + (b.R - a.R) * t, 0, 1),
					Math.Clamp(a.G + (b.G - a.G) * t, 0, 1),
					Math.Clamp(a.B + (b.B - a.B) * t, 0, 1)
				);

		public static Stream ToStream(this string message)
		{
			var stream = new MemoryStream();
			var sw = new StreamWriter(stream);
			sw.Write(message);
			sw.Flush();
			stream.Position = 0;

			return stream;
		}
	}
}