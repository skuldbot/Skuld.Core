﻿using System;
using System.Globalization;
using System.Resources;

namespace Skuld.Core.Extensions.Localisation
{
	public static class LocalisationExtensions
	{
		public static string CheckEmptyWithLocale(this int? val, ResourceManager loc)
		{
			if (val.HasValue)
			{
				return Convert.ToString(val, CultureInfo.InvariantCulture);
			}
			return loc.GetString("SKULD_GENERIC_EMPTY", CultureInfo.InvariantCulture);
		}

		public static string CheckEmptyWithLocale(this string[] val, string seperator, ResourceManager loc)
		{
			if (val.Length == 0)
			{
				return loc.GetString("SKULD_GENERIC_EMPTY", CultureInfo.InvariantCulture);
			}
			else
			{
				string msg = "";
				foreach (var item in val)
				{
					var itm = item.CheckEmptyWithLocale(loc);
					if (itm != loc.GetString("SKULD_GENERIC_EMPTY", CultureInfo.InvariantCulture))
					{
						msg += itm + seperator;
					}
				}
				msg = msg.Remove(msg.Length - seperator.Length);
				return msg;
			}
		}

		public static string CheckEmptyWithLocale(this string val, ResourceManager loc)
			=> val ?? loc.GetString("SKULD_GENERIC_EMPTY", CultureInfo.InvariantCulture);
	}
}