using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Contracts
{
    public class PaginationRequest
    {
        /// <summary>
        /// The Page Number to be returned from the API Query.
        /// </summary>
        public int Page { get; init; } = 1;

        /// <summary>
        /// The maximum number of items to be returned from the API Query per Page.
        /// </summary>
        public int PageSize { get; init; } = 30;
    }
}
