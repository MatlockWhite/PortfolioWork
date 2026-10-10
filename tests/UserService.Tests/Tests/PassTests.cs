using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using UserService.Tests.Helpers;
using UserService.Tests.Models;

namespace UserService.Tests.Tests
{
    /// <summary>
    /// The Pass Tests class is where additional tests will be added to verify the functionality of the UserService.
    /// These tests should allow certain scenarios to pass and verify that the UserService behaves as expected.
    /// </summary>
    public class PassTests
    {
        private readonly HttpClient _httpClient;
        private readonly MockData _mockData;
        private readonly UserHelper _userHelper;
        private readonly AuthenticationHelper _authenticationHelper;

        /// <summary>
        /// Setup the HttpClient for testing the User Service API. 
        /// The BaseAddress is set to the local development server where the User Service is running.
        /// </summary>
        public PassTests()
        {
            _mockData = new MockData();
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(_mockData.GetURIConnection())
            };

            _userHelper = new UserHelper(_httpClient);
            _authenticationHelper = new AuthenticationHelper(_httpClient);
        }


        /// <summary>
        /// Test to ensure that Admins are considered Authorized Users and are able to successfully Modify a Given Users Data.
        /// </summary>
        /// <returns>Returns the status of whether or Not the Task was successful.</returns>
        [Fact]
        public async Task AuthorizedUserManipulationTest_Admin()
        {
            var userResponse = await _userHelper.HaveAdminCreateDefaultTestUser();
            Assert.NotNull(userResponse);

            LoginRequest loginRequest = new LoginRequest
            {
                Username = _mockData.GetAdminUser().Username,
                Password = _mockData.GetAdminUser().Password
            };

            // Relogin as an Administrator.
            var tokenAdmin = await _authenticationHelper.Login(loginRequest);
            Assert.False(string.IsNullOrWhiteSpace(tokenAdmin));
            _authenticationHelper.ApplyJwtToken(tokenAdmin);

            // Ok now we have the newly created user we need to change their name and call the update function.
            try
            {
                var id = userResponse.Id;
                var fName = "AdminChanged_FName";

                userResponse.FirstName = fName;

                // NOTE: Because we are using a patch and we have properly handled the update process inside of our UserService,
                // we can simply pass the entire UserResponse object back to the API and it will handle the update process for us
                // on only the fields that have changed. We also have measures in place to ensure that users without the proper
                // authorization are not able to update fields that they are not authorized to update and those should be treated
                // as null and skipped.

                var response = await _httpClient.PatchAsJsonAsync($"/api/User/{userResponse.Id}", userResponse);
                Assert.True(response.IsSuccessStatusCode, $"Update failed with Status Code: {response.StatusCode}");

                var updatedUserResponse = await response.Content.ReadFromJsonAsync<UserResponse>();
                Assert.NotNull(updatedUserResponse);
                Assert.Equal(fName, updatedUserResponse.FirstName);
                Assert.Equal(id, updatedUserResponse.Id);
            }
            finally
            {
                // Ok now that we have run our tests we need to Delete the newly created User.
                await _userHelper.DeleteUser(userResponse.Id);
            }
        }

        /// <summary>
        /// Test to ensure that Supervisors are considered Authorized Users for their teammembers and are able to 
        /// successfully Modify a Given Users Data if they belong to the same Team.
        /// </summary>
        /// <returns>Returns the status of whether or Not the Task was successful.</returns>
        [Fact]
        public async Task AuthorizedUserManipulationTest_Supervisor()
        {
            var userResponse = await _userHelper.HaveAdminCreateDefaultTestUser();
            Assert.NotNull(userResponse);

            LoginRequest loginRequest = new LoginRequest
            {
                Username = _mockData.GetSupervisor1User().Username,
                Password = _mockData.GetSupervisor1User().Password
            };

            // Relogin as a Supervisor
            var tokenSupervisor = await _authenticationHelper.Login(loginRequest);
            Assert.False(string.IsNullOrWhiteSpace(tokenSupervisor));
            _authenticationHelper.ApplyJwtToken(tokenSupervisor);

            // Ok now we have the newly created user we need to change their name and call the update function.
            try
            {
                var id = userResponse.Id;
                var fName = "SupervisorChanged_FName";

                userResponse.FirstName = fName;

                var response = await _httpClient.PatchAsJsonAsync($"/api/User/{userResponse.Id}", userResponse);
                Assert.True(response.IsSuccessStatusCode, $"Update failed with Status Code: {response.StatusCode}");

                var updatedUserResponse = await response.Content.ReadFromJsonAsync<UserResponse>();
                Assert.NotNull(updatedUserResponse);
                Assert.Equal(fName, updatedUserResponse.FirstName);
                Assert.Equal(id, updatedUserResponse.Id);
            }
            finally
            {
                await _userHelper.HaveAdminDeleteUser(userResponse.Id);
            }
        }

