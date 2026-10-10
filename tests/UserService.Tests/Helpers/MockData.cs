using UserService.Tests.Models;

namespace UserService.Tests.Helpers
{
    /// <summary>
    /// The following Data will be used for Automated Testing of the User Service. 
    /// This is a generic set of data that will be used to test the User Service API.
    /// </summary>
    public class MockData
    {
        private const string URIConnection = "http://localhost:5215";          // This is the URI Connection for the User Service API.

        // The following will be used for automated PW Check Tests.
        // A Valid Password Contains the following: 2 Uppercase Letters, 2 Lowercase Letters, 1 Special Character, 1 Number, a Minimum Length of 6 Characters. 
        private const string VALID_USED_PASSWORD = "Password@@Master11";        // This is the Generic Password we will be using for all accounts for this testing purposes.
        private const string VALID_UNUSED_PASSWORD = "PA$$w0rd";                // This Password meets all criteria for Passwords but is not used as the password.
        private const string INVALID_PASSWORD_NUMS_ONLY = "1234567";            // Invalid Password Containing No Letters or Special Characters.
        private const string INVALID_PASSWORD_NO_SPECIAL_CHAR = "PAssword11";   // Invalid Password Contains No Special Character.
        private const string INVALID_PASSWORD_NO_NUMS = "PA$$word!!";           // Invalid Password Contains No Numbers.
        private const string INVALID_PASSWORD_TOO_SHORT = "PA$5w";              // Invalid Password Too Short and missing 1 lowercase.
        private const string INVALID_PASSWORD_NO_UPPER_CASE = "pa$$word11";     // Invalid Password Contains No Uppercase Letters.
        private const string INVALID_PASSWORD_NO_LOWER_CASE = "PA$$W0RD";       // Invalid Password Contains No Lowercase Letters.
        private const string INVALID_PASSWORD_TOO_LONG = "PA$$w0rd1234567890123456789012345678901234567890";       // Invalid Password is Too Long.

        // The following will be used for automated Role Check Tests.
        private const int ADMIN_ROLE_ID = 1;
        private const int SUPERVISOR_ROLE_ID = 2;
        private const int WORKER_ROLE_ID = 3;
        private const int DRIVER_ROLE_ID = 4;

        // The following will be used for automated Role Check Tests.
        private const string ADMIN_ROLEKEY = "ADMINISTRATOR";
        private const string SUPERVISOR_ROLEKEY = "SUPERVISOR";
        private const string WORKER_ROLEKEY = "WAREHOUSE_WORKER";
        private const string DRIVER_ROLEKEY = "DRIVER";

        // The following are the current teams in the DB.
        private const string TEAM_1 = "Team 1";
        private const string TEAM_2 = "Team 2";
        private const string ADMINS = "Admins";

        // The following are used values already associated with other users in the DB.
        private const string USED_EMAIL = "Admin@Admin.com";
        private const string USED_USERNAME = "Admin_1";

        // The following Users will be used for Testing the different credentials and Roles and all other generic testing.
        // Team Admins Users
        private User ADMIN_1 = new User(3, "Admin_1", VALID_USED_PASSWORD, ADMINS, ADMIN_ROLE_ID, ADMIN_ROLEKEY);

        // Team 1 Users
        private User SUPERVISOR_1 = new User(4, "Supervisor_1", VALID_USED_PASSWORD, TEAM_1, SUPERVISOR_ROLE_ID, SUPERVISOR_ROLEKEY);
        private User WORKER_1 = new User(5, "Worker_1", VALID_USED_PASSWORD, TEAM_1, WORKER_ROLE_ID, WORKER_ROLEKEY);
        private User DRIVER_1 = new User(6, "Driver_1", VALID_USED_PASSWORD, TEAM_1, DRIVER_ROLE_ID, DRIVER_ROLEKEY);

        // Team 2 Users
        private User DRIVER_2 = new User(7, "Driver_2", VALID_USED_PASSWORD, TEAM_2, DRIVER_ROLE_ID, DRIVER_ROLEKEY);
        private User WORKER_2 = new User(8, "Worker_2", VALID_USED_PASSWORD, TEAM_2, WORKER_ROLE_ID, WORKER_ROLEKEY);
        private User SUPERVISOR_2 = new User(9, "Supervisor_2", VALID_USED_PASSWORD, TEAM_2, SUPERVISOR_ROLE_ID, SUPERVISOR_ROLEKEY);

        /// <summary>
        /// Constructor for the MockData class.
        /// </summary>
        public MockData() { }

        // URI Connection Getter for the MockData class.
        public string GetURIConnection() { return URIConnection; }

        // User Getters for the MockData class.
        public User GetAdminUser() { return ADMIN_1; }
        public User GetSupervisor1User() { return SUPERVISOR_1; }
        public User GetWorker1User() { return WORKER_1; }
        public User GetDriver1User() { return DRIVER_1; }
        public User GetSupervisor2User() { return SUPERVISOR_2; }
        public User GetWorker2User() { return WORKER_2; }
        public User GetDriver2User() { return DRIVER_2; }

        // Password Getters for the MockData class.
        public string GetValidUsedPassword() { return VALID_USED_PASSWORD; }
        public string GetValidUnusedPassword() { return VALID_UNUSED_PASSWORD; }
        public string GetInvalidPasswordNumsOnly() { return INVALID_PASSWORD_NUMS_ONLY; }
        public string GetInvalidPasswordNoSpecialChar() { return INVALID_PASSWORD_NO_SPECIAL_CHAR; }
        public string GetInvalidPasswordNoNums() { return INVALID_PASSWORD_NO_NUMS; }
        public string GetInvalidPasswordTooShort() { return INVALID_PASSWORD_TOO_SHORT; }
        public string GetInvalidPasswordNoUpperCase() { return INVALID_PASSWORD_NO_UPPER_CASE; }
        public string GetInvalidPasswordNoLowerCase() { return INVALID_PASSWORD_NO_LOWER_CASE; }
        public string GetInvalidPasswordTooLong() { return INVALID_PASSWORD_TOO_LONG; }

        // Role_Id Getters for the MockData class.
        public int GetAdminRoleId() { return ADMIN_ROLE_ID; }
        public int GetSupervisorRoleId() { return SUPERVISOR_ROLE_ID; }
        public int GetWorkerRoleId() { return WORKER_ROLE_ID; }
        public int GetDriverRoleId() { return DRIVER_ROLE_ID; }

        // RoleKey Getters for the MockData class.
        public string GetAdminRoleKey() { return ADMIN_ROLEKEY; }
        public string GetSupervisorRoleKey() { return SUPERVISOR_ROLEKEY; }
        public string GetWorkerRoleKey() { return WORKER_ROLEKEY; }
        public string GetDriverRoleKey() { return DRIVER_ROLEKEY; }

        // Team Getters for the MockData class.
        public string GetTeam1() { return TEAM_1; }
        public string GetTeam2() { return TEAM_2; }
        public string GetAdminsTeam() { return ADMINS; }

        // Used Email and Username Getters for the MockData class.
        public string GetUsedEmail() { return USED_EMAIL; }
        public string GetUsedUsername() { return USED_USERNAME; }
    }
}