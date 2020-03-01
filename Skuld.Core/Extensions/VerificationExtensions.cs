using System;
using System.Collections.Generic;
using System.Linq;

namespace Skuld.Core.Extensions.Verification
{
    public static class VerificationExtensions
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
            => Uri.TryCreate(input, UriKind.Absolute, out _);

        public static bool IsRecurring(this ulong val, int startLimit)
        {
            var str = Convert.ToString(val);
            var iarr = new List<int>();
            foreach (var ch in str)
            {
                iarr.Add(Convert.ToInt32(Convert.ToString(ch)));
            }

            var same = iarr.All(x => x == iarr[0]);

            return (same && iarr.Count() >= startLimit);
        }

        public static bool IsBitSet(this ulong i, ulong shifted)
            => (i & shifted) != 0;

        public static bool IsSameUpperedInvariant(this string original, string comparison)
            => original.ToUpperInvariant() == comparison.ToUpperInvariant();

        public static bool ContainsUpperedInvariant(this string original, string comparison)
            => original.ToUpperInvariant().Contains(comparison.ToUpperInvariant());
    }
}