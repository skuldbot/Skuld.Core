using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;

namespace Skuld.Core.Extensions
{
    public static class GenericExtensions
    {
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