        /// <summary>
        /// Verifies that a Supervisor is not able to update fields that they are not authorized to update. 
        /// In this case, the Supervisor is attempting to update the IsActive field of a User which they are authorized to do. 
        /// The test verifies that the IsActive field changes after the update attempt.
        /// </summary>
        /// <returns>Returns the status of whether or Not the Task was successful.</returns>
        [Fact]
        public async Task AuthorizedUserManipulationTest_SupervisorSpecifiedFileds()
        {
            var userResponse = await _userHelper.HaveAdminCreateDefaultTestUser();
            Assert.NotNull(userResponse);

            LoginRequest loginRequest = new LoginRequest
            {
                Username = _mockData.GetSupervisor1User().Username,
                Password = _mockData.GetSupervisor1User().Password
            };

            // Relogin as a Supervisor
            var tokenSupervisor = await _authenticationHelper.Login(loginRequest);
            Assert.False(string.IsNullOrWhiteSpace(tokenSupervisor));
            _authenticationHelper.ApplyJwtToken(tokenSupervisor);

            // Ok now we have the newly created user we need to change their name and call the update function.
            try
            {
                var id = userResponse.Id;
                var isActive = false;

                userResponse.IsActive = isActive;

                var response = await _httpClient.PatchAsJsonAsync($"/api/User/{userResponse.Id}", userResponse);
                Assert.True(response.IsSuccessStatusCode, $"Update failed with Status Code: {response.StatusCode}");

                var updatedUserResponse = await response.Content.ReadFromJsonAsync<UserResponse>();
                Assert.NotNull(updatedUserResponse);
                Assert.Equal(isActive, updatedUserResponse.IsActive);
                Assert.Equal(id, updatedUserResponse.Id);
            }
            finally
            {
                await _userHelper.HaveAdminDeleteUser(userResponse.Id);
            }
        }

        /// <summary>
        /// Test to ensure that a User is able to successfully Modify their 
        /// own Data and that they are considered an Authorized User for their own data.
        /// </summary>
        /// <returns>Returns the status of whether or Not the Task was successful.</returns>
        [Fact]
        public async Task AuthorizedUserManipulationTest_Self() {
            var userResponse = await _userHelper.HaveAdminCreateDefaultTestUser();
            Assert.NotNull(userResponse);

            LoginRequest loginRequest = new LoginRequest
            {
                Username = userResponse.Username,
                Password = _mockData.GetValidUsedPassword()     // The Password for the newly created user is set to the Valid Used Password from MockData.
            };

            // Relogin as the newly created user.
            var tokenSelf = await _authenticationHelper.Login(loginRequest);
            Assert.False(string.IsNullOrWhiteSpace(tokenSelf));
            _authenticationHelper.ApplyJwtToken(tokenSelf);

            // Ok now we have the newly created user we need to change their name and call the update function.
            try
            {
                var id = userResponse.Id;
                var fName = "SelfChanged_FName";

                userResponse.FirstName = fName;

                var response = await _httpClient.PatchAsJsonAsync($"/api/User/{userResponse.Id}", userResponse);
                Assert.True(response.IsSuccessStatusCode, $"Update failed with Status Code: {response.StatusCode}");

                var updatedUserResponse = await response.Content.ReadFromJsonAsync<UserResponse>();
                Assert.NotNull(updatedUserResponse);
                Assert.Equal(fName, updatedUserResponse.FirstName);
                Assert.Equal(id, updatedUserResponse.Id);
            }
            finally
            {
                await _userHelper.HaveAdminDeleteUser(userResponse.Id);
            }
        }

        /// <summary>
        /// Test to ensure that a Valid Password is acceptable when performing a User/Creation or User Password Change.
        /// </summary>
        /// <returns>Returns the status of whether or Not the Task was successful.</returns>
        [Fact]
        public async Task PasswordValidationTest_ValidPassword() {
            var userResponse = await _userHelper.HaveAdminCreateDefaultTestUser();
            Assert.NotNull(userResponse);

            LoginRequest loginRequest = new LoginRequest
            {
                Username = _mockData.GetAdminUser().Username,
                Password = _mockData.GetAdminUser().Password
            };

            // Relogin as an Administrator.
            var tokenAdmin = await _authenticationHelper.Login(loginRequest);
            Assert.False(string.IsNullOrWhiteSpace(tokenAdmin));
            _authenticationHelper.ApplyJwtToken(tokenAdmin);

            // Ok now we have the newly created user we need to change their name and call the update function.
            try
            {
                // Now because the UserResponse does not contain the Password, we need to set the values of the Password to a valid unused password from MockData.
                var id = userResponse.Id;
                var password = _mockData.GetValidUnusedPassword();

                UserManipulationRequest request = new UserManipulationRequest
                {
                    FirstName = userResponse.FirstName,
                    LastName = userResponse.LastName,
                    Username = userResponse.Username,
                    Email = userResponse.Email,
                    PhoneNumber = userResponse.PhoneNumber,
                    Team = userResponse.Team,
                    RoleId = userResponse.RoleId,
                    Password = password
                };

                var response = await _httpClient.PatchAsJsonAsync($"/api/User/{userResponse.Id}", request);
                Assert.True(response.IsSuccessStatusCode, $"Update failed with Status Code: {response.StatusCode}");

                // Now because our UserResponse does not contain the Password, we cannot verify that the Password was updated successfully through GetUser methods.
                // However, we can verify that password was updated successfully by attempting to login with the new password.

                LoginRequest loginRequestNewPassword = new LoginRequest
                {
                    Username = userResponse.Username,
                    Password = password
                };

                _authenticationHelper.ClearJwtToken(); // Clear the previous token before attempting to login with the new password.
                var tokenNewPassword = await _authenticationHelper.Login(loginRequestNewPassword);
                Assert.False(string.IsNullOrWhiteSpace(tokenNewPassword));
            }
            finally
            {
                // Ok now that we have run our tests we need to Delete the newly created User.
                await _userHelper.HaveAdminDeleteUser(userResponse.Id);
            }
        }
    }
}
