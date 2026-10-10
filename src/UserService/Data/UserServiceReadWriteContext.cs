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
    /// The UserService Read & Write Context. 
    /// This will be used for Data Context that limits access to Read & Write functionality.
    /// </summary>
    public class UserServiceReadWriteContext : UserServiceContextBase
    {
        /// <summary>
        /// Represents the UserService DB Context using Read & Write DB User Connection.
        /// </summary>
        /// <param name="options">The UserServiceReadWriteContext</param>
        public UserServiceReadWriteContext(DbContextOptions<UserServiceReadWriteContext> options)
            : base(options)
        {
        }
    }
}
