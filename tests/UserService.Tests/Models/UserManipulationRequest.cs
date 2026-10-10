using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Tests.Models
{
    /// <summary>
    /// The User Manipulation Request class is a model that simulates the data required to create a new user 
    /// or update an existing user in the system for Testing. 
    /// </summary>
    public class UserManipulationRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Team { get; set; }
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public int? RoleId { get; set; }
    }
}
