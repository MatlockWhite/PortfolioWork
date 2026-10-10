using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Tests.Models
{
    /// <summary>
    /// The UserResponse class is a model that simulates the data returned from the system when retrieving user information for Testing.
    /// </summary>
    public class UserResponse
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Team { get; set; }
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public int? RoleId { get; set; }
        public bool IsActive { get; set; }
        public DateTime? LastActive { get; set; }
    }
}
