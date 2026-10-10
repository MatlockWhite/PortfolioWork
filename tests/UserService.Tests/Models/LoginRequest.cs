using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Tests.Models
{
    /// <summary>
    /// The LoginRequest class is a simple model simulating Login Request Data (Username, Password).
    /// This is used for testing purposes to simulate login requests.
    /// </summary>
    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
