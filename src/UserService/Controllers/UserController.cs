/**********************************************
 * 
 *      Portfolio Work For Matlock White
 *          User Service Example
 *        Controllers: UserController
 *              
 * ********************************************/

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using UserService.Data;
using UserService.Models;

namespace UserService.Controllers
{
    /// <summary>
    /// The User Controller Responsible for Read-Only Requests.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        /// <summary>
        /// The DB Context for Read-Only Values.
        /// </summary>
        private readonly UserServiceReadContext _readContext;
        private readonly UserServiceWriteContext _writeContext;
        private readonly UserServiceReadWriteContext _readWriteContext;

        /// <summary>
        /// The following consts are the available Role Definitions that would be required for Role specific checks required in data manipulation.
        /// </summary>
        private const string ADMINISTRATOR = "ADMINISTRATOR";           // Admins can manipulate any User
        private const string SUPERVISOR = "SUPERVISOR";                 // Supervisors can manipulate Users belonging to their team.
        private const string WAREHOUSE_WORKER = "WAREHOUSE_WORKER";     // Can only manipulate themselves.
        private const string DRIVER = "DRIVER";                         // Can only manipulate themselves.

        /// <summary>
        /// The Configuration for our JWT.
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// The Controller Constructor.
        /// </summary>
        /// <param name="readContext">The DB Context used for this Controller (ReadOnly).</param>
        /// <param name="writeContext">The DB Context used for this Controller (WriteOnly).</param>
        /// <param name="readWriteContext">The DB Context used for this Controller (Read and Write).</param>
        public UserController(UserServiceReadContext readContext,
                                UserServiceWriteContext writeContext,
                                UserServiceReadWriteContext readWriteContext,
                                IConfiguration configuration)
        {
            _readContext = readContext ?? throw new ArgumentNullException(nameof(readContext));
            _writeContext = writeContext ?? throw new ArgumentNullException(nameof(writeContext));
            _readWriteContext = readWriteContext ?? throw new ArgumentNullException(nameof(readWriteContext));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <summary>
        /// Gets all Users.
        /// </summary>
        /// <returns>The List of DB Users modified to UserResponse for Front-End UI.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponse>>> GetUsers()
        {
            var users = await _readContext.Users
                .AsNoTracking()
                .Select(user => new UserResponse
                {
                    Id = user.Id,
                    Username = user.Username,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Team = user.Team,
                    PhoneNumber = user.PhoneNumber,
                    RoleId = user.RoleId,
                    IsActive = user.IsActive,
                    LastActive = user.LastActive
                })
                .ToListAsync();

            return Ok(users);
        }

        /// <summary>
        /// Gets the Specified User given the Users Id.
        /// </summary>
        /// <param name="id">The Users Id.</param>
        /// <returns>The specified User Information modified to UserResponse for Front-End UI.</returns>
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponse>> GetUser(int id)
        {
            var user = await _readContext.Users
                .AsNoTracking()
                .Where(user => user.Id == id)
                .Select(user => new UserResponse
                {
                    Id = user.Id,
                    Username = user.Username,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Team = user.Team,
                    PhoneNumber = user.PhoneNumber,
                    RoleId = user.RoleId,
                    IsActive = user.IsActive,
                    LastActive = user.LastActive
                })
                .FirstOrDefaultAsync();

            if (user == null) { return NotFound(); }
            return Ok(user);
        }

        /// <summary>
        /// Creates the DB User. 
        /// </summary>
        /// <param name="request">The Create User Request sent from Front-End.</param>
        /// <returns>Successful: The UserResponse containing appropriate data for Front-End. Failure: Failure Response. </returns>
        [HttpPost]
        public async Task<ActionResult<UserResponse>> CreateUser(CreateUserRequest request)
        {
            if (!TryGetActiveUserClaims(out var activeUserId, out var activeUserRole, out var activeUserTeam)) { return StatusCode(StatusCodes.Status401Unauthorized, "The Active User is Unauthorized to perform this Action."); }
            if (activeUserRole != ADMINISTRATOR) { return StatusCode(StatusCodes.Status401Unauthorized, "User Creation can only be performed by Administrators."); }

            // Check Username Validity.
            var usernameExists = await _readWriteContext.Users
                .AsNoTracking()
                .AnyAsync(user => user.Username == request.Username);
            if (usernameExists) { return Conflict("Username currently in use. Please Select another Username."); }

            // Check Email Validity.
            var emailExists = await _readWriteContext.Users
                .AsNoTracking()
                .AnyAsync(user => user.Email == request.Email);
            if (emailExists) { return Conflict("Email is currently in use. To retrieve forgotten account see account retrieval steps."); }

            // Check Password Validity.
            if (!ValidPassword(request.Password)) { return BadRequest("The Password you entered does not meet the criteria."); }

            // Check Role Validity.
            if (request.RoleId.HasValue)
            {
                var roleExists = await _readWriteContext.Roles
                    .AsNoTracking()
                    .AnyAsync(role => role.Id == request.RoleId.Value);
                if (!roleExists) { return BadRequest("The specified Role does not exist."); }
            }

            var user = new User
            {
                PasswordHash = CreatePasswordHash(request.Password),
                Username = request.Username,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                RoleId = request.RoleId,
                IsActive = true,
                LastActive = null,
                Team = request.Team,
            };

            _readWriteContext.Add(user);
            await _readWriteContext.SaveChangesAsync();

            var response = new UserResponse
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                RoleId = user.RoleId,
                IsActive = user.IsActive,
                LastActive = user.LastActive,
                Team = user.Team
            };

            return CreatedAtAction(
                nameof(GetUser),
                new { id = user.Id },
                response);
        }

