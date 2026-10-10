using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Context
{
    public class CorrelationContext
    {
        /// <summary>
        /// The Unique Identifier used to correlate requests and responses across different services.
        /// </summary>
        public string CorrelationId { get; init; } = string.Empty;
    }
}
