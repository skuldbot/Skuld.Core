using Skuld.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

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
                while (!(box[0] < n * (byte.MaxValue / n)));
                int k = (box[0] % n);
                n--;
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static double NthRoot(this double value, int nth)
            => Math.Pow(value, 1.0 / nth);

        public static T RandomValue<T>(this IEnumerable<T> entries) where T : class
        {
            var list = entries.ToList();

            var index = SkuldRandom.Next(0, list.Count);

            return list[index];
        }

        //https://stackoverflow.com/a/489421
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        public static ulong Add(this ulong source, ulong value)
        {
            try
            {
                checked
                {
                    source += value;
                }
            }
            catch
            {
                source = ulong.MaxValue;
            }
            return source;
        }

        public static ulong Add(this ulong source, uint value)
        {
            try
            {
                checked
                {
                    source += value;
                }
            }
            catch
            {
                source = ulong.MaxValue;
            }
            return source;
        }

        public static ulong Subtract(this ulong source, ulong value)
        {
            try
            {
                checked
                {
                    source -= value;
                }
            }
            catch
            {
                source = 0;
            }
            return source;
        }

        public static ulong Subtract(this ulong source, uint value)
        {
            try
            {
                checked
                {
                    source -= value;
                }
            }
            catch
            {
                source = 0;
            }
            return source;
        }

        public static uint Add(this uint source, uint value)
        {
            try
            {
                checked
                {
                    source += value;
                }
            }
            catch
            {
                source = uint.MaxValue;
            }
            return source;
        }

        public static uint Subtract(this uint source, uint value)
        {
            try
            {
                checked
                {
                    source -= value;
                }
            }
            catch
            {
                source = 0;
            }
            return source;
        }

        #region DateTime

        public static int MonthsBetween(this DateTime date1, DateTime date2)
            => (int)Math.Round(date1.Subtract(date2).Days / (365.25 / 12), MidpointRounding.AwayFromZero);

        public static DateTime FromEpoch(this ulong epoch)
            => new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(Convert.ToDouble(epoch));

        public static ulong ToEpoch(this DateTime dateTime)
            => (ulong)(dateTime.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

        #endregion DateTime

        public static object Then(this object obj, Action<object> func)
        {
            func.Invoke(obj);

            return obj;
        }

        public static object ThenAsync(this object obj, Func<object, Task<object>> func)
            => func.Invoke(obj).GetAwaiter().GetResult();

        public static object ThenAfter(this object obj, Action<object> func, int milliseconds)
        {
            Task.Delay(milliseconds);

            func.Invoke(obj);

            return obj;
        }

        public static object ThenAfterAsync(this object obj, Func<object, Task<object>> func, int milliseconds)
        {
            Task.Run(async () =>
            {
                await Task.Delay(milliseconds).ConfigureAwait(false);

                obj = await func.Invoke(obj).ConfigureAwait(false);
            });

            return obj;
        }

    }
}