using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UserService.Tests.Models;
using UserService.Tests.Helpers;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace UserService.Tests.Helpers
{
    /// <summary>
    /// The UserHelper class is a utility class designed to facilitate user-related operations in the context of testing the UserService. It provides methods for creating and deleting users, as well as managing authentication for administrative actions. This class is intended to be used in integration tests to ensure that user management functionalities work as expected.
    /// </summary>
    public class UserHelper
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationHelper _authenticationHelper;
        private readonly MockData _mockData;

        public UserHelper(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _authenticationHelper = new AuthenticationHelper(_httpClient);
            _mockData = new MockData();
        }

        /// <summary>
        /// Default Function that will attempt to create a Default User 
        /// </summary>
        /// <param name="createUserRequest">The User Manipulation Request Information.</param>
        /// <returns>Returns the newly created UserResponse if successful or Null if not successful.</returns>
        public async Task<UserResponse?> CreateUser(UserManipulationRequest createUserRequest)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/User", createUserRequest);
            Assert.NotNull(response);

            if (!response.IsSuccessStatusCode) { return null; }

            var userResponse = await response.Content.ReadFromJsonAsync<UserResponse>();
            return userResponse;
        }

        /// <summary>
        /// Deletes the matching User from the Users Table provided the userId.
        /// </summary>
        /// <param name="userId">The Id of the user to delete.</param>
        /// <returns>The HTTP Response Message.</returns>
        /// <exception cref="Exception">Throws this exception if the response indicates an unsuccessful status code.</exception>
        public async Task DeleteUser(int userId)
        {
            var response = await _httpClient.DeleteAsync($"/api/User/{userId}");
            Assert.True(response.IsSuccessStatusCode, $"Deletion of userID: {userId} failed with Status Code: {response.StatusCode}");
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        /// <summary>
        /// Re-usable Task which performs the CreateDefaultTestUser
        /// </summary>
        /// <returns>Returns the User Response for the newly created </returns>
        public async Task<UserResponse> HaveAdminCreateDefaultTestUser()
        {
            LoginRequest loginRequest = new LoginRequest
            {
                Username = _mockData.GetAdminUser().Username,
                Password = _mockData.GetAdminUser().Password
            };
            var token = await _authenticationHelper.Login(loginRequest);
            Assert.False(string.IsNullOrWhiteSpace(token));
            _authenticationHelper.ApplyJwtToken(token);

            var createUserRequest = CreateDefaultTestUserRequest();
            Assert.NotNull(createUserRequest);

            var userResponse = await CreateUser(createUserRequest);
            Assert.NotNull(userResponse);

            // Logout the Admin User.
            _authenticationHelper.ClearJwtToken();
            return userResponse;
        }

        /// <summary>
        /// Re-Usable Task which performs the DeleteUser functionality as an Administrator and then Logs out the Administrator.
        /// </summary>
        /// <param name="userId">The Id of the user to be Deleted.</param>
        /// <returns>Returns the status of whether the Task was successful or not.</returns>
        public async Task HaveAdminDeleteUser(int userId)
        {
            // Ensure Noone is logged in.
            _authenticationHelper.ClearJwtToken();

            LoginRequest loginRequest = new LoginRequest
            {
                Username = _mockData.GetAdminUser().Username,
                Password = _mockData.GetAdminUser().Password
            };

            var token = await _authenticationHelper.Login(loginRequest);
            Assert.False(string.IsNullOrWhiteSpace(token));
            _authenticationHelper.ApplyJwtToken(token);
            // Delete the User.
            await DeleteUser(userId);

            // Logout the Admin.
            _authenticationHelper.ClearJwtToken();
        }

        /// <summary>
        /// Creates a viable new UserManipulationRequest that uses GUID to prevent duplication of the username and Email which are meant to be unique.
        /// </summary>
        /// <returns>The newly created User Manipulation Request.</returns>
        public UserManipulationRequest CreateDefaultTestUserRequest()
        {
            var guid = Guid.NewGuid().ToString("N");

            return new UserManipulationRequest
            {
                Username = $"TEST_USER_{guid}",
                Password = _mockData.GetValidUsedPassword(),
                FirstName = "TEST_USER",
                LastName = "TEST_USER",
                Email = $"TEST_USER_{guid}@TEST.com",
                PhoneNumber = "(555)555-5555",
                RoleId = _mockData.GetWorkerRoleId(),
                Team = _mockData.GetTeam1()
            };
        }
    }
}
