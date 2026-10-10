using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Tests.Models
{
    /// <summary>
    /// The Login Response class is a simple model simulating Login Response Data (Token).
    /// This is used for testing purposes to simulate login responses.
    /// </summary>
    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
    }
}
