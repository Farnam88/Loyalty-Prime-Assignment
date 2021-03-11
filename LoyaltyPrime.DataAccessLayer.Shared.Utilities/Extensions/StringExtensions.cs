namespace LoyaltyPrime.DataAccessLayer.Shared.Utilities.Extensions
{
    public static class StringExtensions
    {
        public static bool ContainsString(this string str)
        {
            return str is not null && !string.IsNullOrWhiteSpace(str);
        }

        public static bool HasString(string str)
        {
            return str is not null && !string.IsNullOrWhiteSpace(str);
        }
    }
}