using System;
using System.Security.Cryptography;

namespace Skuld.Core
{
	public class SkuldRandom : RandomNumberGenerator
	{
		public static readonly Random NonCrypto = new();
		public static readonly RandomNumberGenerator CryptoProvider = new RNGCryptoServiceProvider();

		public static int Next()
		{
			var data = new byte[sizeof(int)];

			CryptoProvider.GetBytes(data);

			return BitConverter.ToInt32(data, 0) & (int.MaxValue - 1);
		}

		public static int Next(int maxValue)
		{
			return Next(0, maxValue);
		}

		public static int Next(int minValue, int maxValue)
		{
			if (minValue > maxValue)
			{
				throw new ArgumentOutOfRangeException(nameof(minValue), maxValue, $"{nameof(minValue)} cannot be larger than {nameof(maxValue)}");
			}

			return (int)Math.Floor((minValue + ((double)maxValue - minValue) * NextDouble()));
		}

		public static long Next(long maxValue)
		{
			return Next(0L, maxValue);
		}

		public static long Next(long minValue, long maxValue)
		{
			if (minValue > maxValue)
			{
				throw new ArgumentOutOfRangeException(nameof(minValue), maxValue, $"{nameof(minValue)} cannot be larger than {nameof(maxValue)}");
			}
			return (long)Math.Floor((minValue + ((double)maxValue - minValue) * NextDouble()));
		}

		public static ulong Next(ulong maxValue)
		{
			return Next(0, maxValue);
		}

		public static ulong Next(ulong minValue, ulong maxValue)
		{
			if (minValue > maxValue)
			{
				throw new ArgumentOutOfRangeException(nameof(minValue), maxValue, $"{nameof(minValue)} cannot be larger than {nameof(maxValue)}");
			}
			return (ulong)Math.Floor((minValue + ((double)maxValue - minValue) * NextDouble()));
		}

		public static double NextDouble()
		{
			var data = new byte[sizeof(uint)];
			CryptoProvider.GetBytes(data);
			var randUint = BitConverter.ToUInt32(data, 0);
			return randUint / (uint.MaxValue + 1.0);
		}

		public override void GetBytes(byte[] data)
		{
			CryptoProvider.GetBytes(data);
		}

		public override void GetNonZeroBytes(byte[] data)
		{
			CryptoProvider.GetNonZeroBytes(data);
		}
	}
}