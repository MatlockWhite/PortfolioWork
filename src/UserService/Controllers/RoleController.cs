/**********************************************
 * 
 *      Portfolio Work For Matlock White
 *          User Service Example
 *        Controllers: RoleController
 *              
 * ********************************************/


using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.Models;


namespace UserService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoleController : ControllerBase
{
    private readonly UserServiceReadContext _context;

    public RoleController(UserServiceReadContext context)
    {
        _context = context;
    }

    // Returns the Roles.
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Role>>> GetRoles()
    {
        // NOTE: We are using AsNoTracking here since the endpoint is read-only.
        //       The goal is that we reduce the overhead since EF doesnt need to track these Role Objects for later changes.
        var roles = await _context.Roles.AsNoTracking().ToListAsync();

        return Ok(roles);
    }
}