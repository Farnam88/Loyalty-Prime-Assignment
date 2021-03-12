#nullable enable
using System.Collections.Generic;

namespace LoyaltyPrime.Shared.Utilities.Common.Data
{
    public class Error
    {
        public Error(string errorType, IDictionary<string, string> info = null!)
        {
            ErrorType = errorType;
            Info = info;
        }

        public IDictionary<string, string>? Info { get; private set; }

        public string ErrorType { get; set; }
    }
}