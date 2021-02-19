﻿using Discord;
using System;
using System.IO;
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
	}
}