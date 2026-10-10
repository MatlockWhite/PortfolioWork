/**********************************************
 * 
 *      Portfolio Work For Matlock White
 *          User Service Example
 *              Model: User
 *              
 * ********************************************/


namespace UserService.Models;

/// <summary>
/// The following is the basic schema template for our User Model as it exists within the DB.
/// </summary>
public class User
{
    /// <summary>
    /// The Unique Id used for the User Index.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The Username for the given User. (Not Null, Unique)
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// The Password Hash value stored in the DB for the Given User. (Not Null).
    /// </summary>
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>
    /// The Firstname of the given user. (Not Null).
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// The Lastname of the given user. (Not Null).
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// The Team of the given user. (Nullable).
    /// </summary>
    public string? Team {  get; set; }

    /// <summary>
    /// The Email Address of the given user. (Not Null, Unique).
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// The PhoneNumber of the given user. (Nullable).
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// The Role Id Foreign Key for determining the users Role. (Foreign Key, Nullable).
    /// </summary>
    public int? RoleId { get; set; }

    /// <summary>
    /// The value indicating if the User is currently Active. (Not Null).
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// The Datetime value of when the given user was Last Active. (Nullable).
    /// </summary>
    public DateTime? LastActive { get; set; }

    /// <summary>
    /// The Role Data associated with the given user. This is referenced from the RoleId param.
    /// </summary>
    public Role? Role { get; set; }
}

/// <summary>
/// The User Response that is returned to the Front-End user. 
/// </summary>
public class UserResponse
{
    /*          The UserResponse expresses an important point, An API Contract does not equal a Database Entity.
                For this the User Response will be a more limited form of User where we will intentionally withold information
                that will be returned to the Front-End User.
    
        NOTE:   For this example the UserResponse will not contain the Password Hash simulating how we would never present 
                the Users Password information to the frontend user ever.

                For Readability I will not repeat the documentation I normally provide as it mirrors the Documentation for the 
                User model which can be found above.
    */

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

/// <summary>
/// The model passed in from Front-End for User Creation. 
/// Includes a non-hashed stringified password, but no IsActive, or LastActive properties.
/// </summary>
public class CreateUserRequest
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

/// <summary>
/// The Model passed in from Front-End for User Update/Deletion.
/// </summary>
public class UpdateUserRequest
{
    public string Username { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string? Team { get; set; }
    public int? RoleId { get; set; }
    public bool? IsActive { get; set; }
}

/// <summary>
/// The Model passed in from Front-End for User Login.
/// </summary>
public class LoginRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

/// <summary>
/// The Login Response used for Authentication purposes.
/// </summary>
public class LoginResponse
{
    public string Token { get; set;} = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public UserResponse User { get; set; } = new();
}