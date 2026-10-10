using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using UserService.Tests.Helpers;
using UserService.Tests.Models;

namespace UserService.Tests.Tests
{
    /// <summary>
    /// The Required Tests class are the essential tests that must be implemented to verify the core functionality of the UserService.
    /// These tests are critical to ensuring that the other UserService tests can be implemented and that the 
    /// UserService itself behaves as expected under various conditions and scenarios.
    /// </summary>
    public class RequiredTests
    {
        private readonly HttpClient _httpClient;
        private readonly MockData _mockData;
        private readonly UserHelper _userHelper;
        private readonly AuthenticationHelper _authenticationHelper;

        /// <summary>
        /// Setup the HttpClient for testing the User Service API. 
        /// The BaseAddress is set to the local development server where the User Service is running.
        /// </summary>
        public RequiredTests()
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
        /// Test the Login functionality of the User Service API. 
        /// This test simulates a login request using the ADMIN_1 user credentials and checks the response 
        /// for successful authentication.
        /// </summary>
        [Fact]
        public async Task LoginTest()
        {
            LoginRequest loginRequest = new LoginRequest
            {
                Username = _mockData.GetAdminUser().Username,
                Password = _mockData.GetAdminUser().Password
            };

            var token = await _authenticationHelper.Login(loginRequest);
            Assert.False(string.IsNullOrWhiteSpace(token));
            // NOTE:    If this method fails that means that User Authentication is not working and
            //          all other tests will fail as they require a valid token to execute.
        }

        /// <summary>
        /// Tests to verify that the JWT Authentication is working correctly. 
        /// This test simulates a login request using the ADMIN_1 user credentials, retrieves the JWT token, 
        /// and applies it to the HttpClient for subsequent requests. 
        /// It checks that the token is not null or empty, indicating successful authentication.
        /// </summary>
        /// <returns>A Task representing the asynchronous operation.</returns>
        [Fact]
        public async Task AuthenticationTest()
        {
            LoginRequest loginRequest = new LoginRequest
            {
                Username = _mockData.GetAdminUser().Username,
                Password = _mockData.GetAdminUser().Password
            };
            var token = await _authenticationHelper.Login(loginRequest);
            Assert.False(string.IsNullOrWhiteSpace(token));
            _authenticationHelper.ApplyJwtToken(token);

            // NOTE:    If this method fails that means that Token Authentication is not working and
            //          all other tests will fail as they require a valid token to execute.
        }

        /// <summary>
        /// Gets the List of Roles from the User Service API.
        /// </summary>
        /// <returns>Returns the List of Roles.</returns>
        [Fact]
        public async Task GetRoleTest()
        {
            var response = await _httpClient.GetAsync("/api/Role");
            Assert.True(response.IsSuccessStatusCode);

            var content = await response.Content.ReadFromJsonAsync<List<Role>>();

            // Verify Content was returned.
            Assert.NotNull(content);
            Assert.NotEmpty(content);

            // Check the Roles for expected output (ADMINISTRATOR, SUPERVISOR, WORKER, DRIVER)
            Assert.Contains(content, x => x.Id == _mockData.GetAdminRoleId() && x.RoleKey == _mockData.GetAdminRoleKey());
            Assert.Contains(content, x => x.Id == _mockData.GetSupervisorRoleId() && x.RoleKey == _mockData.GetSupervisorRoleKey());
            Assert.Contains(content, x => x.Id == _mockData.GetWorkerRoleId() && x.RoleKey == _mockData.GetWorkerRoleKey());
            Assert.Contains(content, x => x.Id == _mockData.GetDriverRoleId() && x.RoleKey == _mockData.GetDriverRoleKey());

            // NOTE: If this method fails that means that our role assignments for the User Methods is no longer viable and should not be trusted or executed.
        }


        /// <summary>
        /// Tests the GetUsers API call and ensures that the data that is returned does not contain the Hashed Password but does contain relevant User Data.
        /// </summary>
        /// <returns>Returns the List of Users.</returns>
        [Fact]
        public async Task GetUsersTest()
        {
            var response = await _httpClient.GetAsync("/api/User");
            Assert.True(response.IsSuccessStatusCode);

            // We need to test that the content does not return the PasswordHash value as that should remain hidden.
            var content = await response.Content.ReadAsStringAsync();
            Assert.DoesNotContain("passwordHash", content, StringComparison.OrdinalIgnoreCase);

            // Test that the rest of the User Data returned matches what we would expect.
            var users = await response.Content.ReadFromJsonAsync<List<UserResponse>>();

            // Verify Content was returned.
            Assert.NotNull(users);
            Assert.NotEmpty(users);


            // Check against the predefined User Information Saved. For this test we will use the values for Team 1.
            var adminUser = _mockData.GetAdminUser();
            Assert.NotNull(adminUser);

            Assert.Contains(users,
                    x => x.Id == adminUser.Id &&
                    x.Username == adminUser.Username &&
                    x.Team == adminUser.Team &&
                    x.RoleId == adminUser.RoleId);

            var supervisor1User = _mockData.GetSupervisor1User();
            Assert.NotNull(supervisor1User);

            Assert.Contains(users,
                    x => x.Id == supervisor1User.Id &&
                    x.Username == supervisor1User.Username &&
                    x.Team == supervisor1User.Team &&
                    x.RoleId == supervisor1User.RoleId);

            var worker1User = _mockData.GetWorker1User();
            Assert.NotNull(worker1User);

            Assert.Contains(users,
                    x => x.Id == worker1User.Id &&
                    x.Username == worker1User.Username &&
                    x.Team == worker1User.Team &&
                    x.RoleId == worker1User.RoleId);

            // NOTE:    If this method fails that means that our User Data is no
            //          longer being returned correctly and should not be trusted or executed.
        }

        /// <summary>
        /// Uses the AuthenticationTest variables and Creates a new User then Deletes the newly created User.
        /// </summary>
        /// <returns>Returns the Status of whether or not the Task was successful.</returns>
        [Fact]
        public async Task CreateAndDeleteUserTest()
        {
            LoginRequest loginRequest = new LoginRequest
            {
                Username = _mockData.GetAdminUser().Username,
                Password = _mockData.GetAdminUser().Password
            };
            var token = await _authenticationHelper.Login(loginRequest);
            Assert.False(string.IsNullOrWhiteSpace(token));
            _authenticationHelper.ApplyJwtToken(token);

            // Call the Create Default Test User Request.
            var createUserRequest = _userHelper.CreateDefaultTestUserRequest();
            Assert.NotNull(createUserRequest);

            var userResponse = await _userHelper.CreateUser(createUserRequest);
            Assert.NotNull(userResponse);

            // Delete the newly created User.
            await _userHelper.DeleteUser(userResponse.Id);

            _authenticationHelper.ClearJwtToken();

            // NOTE:    If this method fails that means that our User Creation and Deletion functionality is no
            //          longer being executed correctly and should not be trusted or executed.
        }
    }
}
