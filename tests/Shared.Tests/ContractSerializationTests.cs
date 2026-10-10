using Shared.Contracts;
using System;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Tests
{
    public class ContractSerializationTests
    {
        /// <summary>
        /// The Check to see if the APIResponse can be setup and Serialized/De-Serialized into JSON format correctly.
        /// For this we are testing generic data. This is meant to be a basic test that should always pass.
        /// </summary>
        [Fact (DisplayName = "The Check to see if the APIResponse can be setup and Serialized/De-Serialized into JSON correctly.")]
        public void ApiResponse_SerializesSuccessfully()
        {
            var response = new ApiResponse<string>
            {
                Success = true,
                Data = "Test",
                CorrelationId = "12345"
            };

            var json = JsonSerializer.Serialize(response);

            Assert.Contains("Test", json);
            Assert.Contains("12345", json);
        }

        /// <summary>
        /// The verification that the APIError can be Serialized/De-Serialized into JSON format correctly and properly references our Contract ErrorCode and HTTPStatusCodes.
        /// For this we are using our inhouse ErrorCode and HTTPStatusCode, other than that this is meant to be a basic test that should pass.
        /// </summary>
        [Fact (DisplayName = "The verification that the APIError can be Serialized/De-Serialized into JSON format correctly and properly references our Contract ErrorCodes and HTTPStatusCodes.")]
        public void ApiError_SerializesSuccessfully()
        {
            var error = new ApiError
            {
                StatusCode = HTTPStatusCode.NotFound,
                ErrorCode = ErrorCode.NotFound,
                ErrorMessage = "Resource not found."
            };

            var json = JsonSerializer.Serialize(error);

            Assert.Contains("404", json);
            Assert.Contains("NOT_FOUND", json);
            Assert.Contains("Resource not found.", json);
        }
    }
}
