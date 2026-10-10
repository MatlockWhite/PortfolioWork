/**********************************************
 * 
 *      Portfolio Work For Matlock White
 *          User Service Example
 *              Model: Role
 *              
 * ********************************************/


namespace UserService.Models;

// The following is the basic schema template for our Role Model.
public class Role
{
    // The Unique Id used for the Role Index.
    public int Id { get; set; }

    // The Unique Key that describes what the Role is.
    public string RoleKey { get; set; } = string.Empty;

    // The Name of the Role that is used for Display Purposes.
    public string DisplayName { get; set; } = string.Empty;

    // Brief Description of the Roles Responsibilities.
    public string? Description { get; set; }
}