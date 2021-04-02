using Discord.Commands;
using Sentry;
using Skuld.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Skuld.Core.Extensions
{
	public static class GenericExtensions
	{
		//https://stackoverflow.com/a/1262619
		/// <summary>
		/// SHuffle a list
		/// </summary>
		/// <typeparam name="T">Collection Item</typeparam>
		/// <param name="list">List to shuffle</param>
		public static void Shuffle<T>(this IList<T> list)
		{
			if (list is null)
			{
				return;
			}

			int n = list.Count;
			while (n > 1)
			{
				byte[] box = new byte[1];
				do SkuldRandom.CryptoProvider.GetBytes(box);
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

		//https://stackoverflow.com/a/489421
		public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
		{
			HashSet<TKey> seenKeys = new();
			foreach (TSource element in source)
			{
				if (!seenKeys.Add(keySelector(element))) continue;

				yield return element;
			}
		}

		/// <summary>
		/// Grab a non cryptographically random element from a collection
		/// </summary>
		/// <typeparam name="T">Anything</typeparam>
		/// <param name="source">Source collection</param>
		/// <returns>Anything within the list</returns>
		public static T Random<T>(this IEnumerable<T> source)
			=> source.ElementAtOrDefault(SkuldRandom.NonCrypto.Next(0, source.Count()));

		/// <summary>
		/// Grab a cryptographically random element from a collection
		/// </summary>
		/// <typeparam name="T">Anything</typeparam>
		/// <param name="source">Source collection</param>
		/// <returns>Anything within the list</returns>
		public static T CryptoRandom<T>(this IEnumerable<T> source)
			=> source.ElementAtOrDefault(SkuldRandom.Next(0, source.Count()));

		#region Safe Mathematics

		public static ulong Add(this ulong source, ulong value)
		{
			try
			{
				checked { source += value; }
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
				checked { source += value; }
			}
			catch
			{
				source = ulong.MaxValue;
			}
			return source;
		}

		public static uint Add(this uint source, uint value)
		{
			try
			{
				checked { source += value; }
			}
			catch
			{
				source = uint.MaxValue;
			}
			return source;
		}

		public static ulong Subtract(this ulong source, ulong value)
		{
			try
			{
				checked { source -= value; }
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
				checked { source -= value; }
			}
			catch
			{
				source = 0;
			}
			return source;
		}

		public static uint Subtract(this uint source, uint value)
		{
			try
			{
				checked { source -= value; }
			}
			catch
			{
				source = 0;
			}
			return source;
		}

		#endregion Safe Mathematics

		/// <summary>
		/// Normalise a number between a start and end point
		/// </summary>
		/// <param name="value">Current Value</param>
		/// <param name="start">Start Point</param>
		/// <param name="end">Endpoint</param>
		/// <returns>Normalised number</returns>
		public static float Normalise(this float value, float start, float end)
			=> (value - start) / (end - start);

		/// <summary>
		/// Normalise a number between a start and end point
		/// </summary>
		/// <param name="value">Current Value</param>
		/// <param name="start">Start Point</param>
		/// <param name="end">Endpoint</param>
		/// <returns>Normalised number</returns>
		public static double Normalise(this double value, double start, double end)
			=> (value - start) / (end - start);

		#region DateTime

		public static int MonthsBetween(this DateTime date1, DateTime date2)
			=> (int)Math.Round(date1.Subtract(date2).Days / (365.25 / 12), MidpointRounding.AwayFromZero);

		public static DateTime FromEpoch(this ulong epoch)
			=> new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(Convert.ToDouble(epoch));

		public static ulong ToEpoch(this DateTime dateTime)
			=> (ulong)dateTime.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

		#endregion DateTime

		public static object Then(this object obj, Action<object> func)
		{
			if (func is null)
			{
				return null;
			}

			func(obj);

			return obj;
		}

		public static object Then<T>(this object obj, Action<T> func)
		{
			if (func is null)
			{
				return null;
			}

			func((T)obj);

			return obj;
		}

		public static object ThenAsync(this object obj, Func<object, Task<object>> func)
		{
			if (func is null)
			{
				return obj;
			}

			return func(obj).GetAwaiter().GetResult();
		}

		public static object ThenAfter(this object obj, Action<object> func, int milliseconds)
		{
			if (func is null)
			{
				return obj;
			}

			Task.Delay(milliseconds);

			func(obj);

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

		public static EventResult<T> Is<T>(this object source)
		{
			if (source is T t)
			{
				return EventResult<T>.FromSuccess<T>(t);
			}

			return EventResult<T>.FromFailure<T>(default, $"{source} is not of type: {typeof(T).Name}");
		}

		public static T As<T>(this object source)
		{
			if (source is null)
			{
				return default;
			}

			TypeConverter converter = new();

			return (T)converter.ConvertTo(source, typeof(T));
		}

		public static SentryEvent ToSentryEvent<T>(this T exception) where T : Exception
		{
			return new SentryEvent
			{
				Message = exception.ToString()
			};
		}

		public static SentryEvent ToSentryEvent([NotNull] this ICommandContext context, Exception exception)
		{
			var sentryEvent = exception.ToSentryEvent();

			sentryEvent.User = new User
			{
				Username = context.Message.Author.FullName(),
				Id = context.Message.Author.Id.ToString(CultureInfo.InvariantCulture)
			};

			sentryEvent.SetTag("guild_id", context.Guild?.Id.ToString(CultureInfo.InvariantCulture) ?? "DMs");
			sentryEvent.SetTag("channel_id", context.Channel.Id.ToString(CultureInfo.InvariantCulture));

			return sentryEvent;
		}
	}
}