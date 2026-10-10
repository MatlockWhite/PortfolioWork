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
    /// The UserService Write Context. 
    /// This will be used for Data Context that limits access to Write-Only functionality.
    /// NOTE:   This is a true Write-Only there is no Read functionality available to this Service.
    ///         That is by design and although there is functionality that would otherwise require Read and Write functionality,
    ///         such as creating a users Email Address when creating a new User, it will be handled in the ReadWriteContext class.
    /// </summary>
    public class UserServiceWriteContext : UserServiceContextBase
    {
        /// <summary>
        /// Represents the UserService DB Context using the Write-Only DB User Connection.
        /// </summary>
        /// <param name="options">The UserServiceWriteContext</param>
        public UserServiceWriteContext(DbContextOptions<UserServiceWriteContext> options)
            : base(options)
        {
        }
    }
}
