using Shared.Contracts;
using Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Tests
{
    public class ResultTests
    {
        /// <summary>
        /// Check that a Result object works by verifying IsSuccess is set to true has no error associated with it.
        /// </summary>
        [Fact (DisplayName = "Check that a Result object works by verifying IsSuccess is set to true has no error associated with it.")]
        public void Result_Success_ShouldHaveNoError()
        {
            var result = new Result
            {
                IsSuccess = true, 
                Error = null 
            };

            Assert.True(result.IsSuccess);
            Assert.Null(result.Error);
        }

        /// <summary>
        /// Check that the results value is set correctly when IsSuccess is true and that the Value matches what is expected.
        /// </summary>
        [Fact (DisplayName = "Check that the results value is set correctly when IsSuccess is true and that the Value matches what is expected.")]
        public void ResultT_Success_ContiansValue()
        {
            var result = new Results<string>
            {
                IsSuccess = true,
                Value = "Test Value"
            };

            Assert.True(result.IsSuccess);
            Assert.Equal("Test Value", result.Value);
        }

        /// <summary>
        /// Check that the results value is set correctly when IsSuccess is false and that the Error matches what is expected.
        /// </summary>
        [Fact (DisplayName = "Check that the results value is set correctly when IsSuccess is false and that the Error matches what is expected.")]
        public void ResultT_Failure_ContainsError()
        {
            var result = new Results<string>
            {
                IsSuccess = false,
                Error = new ApiError
                {
                    StatusCode = HTTPStatusCode.BadRequest,
                    ErrorCode = ErrorCode.BadRequest,
                    ErrorMessage = "Test Failed as Expected",
                }
            };

            Assert.False(result.IsSuccess);
            Assert.NotNull(result.Error);
            Assert.Equal(ErrorCode.BadRequest, result.Error.ErrorCode);
        }
    }
}
