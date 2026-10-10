using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Contracts
{
    public class PagedResponse<T>
    {
        /// <summary>
        /// The collection of items returned from the API Query for the given Page and PageSize.
        /// </summary>
        public IReadOnlyCollection<T> Items { get; init; } = Array.Empty<T>();

        /// <summary>
        /// The current page number of the paged response.
        /// </summary>
        public int Page { get; init; }

        /// <summary>
        /// The number of items per page of the paged response.
        /// </summary>
        public int PageSize { get; init; }

        /// <summary>
        /// The total number of items available for the paged response.
        /// </summary>
        public int TotalItems { get; init; }

        /// <summary>
        /// Determines the total number of pages based on the total items and page size.
        /// </summary>
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
    }
}
