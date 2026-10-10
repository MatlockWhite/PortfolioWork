using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Contracts
{
    public class ValidationError
    {
        /// <summary>
        /// The Area/Field where the associated ValidationError occurred.
        /// </summary>
        public string Field { get; init; } = string.Empty;

        /// <summary>
        /// The Error Message associated with the given ValidationError.
        /// </summary>
        public string Message { get; init; } = string.Empty;
    }
}


/*
 * Examples of how this would/should look:
 *      JSON
 *      {
 *          "field": "username",
 *          "message": "Username is required."
 *      }
 *      
 *      {
 *          "field": "password",
 *          "message": "Password must be at least 8 characters long."
 *      }
 */