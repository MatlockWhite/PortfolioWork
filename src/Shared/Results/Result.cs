using Shared.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Results
{
    public class Result
    {
        /// <summary>
        /// The Value indicating if the operation was successful or not. This property is true when the operation is successful, and false when there is an error.
        /// </summary>
        public bool IsSuccess { get; init; }

        /// <summary>
        /// The API Error associated with the Result, if any. This property is null when the operation is successful.
        /// </summary>
        public ApiError? Error { get; init; }
    }
}
