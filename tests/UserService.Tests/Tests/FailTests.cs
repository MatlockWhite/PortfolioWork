/**
 * 
 *      The Fail Test File is where additional tests will be added to verify the functionality of the UserService.
 *      These tests are designed to return a failure response when attempting to perform Invalid Operations.
 *      
 *      The Following Tests will be added:
 *          - Prevention of Duplicate User values (Username, Email)
 *          - Reject Unauthorized User Manipulation: Tests (Worker, Driver, Unauthenticated, Supervisor of a different Team) 
 *          - Reject Unauthorized User Manipulation of Specific Fields: Tests (Supervisor of same Team attempting to change RoleId, Supervisor of same Team attempting to change Team)
 *          - Reject invalid Passwords: Tests (No Numbers, No Letters, Too Short, Too Long, No Capital Letter, No Lowercase Letter, No Special Character)
 * 
 */


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
    /// The Fail Tests class is where additional tests will be added to verify the functionality of the UserService.
    /// These tests are designed to ensure that the UserService does not allow invalid operations and 
    /// behaves as expected under various real-world conditions and scenarios.
    /// </summary>
    public class FailTests
    {
        private readonly HttpClient _httpClient;
        private readonly MockData _mockData;
        private readonly UserHelper _userHelper;
        private readonly AuthenticationHelper _authenticationHelper;

        /// <summary>
        /// Setup the HttpClient for testing the User Service API. 
        /// The BaseAddress is set to the local development server where the User Service is running.
        /// </summary>
        public FailTests()
        {
            _mockData = new MockData();
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(_mockData.GetURIConnection())
            };

            _userHelper = new UserHelper(_httpClient);
            _authenticationHelper = new AuthenticationHelper(_httpClient);
        }

        // Duplicate User Creation Tests
        /// <summary>
        /// Test to verify that the User Service API correctly handles attempts to create a new user with a username that already exists in the system.
        /// </summary>
        /// <returns>Returns whether or not the Task was able to successfully verify the duplicate username scenario.</returns>
        [Fact]
        public async Task DuplicateUsernameTest()
        {
            // Get a default valid UserManipulationRequest object to use for testing.
            UserManipulationRequest badUserData = _userHelper.CreateDefaultTestUserRequest();
            var adminUser = _mockData.GetAdminUser();

            // Change the Username to match an existing user in the system to simulate a duplicate username scenario.
            badUserData.Username = _mockData.GetUsedUsername();

            // Login as Administrator so we know we have the correct permissions to create a new user.
            LoginRequest loginRequest = new LoginRequest
            {
                Username = adminUser.Username,
                Password = adminUser.Password
            };

            // login as an Administrator.
            var tokenAdmin = await _authenticationHelper.Login(loginRequest);
            Assert.False(string.IsNullOrWhiteSpace(tokenAdmin));
            _authenticationHelper.ApplyJwtToken(tokenAdmin);

            // Attempt to create a new user with the duplicate username.
            var response = await _httpClient.PostAsJsonAsync("/api/User", badUserData);
            Assert.NotNull(response);
            // Now verify we received the correct status code for a duplicate username scenario.
            Assert.True(response.StatusCode == System.Net.HttpStatusCode.Conflict, $"Expected Conflict status code for duplicate username, but got {response.StatusCode}.");
        }

        /// <summary>
        /// Test to verify that the User Service API correctly handles attempts to create a new user with an email that already exists in the system.
        /// </summary>
        /// <returns>Returns whether or not the Task was able to successfully verify the duplicate email scenario.</returns>
        [Fact]
        public async Task DuplicateEmailTest()
        {

            // Get a default valid UserManipulationRequest object to use for testing.
            UserManipulationRequest badUserData = _userHelper.CreateDefaultTestUserRequest();
            var adminUser = _mockData.GetAdminUser();

            // Change the Email to match an existing user in the system to simulate a duplicate email scenario.
            badUserData.Email = _mockData.GetUsedEmail();

            // Login as Administrator so we know we have the correct permissions to create a new user.
            LoginRequest loginRequest = new LoginRequest
            {
                Username = adminUser.Username,
                Password = adminUser.Password
            };

            // login as an Administrator.
            var tokenAdmin = await _authenticationHelper.Login(loginRequest);
            Assert.False(string.IsNullOrWhiteSpace(tokenAdmin));
            _authenticationHelper.ApplyJwtToken(tokenAdmin);

            // Attempt to create a new user with the duplicate email.
            var response = await _httpClient.PostAsJsonAsync("/api/User", badUserData);
            Assert.NotNull(response);
            // Now verify we received the correct status code for a duplicate email scenario.
            Assert.True(response.StatusCode.Equals(System.Net.HttpStatusCode.Conflict), $"Expected Conflict status code for duplicate Email, but got {response.StatusCode}.");
        }

        // Unauthorized User Manipulation Tests (Worker, Driver, Unauthenticated, Supervisor of a different Team)

        /// <summary>
        /// Attempts to alter a newly created user as a Worker and expects to receive a failure response. 
        /// This test verifies that the User Service API correctly restricts unauthorized users from manipulating user data.
        /// </summary>
        /// <returns>Returns whether or not the Task was able to successfully verify the unauthorized user manipulation scenario.</returns>
        [Fact]
        public async Task UnauthorizedUserManipulationTest_Worker()
        {
            var userResponse = await _userHelper.HaveAdminCreateDefaultTestUser();
            Assert.NotNull(userResponse);

            LoginRequest loginRequest = new LoginRequest
            {
                Username = _mockData.GetWorker1User().Username,
                Password = _mockData.GetWorker1User().Password
            };

            // Relogin as an Worker.
            var tokenWorker = await _authenticationHelper.Login(loginRequest);
            Assert.False(string.IsNullOrWhiteSpace(tokenWorker));
            _authenticationHelper.ApplyJwtToken(tokenWorker);

            // Ok now we have the newly created user we need to change their name and call the update function.
            try
            {
                var id = userResponse.Id;
                var fName = "WorkerChanged_FName";

                userResponse.FirstName = fName;

                var response = await _httpClient.PatchAsJsonAsync($"/api/User/{userResponse.Id}", userResponse);
                Assert.True(response.StatusCode.Equals(System.Net.HttpStatusCode.Forbidden), $"Update failed with Status Code: {response.StatusCode}");
            }
            finally
            {
                // Ok now that we have run our tests we need to Have the Admin Delete the newly created User.
                await _userHelper.HaveAdminDeleteUser(userResponse.Id);
            }
        }

        /// <summary>
        /// Attempts to alter a newly created user as a Driver and expects to receive a failure response. 
        /// This test verifies that the User Service API correctly restricts unauthorized Driver users from manipulating user data.
        /// </summary>
        /// <returns>Returns whether or not the Task was able to successfully verify the unauthorized user manipulation scenario.</returns>
        [Fact]
        public async Task UnauthorizedUserManipulationTest_Driver()
        {
            var userResponse = await _userHelper.HaveAdminCreateDefaultTestUser();
            Assert.NotNull(userResponse);

            LoginRequest loginRequest = new LoginRequest
            {
                Username = _mockData.GetDriver1User().Username,
                Password = _mockData.GetDriver1User().Password
            };

            // Relogin as an Driver.
            var tokenDriver = await _authenticationHelper.Login(loginRequest);
            Assert.False(string.IsNullOrWhiteSpace(tokenDriver));
            _authenticationHelper.ApplyJwtToken(tokenDriver);

            // Ok now we have the newly created user we need to change their name and call the update function.
            try
            {
                var id = userResponse.Id;
                var fName = "DriverChanged_FName";

                userResponse.FirstName = fName;

                var response = await _httpClient.PatchAsJsonAsync($"/api/User/{userResponse.Id}", userResponse);
                Assert.True(response.StatusCode.Equals(System.Net.HttpStatusCode.Forbidden), $"Update failed with Status Code: {response.StatusCode}");
            }
            finally
            {
                // Ok now that we have run our tests we need to Have the Admin Delete the newly created User.
                await _userHelper.HaveAdminDeleteUser(userResponse.Id);
            }
        }

        /// <summary>
        /// Attempts to alter a newly created user as an unauthenticated user and expects to receive a failure response. 
        /// This test verifies that the User Service API correctly restricts unauthenticated users from manipulating user data.
        /// </summary>
        /// <returns>Returns whether or not the Task was able to successfully verify the unauthorized user manipulation scenario.</returns>
        [Fact]
        public async Task UnauthorizedUserManipulationTest_Unauthenticated()
        {
            var userResponse = await _userHelper.HaveAdminCreateDefaultTestUser();
            Assert.NotNull(userResponse);

            // Now clear the JWT Token to simulate an unauthenticated user.
            _authenticationHelper.ClearJwtToken();

            // Ok now we have the newly created user we need to change their name and call the update function.
            try
            {
                var id = userResponse.Id;
                var fName = "NoUserChanged_FName";

                userResponse.FirstName = fName;

                var response = await _httpClient.PatchAsJsonAsync($"/api/User/{userResponse.Id}", userResponse);
                Assert.True(response.StatusCode.Equals(System.Net.HttpStatusCode.Unauthorized), $"Update failed with Status Code: {response.StatusCode}");
            }
            finally
            {
                // Ok now that we have run our tests we need to Have the Admin Delete the newly created User.
                await _userHelper.HaveAdminDeleteUser(userResponse.Id);
            }
        }

        /// <summary>
        /// Attempts to alter a newly created user as a Supervisor of a different team and expects to receive a failure response.
        /// This test verifies that the User Service API correctly restricts supervisors from manipulating user data of other teams.
        /// </summary>
        /// <returns>Returns whether or not the Task was able to successfully verify the Forbidden user manipulation scenario.</returns>
        [Fact]
        public async Task UnauthorizedUserManipulationTest_Supervisor_WrongTeam()
        {
            var userResponse = await _userHelper.HaveAdminCreateDefaultTestUser();
            Assert.NotNull(userResponse);

            LoginRequest loginRequest = new LoginRequest
            {
                Username = _mockData.GetSupervisor2User().Username,
                Password = _mockData.GetSupervisor2User().Password
            };

            // Relogin as an Supervisor of Team 2.
            var tokenSupervisor = await _authenticationHelper.Login(loginRequest);
            Assert.False(string.IsNullOrWhiteSpace(tokenSupervisor));
            _authenticationHelper.ApplyJwtToken(tokenSupervisor);

            // Ok now we have the newly created user we need to change their name and call the update function.
            try
            {
                var id = userResponse.Id;
                var fName = "WrongTeamSupervisorChanged_FName";

                userResponse.FirstName = fName;

                var response = await _httpClient.PatchAsJsonAsync($"/api/User/{userResponse.Id}", userResponse);
                Assert.True(response.StatusCode.Equals(System.Net.HttpStatusCode.Forbidden), $"Update failed with Status Code: {response.StatusCode}");
            }
            finally
            {
                // Ok now that we have run our tests we need to Have the Admin Delete the newly created User.
                await _userHelper.HaveAdminDeleteUser(userResponse.Id);
            }
        }

        /// <summary>
        /// Verifies that a Supervisor of the same team is not allowed to modify certain fields of a user, 
        /// such as RoleId and expects to receive a failure response.
        /// </summary>
        /// <returns>Returns the status of whether or Not the Task was successful.</returns>
        [Fact]
        public async Task UnauthorizedUserManipulationTest_Supervisor_NotAllowedToModifySpecifiedFields_RoleId()
        {
            var userResponse = await _userHelper.HaveAdminCreateDefaultTestUser();
            Assert.NotNull(userResponse);

            LoginRequest loginRequest = new LoginRequest
            {
                Username = _mockData.GetSupervisor1User().Username,
                Password = _mockData.GetSupervisor1User().Password
            };

            // Relogin as an Supervisor of same Team.
            var tokenSupervisor = await _authenticationHelper.Login(loginRequest);
            Assert.False(string.IsNullOrWhiteSpace(tokenSupervisor));
            _authenticationHelper.ApplyJwtToken(tokenSupervisor);

            // Ok now we have the newly created user we need to change their name and call the update function.
            try
            {
                var id = userResponse.Id;
                var roleId = _mockData.GetAdminRoleId();

                userResponse.RoleId = roleId;

                var response = await _httpClient.PatchAsJsonAsync($"/api/User/{userResponse.Id}", userResponse);
                Assert.True(response.StatusCode.Equals(System.Net.HttpStatusCode.Forbidden), $"Update failed with Status Code: {response.StatusCode}");
            }
            finally
            {
                // Ok now that we have run our tests we need to Have the Admin Delete the newly created User.
                await _userHelper.HaveAdminDeleteUser(userResponse.Id);
            }
        }

        /// <summary>
        /// Verifies that a Supervisor of the same team is not allowed to modify certain fields of a user, 
        /// such as Team and expects to receive a failure response.
        /// </summary>
        /// <returns>Returns the status of whether or Not the Task was successful.</returns>
        [Fact]
        public async Task UnauthorizedUserManipulationTest_Supervisor_NotAllowedToModifySpecifiedFields_Team()
        {
            var userResponse = await _userHelper.HaveAdminCreateDefaultTestUser();
            Assert.NotNull(userResponse);

            LoginRequest loginRequest = new LoginRequest
            {
                Username = _mockData.GetSupervisor1User().Username,
                Password = _mockData.GetSupervisor1User().Password
            };

            // Relogin as an Supervisor of same Team.
            var tokenSupervisor = await _authenticationHelper.Login(loginRequest);
            Assert.False(string.IsNullOrWhiteSpace(tokenSupervisor));
            _authenticationHelper.ApplyJwtToken(tokenSupervisor);

            // Ok now we have the newly created user we need to change their name and call the update function.
            try
            {
                var id = userResponse.Id;
                var teamId = _mockData.GetTeam2();

                userResponse.Team = teamId;

                var response = await _httpClient.PatchAsJsonAsync($"/api/User/{userResponse.Id}", userResponse);
                Assert.True(response.StatusCode.Equals(System.Net.HttpStatusCode.Forbidden), $"Update failed with Status Code: {response.StatusCode}");
            }
            finally
            {
                // Ok now that we have run our tests we need to Have the Admin Delete the newly created User.
                await _userHelper.HaveAdminDeleteUser(userResponse.Id);
            }
        }

        // Password Verification Tests

        /// <summary>
        /// The following test verifies that the User Service API correctly handles updating/creating passwords with invalid values.
        /// </summary>
        /// <param name="passwordType">The type of invalid password to test.</param>
        /// <returns>Returns the status of whether or not the Task was successful.</returns>
        /// <exception cref="ArgumentException">Catch All Exception thrown when an unknown password type is provided.</exception>
        [Theory]
        [InlineData("NoNumbers")]
        [InlineData("TooShort")]
        [InlineData("TooLong")]
        [InlineData("NoCapitalLetter")]
        [InlineData("NoLowercaseLetter")]
        [InlineData("NoSpecialCharacter")]
        [InlineData("NoLetters")]
        public async Task PasswordValidationTest_InvalidPassword(string passwordType)
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

            try
            {
                string password = passwordType switch
                {
                    "NoNumbers" => _mockData.GetInvalidPasswordNoNums(),
                    "TooShort" => _mockData.GetInvalidPasswordTooShort(),
                    "TooLong" => _mockData.GetInvalidPasswordTooLong(),
                    "NoCapitalLetter" => _mockData.GetInvalidPasswordNoUpperCase(),
                    "NoLowercaseLetter" => _mockData.GetInvalidPasswordNoLowerCase(),
                    "NoSpecialCharacter" => _mockData.GetInvalidPasswordNoSpecialChar(),
                    "NoLetters" => _mockData.GetInvalidPasswordNumsOnly(),
                    _ => throw new ArgumentException($"Unknown password type: {passwordType}")
                };

                var response = await PasswordValidationTest_Generic(password, userResponse);
                Assert.True(response.StatusCode.Equals(System.Net.HttpStatusCode.BadRequest), $"Update failed with Status Code: {response.StatusCode}");
            }
            finally
            {                
                // Clean up by deleting the newly created user.
                await _userHelper.HaveAdminDeleteUser(userResponse.Id);
            }
        }

        /// <summary>
        /// The Generic Password Validation Test method is a reusable method that performs the password validation test for a given password and user response.
        /// </summary>
        /// <param name="password">The password to test.</param>
        /// <param name="userResponse">The user response object containing user details.</param>
        /// <returns>The HTTP response message resulting from the password validation test.</returns>
        public async Task<HttpResponseMessage> PasswordValidationTest_Generic(string password, UserResponse userResponse)
        {
            // Now because the UserResponse does not contain the Password, we need to use a UserManipulationRequest model from MockData.
            var id = userResponse.Id;
            var request = _userHelper.CreateDefaultTestUserRequest();

            request.Username = userResponse.Username;
            request.Email = userResponse.Email;
            request.Password = password;

            var response = await _httpClient.PatchAsJsonAsync($"/api/User/{userResponse.Id}", request);
            return response;
        }
    }
}
