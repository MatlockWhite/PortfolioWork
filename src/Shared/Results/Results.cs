using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Results
{
    public class Results<T> : Result
    {
        /// <summary>
        /// The Value(s) associated with the Result, if any. This property is null when the operation is unsuccessful.
        /// </summary>
        public T? Value { get; init; }
    }
}
