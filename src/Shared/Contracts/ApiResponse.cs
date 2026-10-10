using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Contracts
{
    /// <summary>
    /// The Response returned from an API Query made by a Service.
    /// </summary>
    public class ApiResponse<T>
    {
        /// <summary>
        /// Gets or Initializes the Api Response query as Successful or Not.
        /// </summary>
        public bool Success { get; init; }

        /// <summary>
        /// Gets or Initializes the Data Value associated with the given ApiResponse
        /// Can be Any Type and/or Null.
        /// </summary>
        public T? Data { get; init; }

        /// <summary>
        /// Gets or Initializes the APIError values associated with the given Api Response.
        /// Can be Null.
        /// </summary>
        public ApiError? Error {get; init;}

        /// <summary>
        /// Gets or Initializes the Id of the ApiResponse
        /// Can be Null.
        /// </summary>
        public string? CorrelationId { get; init;}
    }


    /*
     * NOTE: I normally do not include examples of how this would appear but since this is for Portfolio work I will include it here.
     *      (P.S.): 
     *          I do normally include Documentation and comments in most if not all of my work, especially areas where the verbage or the terminology can
     *          potentially be misinterpreted. This used to be seen as a good thing but now since the introduction of AI I understand its questionability. 
     *          Just look for my mispelled words to verify it is me making this, lol.
     * 
     *      Examples of how this would/SHOULD look:
     *      JSON
     *      // Successfull Data request
     *      {
     *          "success": true,
     *          "data": {
     *              "id": 1,
     *              "username": "AdminUser"
     *          },
     *          "error": null,
     *          "correlationId": "Some Unique GUID made at Runtime"
     *      }
     *      // Unsuccessful Data Request
     *      {
     *          "success": false,
     *          "data": null,
     *          "error": {
     *              "status_code": 401,
     *              "code": "UNAUTHORIZED_ACCESS",
     *              "message": "The client lacks Valid Authorization Token" 
     *          },
     *          "correlationId": "Some Unique GUID made at Runtime"
     *      }
     */
}
