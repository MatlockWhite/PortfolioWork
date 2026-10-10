using Shared.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Tests
{
    public class ErrorCodeStatusCodeMapTests
    {
        /// <summary>
        /// Check that the GetStatusCode method returns the correct HTTP status code for a valid error code.
        /// For the test we are using the ErrorCode.NotFound which should map to HTTPStatusCode.NotFound (404).
        /// </summary>
        [Fact (DisplayName = "Check that the GetStatusCode method returns the correct HTTP status code for valid Error Code.")]
        public void GetStatusCode_ValidErrorCode_ReturnsCorrectStatusCode()
        {
            var result = ErrorCodeStatusCodeMap.GetStatusCode(ErrorCode.NotFound);
            Assert.Equal(HTTPStatusCode.NotFound, result);
        }

        /// <summary>
        /// Ensure that the GetStatusCode method returns null for an invalid error code.
        /// </summary>
        [Fact(DisplayName = "Ensure that the GetStatusCode method returns null for an invalid error code.")]
        public void GetStatusCode_InvalidErrorCode_ReturnsNull()
        {
            var result = ErrorCodeStatusCodeMap.GetStatusCode("I_DO_NOT_EXIST");
            Assert.Null(result);
        }
        
        /// <summary>
        /// Check that the GetErrorCode method returns the correct error code for a valid HTTP status code.
        /// For the test we are using the HTTPStatusCode.NotFound (404) which should map to ErrorCode.NotFound.
        /// </summary>
        [Fact(DisplayName = "Check that the GetErrorCode method returns the correct error code for a valid HTTP status code.")]
        public void GetErrorCode_ValidStatusCode_ReturnsCorrectErrorCode()
        {
            var result = ErrorCodeStatusCodeMap.GetErrorCode(HTTPStatusCode.NotFound);
            Assert.Equal(ErrorCode.NotFound, result);
        }

        /// <summary>
        /// Check that the GetErrorCode method returns null for an invalid HTTP status code.
        /// </summary>
        [Fact(DisplayName = "Check that the GetErrorCode method returns null for an invalid HTTP status code.")]
        public void GetErrorCode_InvalidStatusCode_ReturnsNull()
        {
            var result = ErrorCodeStatusCodeMap.GetErrorCode(999);
            Assert.Null(result);
        }
    }
}
