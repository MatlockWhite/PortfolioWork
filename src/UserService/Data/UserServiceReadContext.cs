/**********************************************
 * 
 *      Portfolio Work For Matlock White
 *          User Service Example
 *        Data: UserServiceReadContext
 *              
 * ********************************************/


using Microsoft.EntityFrameworkCore;

namespace UserService.Data
{
    /// <summary>
    /// The UserServiceRead Context. 
    /// This will be used for Data Context that limits access to Read-Only functionality.
    /// </summary>
    public class UserServiceReadContext : UserServiceContextBase
    {
        /// <summary>
        /// Represents the UserService DB Context using the Read-Only DB User Connection.
        /// </summary>
        /// <param name="options">The UserServiceReadContext</param>
        public UserServiceReadContext(DbContextOptions<UserServiceReadContext> options)
            : base(options)
        {
        }
    }
}
