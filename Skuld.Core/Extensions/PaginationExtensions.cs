using System.Collections.Generic;

namespace Skuld.Core.Extensions.Pagination
{
	public static class PaginationExtensions
	{
		public static IList<string> PaginateList(this string[] list, int maxrows = 10)
		{
			var pages = new List<string>();
			string pagetext = "";

			for (int x = 0; x < list.Length; x++)
			{
				pagetext += $"{list[x]}\n";

				if ((x + 1) % maxrows == 0 || (x + 1) == list.Length)
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

			for (int x = 0; x < list.Length; x++)
			{
				pagetext += $"{list[x]}\n";

				if ((x + 1) % maxrows == 0 || (x + 1) == list.Length)
				{
					pagetext += "```";
					pages.Add(pagetext);
					pagetext = "```cs\n";
				}
			}

			return pages;
		}
	}
}