        /// <summary>
        /// Updates a Specified User given the User parameters.
        /// </summary>
        /// <param name="id">The Id of the User to be updated.</param>
        /// <param name="request">The User Information to be updated to.</param>
        /// <returns>The Updated User Response Info.</returns>
        [Authorize]
        [HttpPatch("{id}")]
        public async Task<ActionResult<UserResponse>> UpdateUser(int id, UpdateUserRequest request)
        {
            try
            {
                if (!TryGetActiveUserClaims(out var activeUserId, out var activeUserRole, out var activeUserTeam)) { return StatusCode(StatusCodes.Status401Unauthorized, "The Active User is Unauthorized to perform this Action."); }

                var targetUser = await _readWriteContext.Users.FirstOrDefaultAsync(x => x.Id == id);
                if (targetUser == null) { return StatusCode(StatusCodes.Status404NotFound, "No Matching User Found."); }

                // Check for Permissions
                if (!IsAuthorizedToModifyUser(activeUserId, activeUserRole, activeUserTeam, targetUser)) { return StatusCode(StatusCodes.Status403Forbidden, "The Active User does not have the necessary Permissions to modify the specified user."); }
                if (!IsAuthorizedToModifyRestrictedFields(activeUserId, activeUserTeam, activeUserRole, request, targetUser)) { return StatusCode(StatusCodes.Status403Forbidden, "The Active User does not have the necessary Permissions to modify one or more fields of the specified user."); }

                // Handle the no duplicates exceptions and password validation (Username and Email should have no duplicate values)
                if (request.Username != null)
                {
                    var userNameExists = await _readWriteContext.Users
                        .AsNoTracking()
                        .AnyAsync(x =>
                            x.Id != targetUser.Id &&
                            x.Username == request.Username);

                    if (userNameExists) { return Conflict("Username is already in use. Cannot Update."); }
                }

                if (request.Email != null)
                {
                    var emailExists = await _readWriteContext.Users
                        .AsNoTracking()
                        .AnyAsync(x =>
                            x.Id != targetUser.Id &&
                            x.Email == request.Email);
                    if (emailExists) { return Conflict("Email is already in use. Cannot Update"); }
                }

                var acceptedPassword = true;
                if (!string.IsNullOrWhiteSpace(request.Password))
                {
                    acceptedPassword = ValidPassword(request.Password);
                    if (!acceptedPassword) { return BadRequest("Password does not meet requirements. Cannot Update User"); }
                }

                // Update each non-null field with the request information. A Nulled field means that the Active User does not have the authorization to make the change.
                if (request.Username != null) { targetUser.Username = request.Username; }
                if (request.Email != null) { targetUser.Email = request.Email; }
                if (request.FirstName != null) { targetUser.FirstName = request.FirstName; }
                if (request.LastName != null) { targetUser.LastName = request.LastName; }
                if (request.PhoneNumber != null) { targetUser.PhoneNumber = request.PhoneNumber; }
                if (request.Team != null) { targetUser.Team = request.Team; }
                if (request.IsActive.HasValue) { targetUser.IsActive = request.IsActive.Value; }
                if (request.RoleId != null) { targetUser.RoleId = request.RoleId; }
                if (!string.IsNullOrWhiteSpace(request.Password)) { targetUser.PasswordHash = CreatePasswordHash(request.Password); }

                await _readWriteContext.SaveChangesAsync();
                var response = new UserResponse
                {
                    Id = targetUser.Id,
                    Username = targetUser.Username,
                    FirstName = targetUser.FirstName,
                    LastName = targetUser.LastName,
                    PhoneNumber = targetUser.PhoneNumber,
                    Team = targetUser.Team,
                    Email = targetUser.Email,
                    RoleId = targetUser.RoleId,
                    IsActive = targetUser.IsActive,
                    LastActive = targetUser.LastActive,
                };

                return Ok(response);
            }
            catch (FormatException) { return BadRequest("The Request did not match the expectated values."); }
        }

