using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using UserService.Tests.Models;

namespace UserService.Tests.Helpers
{
    /// <summary>
    /// The Authentication Helper class is a utility class that provides methods to assist with 
    /// authentication-related tasks for testing the UserService. 
    /// </summary>
    public class AuthenticationHelper
    {
        private readonly HttpClient _httpClient;

        public AuthenticationHelper(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// The following would simulate an end-user passing the JWT Authentication Token back and forth when executing an API call.
        /// </summary>
        /// <param name="token">The Authorization Token for the currently Logged In and Active User.</param>
        public void ApplyJwtToken(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        /// <summary>
        /// The following would simulate an end-user clearing the JWT Authentication Token either using the logout functionality or manually clearing the token.
        /// </summary>
        public void ClearJwtToken()
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }

        /// <summary>
        /// Re-usable method to perform a login operation for a given users credentials of Username and Password. This method sends a POST request to the /api/User/login endpoint and returns the JWT token if the login is successful.
        /// </summary>
        /// <param name="loginRequest">The Login Request Information.</param>
        /// <returns>Returns the JWT Token if the login is successful; otherwise, returns null.</returns>
        public async Task<string?> Login(LoginRequest loginRequest)
        {
            // Basic Null checks for Username, and Password. This will result in a failure which causes an 
            if (loginRequest == null || string.IsNullOrWhiteSpace(loginRequest.Username) || string.IsNullOrWhiteSpace(loginRequest.Password)) { return null; }

            var response = await _httpClient.PostAsJsonAsync("/api/User/login", loginRequest);

            if (!response.IsSuccessStatusCode) { return null; }

            var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();

            return loginResponse?.Token;
        }
    }
}
