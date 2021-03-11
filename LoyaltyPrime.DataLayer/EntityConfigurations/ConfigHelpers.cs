using System;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace LoyaltyPrime.DataLayer.EntityConfigurations
{
    public class ConfigHelpers
    {
        public static ValueConverter<string, string> ConfigConverterTrimUpper()
        {
            var converter = new ValueConverter<string, string>(
                v => v,
                v => v.Trim().ToUpper());
            return converter;
        }

        public static ValueConverter<string, string> ConfigConverterTrim()
        {
            var converter = new ValueConverter<string, string>(
                v => v,
                v => v.Trim());
            return converter;
        }

        public static ValueComparer<string> ConfigComParer()
        {
            var comparer = new ValueComparer<string>(
                (l, r) => string.Equals(l, r, StringComparison.OrdinalIgnoreCase),
                v => v.Trim().ToUpper().GetHashCode(),
                v => v);
            return comparer;
        }
    }
}