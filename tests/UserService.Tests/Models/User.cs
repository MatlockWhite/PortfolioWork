using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Tests.Models
{
    /// <summary>
    /// Generic structure of important User Information that we will need for Testing.
    /// NOTE: We are using an abbreviated version of the User class for testing purposes. In a real-world scenario, we would likely have a more comprehensive User model like that found in the UserService.
    /// </summary>
    public class User
    {
        /// <summary>
        /// User Constructor.
        /// </summary>
        /// <param name="id">The Users Id.</param>
        /// <param name="username">The Username associated with this User.</param>
        /// <param name="pw">The Users Unhashed PW.</param>
        /// <param name="team">The Users Team.</param>
        /// <param name="roleId">The Users Role Id.</param>
        /// <param name="roleKey">The Role Key value for clarity.</param>
        public User(int id, string username, string pw, string team, int roleId, string roleKey)
        {
            Id = id;
            Username = username;
            Password = pw;
            Team = team;
            RoleId = roleId;
            RoleKey = roleKey;
        }

        // Leaving the following properties undocumented for brevity, but they are self-explanatory based on their names and usage in the UserControllerTests class.
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Team { get; set; } = string.Empty;
        public int RoleId { get; set; }
        public string RoleKey { get; set; } = string.Empty;
    }
}
