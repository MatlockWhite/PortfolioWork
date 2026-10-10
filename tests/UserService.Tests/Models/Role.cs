using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Tests.Models
{
    /// <summary>
    /// The Role class is a duplicate version of the Role class from the UserService project. 
    /// It is used for testing purposes to simulate role data.
    /// </summary>
    public class Role
    {
        public int Id { get; set; }
        public string RoleKey { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
