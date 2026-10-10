using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Contracts
{
    public class ApiError
    {
        /// <summary>
        /// The Numerical value of Status Code associated with the given ApiError.
        /// Can be Null or Empty if no Status Code is provided.
        /// </summary>
        public int? StatusCode { get; init; }

        /// <summary>
        /// The String representation of the Error Code associated with the given ApiError.
        /// Can be Null or Empty if no Error Code is provided.
        /// </summary>
        public string? ErrorCode { get; init; } = string.Empty;

        /// <summary>
        /// The Error Message associated with the given ApiError.
        /// Can be Null or Empty if no Error Message is provided.
        /// </summary>
        public string? ErrorMessage { get; init; } = string.Empty;

        /// <summary>
        /// The Validation Errors associated with the given ApiError.
        /// Can be Null or Empty if no Validation Errors are provided.
        /// </summary>
        public IReadOnlyCollection<ValidationError>? ValidationErrors { get; init; }
    }
}


/*
 * Examples of how this would/should look:
 *      JSON
 *      {
 *          "HTTPStatusCode": 404,
 *          "ErrorCode": "NOT_FOUND",
 *          "ErrorMessage": "The requested resource was not found.",
 *          "ValidationErrors": [
 *              {
 *                  "field": "username",
 *                  "message": "Username not Found."
 *              },
 *              {
 *                  "field": "database",
 *                  "message": "No Database Connection Found."
 *              }
 *          ]
 *      }
 */