        /// <summary>
        /// Delete a Specified User given the Users Id parameter.
        /// </summary>
        /// <param name="id">The Id of the User to be Deleted.</param>
        /// <returns>The Http Response Info.</returns>
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<HttpResponse>> DeleteUser(int id)
        {
            if (!TryGetActiveUserClaims(out var activeUserId, out var activeUserRole, out var activeUserTeam)) { return StatusCode(StatusCodes.Status401Unauthorized, "The Active User is Unauthorized to perform this Action."); }

            // Do not allow user to delete themselves.
            if (activeUserId == id) { return StatusCode(StatusCodes.Status403Forbidden, "Users are not permitted to Delete their own accounts."); }

            var targetUser = await _readWriteContext.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (targetUser == null) { return StatusCode(StatusCodes.Status404NotFound, "No Matching User Found."); }

            // Only Administrators are allowed to perform Deletions.
            if (activeUserRole != ADMINISTRATOR) { return StatusCode(StatusCodes.Status403Forbidden, "The Active User does not have the necessary Permissions to Delete the specified user."); }

            _readWriteContext.Users.Remove(targetUser);
            await _readWriteContext.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Login Request Processing for a specified User.
        /// </summary>
        /// <param name="request">The Login Request Parameters (Username, Password) </param>
        /// <returns>If credentials match DB User details return Login Response (JWT Token, Expiration Time, User Info), otherwise return Unauthorized access and no relevant data.</returns>
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
        {
            User? user = await _readWriteContext.Users
                .Include(user => user.Role)
                .AsNoTracking()
                .FirstOrDefaultAsync(user => user.Username == request.Username);

            var timeout = _configuration["JWT:ExpirationMinutes"] ?? throw new InvalidOperationException("JWT Expiration Minutes is not properly configured");

            if (user == null || !user.IsActive || !VerifyHashedPassword(request.Password, user.PasswordHash)) {
                return Unauthorized("Invalid Username or Password"); // Return no credible information to the user.
            }

            if (!int.TryParse(timeout, out var expirationMinutes) || expirationMinutes < 0) { throw new InvalidOperationException("JWT Expiration Configuration is Invalid"); }
            var expirationTime = DateTime.UtcNow.AddMinutes(expirationMinutes);
            var token = CreateJwtToken(user, expirationTime);

            var userResponse = new UserResponse
            {
                Id = user.Id,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Team = user.Team,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                RoleId = user.RoleId,
                IsActive = user.IsActive,
                LastActive = user.LastActive,
            };

            var response = new LoginResponse
            {
                Token = token,
                ExpiresAt = expirationTime,
                User = userResponse
            };

            return Ok(response);
        }

        /********************************
         *                              *
         *      Utility Functions       *
         *                              *
         ********************************/

        /// <summary>
        /// Creates a Hash value for the Password, we will be using PBKDF2 and SHA-256.
        /// </summary>
        /// <param name="password">The users provided Password.</param>
        /// <returns>The Hashed value Password.</returns>
        private string CreatePasswordHash(string password)
        {
            const string method = "PBKDF2-SHA256"; // We are attempting to use PBKDF2 (Password-Based Key Derivation Function 2) and SHA256 methods. Reason being that it shows foresight and development to account for future updates/revisions.
            const int iterationCount = 100000;
            const int saltSize = 16;    // 16 Bytes or 128-bit Salt size
            const int hashSize = 32;    // 32 Bytes or 256-bit Hash

            var saltValue = RandomNumberGenerator.GetBytes(saltSize);
            var hashValue = Rfc2898DeriveBytes.Pbkdf2(      // Excited this is my first time using a method we did not have to create.
                password,
                saltValue,
                iterationCount,
                HashAlgorithmName.SHA256,
                hashSize
            );

            var saltString = Convert.ToBase64String(saltValue);
            var hashString = Convert.ToBase64String(hashValue);

            var result = $"{method}${iterationCount}${saltString}${hashString}";
            return result;
        }

        /// <summary>
        /// Checks if the string that is passed in meets the minimum requirements for a valid Password.
        /// </summary>
        /// <param name="pw">The Password String being evaluated.</param>
        /// <returns>True: if the string meets the criteria as a Password String. False: If the string does not meet the Criteria. </returns>
        private bool ValidPassword(string pw)
        {
            var result = true;

            // Password minimum Requirements:
            var minLength = 6;              // PW must be at least 6 char long
            var maxLength = 30;             // PW must be no longer than 30 char long
            var numUpperRequired = 2;       // PW must contain 2 Uppercase character(s)
            var numLowerRequired = 2;       // PW Must contain 2 Lowercase character(s)
            var numSpecialRequired = 1;     // PW must contain 1 Special Character(s)
            var numDigitRequired = 1;       // PW Must contain 1 Number(s)

            if (pw.Any(char.IsWhiteSpace)) { return false; }    // No Whitespaces allowed.

            var numDigitCount = pw.Count(char.IsDigit);
            var upperCount = pw.Count(char.IsUpper);
            var lowerCount = pw.Count(char.IsLower);
            var specialNumCount = pw.Count(x => !char.IsLetterOrDigit(x));

            result = pw.Length >= minLength
                    && pw.Length <= maxLength
                    && specialNumCount >= numSpecialRequired
                    && upperCount >= numUpperRequired
                    && lowerCount >= numLowerRequired
                    && numDigitCount >= numDigitRequired;

            return result;
        }

        /// <summary>
        /// Checks if the Stored hashed password matches the given password.
        /// </summary>
        /// <param name="password">The provided password.</param>
        /// <param name="storedPasswordHash">The password from the Database for a given user.</param>
        /// <returns>Returns true if the provided password matches the hashed password for the user otherwise return false.</returns>
        private bool VerifyHashedPassword(string password, string storedPasswordHash)
        {
            var hashParts = storedPasswordHash.Split('$');  // Split the values in the Hash that should be seperated by a '$'.

            if (hashParts.Length != 4) { return false; }    // Something has gone wrong with the value separation.

            var method = hashParts[0];              // Method that was used.
            var iterationString = hashParts[1];     // iteration count.
            var saltString = hashParts[2];          // Salt used.
            var hashValueString = hashParts[3];     // Hash string.

            byte[] saltValue;
            byte[] storedHashValue;

            try
            {
                saltValue = Convert.FromBase64String(saltString);
                storedHashValue = Convert.FromBase64String(hashValueString);
            }
            catch (FormatException)
            {
                return false;
            }

            if (!int.TryParse(iterationString, out var iterationCount) || iterationCount <= 0) { return false; }

            // Ok so this is why we implemented the hash in a PBKDF2 format, I do this professionally so that we could properly track versioning and allow for
            // future development (even though we are not going to do that for this demonstration). 
            // 
            // The idea would be that when we used other methods they could be handled here as well as updating old Hash values to modern
            // methods without major changes to the codebase.
            switch (method)
            {
                case ("PBKDF2-SHA256"):
                    var providedHashValue = Rfc2898DeriveBytes.Pbkdf2(
                        password,
                        saltValue,
                        iterationCount,
                        HashAlgorithmName.SHA256,
                        storedHashValue.Length);

                    return CryptographicOperations.FixedTimeEquals(providedHashValue, storedHashValue);
                default:
                    return false;
            }
        }

        /// //<summary>
        /// Creates the JWT Token for the given user.
        /// </summary>
        /// <param name="user">The User Information.</param>
        /// <param name="expirationTime">The Time value that the JWT should expire.</param>
        /// <returns>The JWT Token with User information.</returns>
        /// <exception cref="InvalidOperationException">Throws this exception if the JWT section is not properly configured.</exception>
        private string CreateJwtToken(User user, DateTime expirationTime)
        {
            var jwtKey = _configuration["JWT:Key"] ?? throw new InvalidOperationException("JWT Key is not configured");
            var jwtIssuer = _configuration["JWT:Issuer"] ?? throw new InvalidOperationException("JWT Issuer is not configured");
            var jwtAudience = _configuration["JWT:Audience"] ?? throw new InvalidOperationException("JWT Audience is not configured");
            var timeout = _configuration["JWT:ExpirationMinutes"] ?? throw new InvalidOperationException("JWT Expiration Minutes is not configured");

            if (!int.TryParse(timeout, out var expirationMinutes) || expirationMinutes <= 0) { throw new InvalidOperationException("JWT Expiration Minutes Configuration is Invalid.");  }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),                               // Add the User Id to the Claim
                new Claim(ClaimTypes.Name, user.Username)                                               // Add the Username to the Claim
            };

