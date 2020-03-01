using System.Collections.Generic;
using System.Linq;

namespace Skuld.Core.Extensions.Pagination
{
    public static class PaginationExtensions
    {
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
    }
}