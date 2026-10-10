using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Headers
{
    public static class HeaderNames
    {
        /// <summary>
        /// The name of the header used to pass the Correlation ID in API requests and responses.
        /// Used for Tracing purposes to correlate requests and responses across different services.
        /// </summary>
        public const string CorrelationId = "X-Correlation-ID";
    }
}
