using System.Collections.Generic;

#nullable enable
namespace LoyaltyPrime.DataAccessLayer.Shared.Utilities.Common.Data
{
    public class ResultModel<TOutput>
    {
        /// <summary>
        /// Success Result Constructor
        /// </summary>
        /// <param name="statusCode">statusCode</param>
        /// <param name="message">Success Message</param>
        /// <param name="result">Result Object(Optional)</param>
        private ResultModel(int statusCode, string message, TOutput result = default)
        {
            StatusCode = statusCode;
            Message = message;
            IsSucceeded = true;
            Result = result;
            Error = null;
        }

        /// <summary>
        /// Fail Result Constructor
        /// </summary>
        /// <param name="statusCode">statusCode</param>
        /// <param name="message">Error Message</param>
        /// <param name="errorType">Error Type(Optional)</param>
        /// <param name="info">Error Info(Optional)</param>
        private ResultModel(int statusCode, string message, string errorType = ErrorTypes.InternalSystemError,
            IDictionary<string, string> info = null!)
        {
            StatusCode = statusCode;
            Message = message;
            IsSucceeded = false;
            Result = default(TOutput);
            Error = new Error(errorType, info);
        }

        public int StatusCode { get; private set; }
        public string Message { get; private set; }
        public bool IsSucceeded { get; private set; }
        public TOutput? Result { get; private set; }
        public Error? Error { get; private set; }

        /// <summary>
        /// Creates Success Result
        /// </summary>
        /// <param name="statusCode">statusCode</param>
        /// <param name="message">Success Message</param>
        /// <param name="result">Result Object(Optional)</param>
        /// <returns>ResultModel</returns>
        public static ResultModel<TOutput> Success(int statusCode, string message = "", TOutput result = default)
        {
            return new ResultModel<TOutput>(statusCode, message, result);
        }

        /// <summary>
        /// Creates Failed Result
        /// </summary>
        /// <param name="statusCode">statusCode</param>
        /// <param name="message">Error Message</param>
        /// <param name="errorType">Error Type</param>
        /// <param name="info">Error Info(Optional)</param>
        /// <returns>ResultModel</returns>
        public static ResultModel<TOutput> Fail(int statusCode, string message,
            string errorType = ErrorTypes.InternalSystemError, IDictionary<string, string> info = null!)
        {
            return new ResultModel<TOutput>(statusCode, message, errorType, info);
        }

        /// <summary>
        /// Not Found Failed Result
        /// </summary>
        /// <param name="targetName">name of target</param>
        /// <param name="info">Error Info(Optional)</param>
        /// <returns>ResultModel</returns>
        public static ResultModel<TOutput> NotFound(string targetName, IDictionary<string, string> info = null!)
        {
            return new ResultModel<TOutput>(404, $"Requested {targetName} not found", ErrorTypes.NotFound, info);
        }
    }
}