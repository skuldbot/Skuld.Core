using System;

namespace Skuld.Core.Utilities
{
    public static class ConversionTools
    {
        public static ulong GetEpochMs()
            => (ulong)new DateTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
    }
}