            if (user.Role != null) { claims.Add(new Claim(ClaimTypes.Role, user.Role.RoleKey)); }       // Add the Rolekey to the Claim
            if (!string.IsNullOrWhiteSpace(user.Team)) { claims.Add(new Claim("team", user.Team)); }    // Add the Team name to the Claim

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: claims,
                expires: expirationTime,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Gets the Active User Claims if it exists.
        /// </summary>
        /// <param name="userId">The User Id.</param>
        /// <param name="roleKey">The Role Key value.</param>
        /// <param name="team">The Team Value.</param>
        /// <returns>Returns True value if the Active User Claim was found or False if not.</returns>
        private bool TryGetActiveUserClaims(out int userId, out string? roleKey, out string? team)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out userId))
            {
                roleKey = null;
                team = null;
                return false;
            }

            roleKey = User.FindFirst(ClaimTypes.Role)?.Value;
            team = User.FindFirst("team")?.Value;
            return true;
        }

        /// <summary>
        /// Determines if the Active User Claim has the Authorization to modify the target User Data.
        /// </summary>
        /// <param name="activeUserId">The Active User Id.</param>
        /// <param name="activeUserRole">The Active Users Rolekey value.</param>
        /// <param name="activeUserTeam">The Active Users Team.</param>
        /// <param name="targetUser">The Target Users Data.</param>
        /// <returns>Returns True if the Active user is allowed to modify the target Users Data otherwise return false.</returns>
        private bool IsAuthorizedToModifyUser(
            int activeUserId,
            string? activeUserRole,
            string? activeUserTeam,
            User targetUser)
        {
            if (activeUserId == targetUser.Id) { return true; }     // Users are allowed to modify their own data.
            if (activeUserRole == ADMINISTRATOR) { return true; }   // Administrators are allowed to modify any User.
            if (activeUserRole == SUPERVISOR &&
                !string.IsNullOrWhiteSpace(activeUserTeam) &&
                activeUserTeam == targetUser.Team) { return true; } // Supervisors are allowed to modify members of their team.
            
            return false;                                           // No Authorization is allowed. 
        }

        /// <summary>
        /// Checks the Authorization for modifying Administrative Restrictive Fields.
        /// </summary>
        /// <param name="activeUserTeam">The Active User's Team.</param>
        /// <param name="activeUserRole">The Active User's Role.</param>
        /// <param name="request">The Update Request data.</param>
        /// <param name="targetUser">The Target Users current Values.</param>
        /// <returns>Returns True if the Active User is allowed to change the information of the targetUser otherwise return False.</returns>
        private bool IsAuthorizedToModifyRestrictedFields(
            int activeUserId,
            string? activeUserTeam,
            string? activeUserRole,
            UpdateUserRequest request,
            User targetUser) 
        { 
            var isAdmin = activeUserRole == ADMINISTRATOR;
            var isValidSupervisor = (activeUserRole == SUPERVISOR && !string.IsNullOrWhiteSpace(activeUserTeam) && targetUser.Team == activeUserTeam);

            // -- Admin Specific fields: --
            // Check if we are attempting to alter the RoleId and the Active User has the authorization of Administrator.
            if (request.RoleId.HasValue &&
                request.RoleId != targetUser.RoleId &&
                !isAdmin)
            {
                return false;
            }

            // Check if we are attempting to alter the Team Value and the Active User has the authorization of Administrator.
            if (request.Team != null &&
                request.Team != targetUser.Team &&
                !isAdmin)
            {
                return false;
            }

            // -- Admin or Supervisor Specific Fields: --
            // Check if we are attempting to alter the IsActive Value and the Active User has the authorization of Administrator or Supervisor.
            if (request.IsActive.HasValue &&
                request.IsActive != targetUser.IsActive &&
                !isAdmin && 
                !isValidSupervisor)
            {
                return false;
            }

            // -- Admin or Current User Specific Fields: --
            // Check if we are attempting to alter the Password and the Active User has the authorization of Administrator or is the current User.
            if (!string.IsNullOrWhiteSpace(request.Password) &&
                !isAdmin &&
                activeUserId != targetUser.Id)
            {
                return false;
            }

            return true;
        }

        // --------NOTE: The following function is a great example of Dev Env test Function and should never be applied to prod level deployments. -----------------------------
        /// <summary>
        /// This Helper Function is Unique in that I am applying it only for testing purposes as I am already 
        /// tired of continuously reapplying the JWT now that we have that and Authorization working correctly. 
        /// </summary>
        /// <param name="token">The Token to be Applied as the Active User.</param>
        //private void AutomaticallyApplyTokenBecauseIDontWantToKeepDoingItOverAndOverForDemonstrationPurposes(string token)
        //{
        //    if (string.IsNullOrWhiteSpace(token)) { return; }
        //    ApplyJwtToken(token);
        //}
    }
}
