using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementAPI.Data;
using StudentManagementAPI.Filters;
using StudentManagementAPI.Models;

namespace StudentManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[SessionAuthorize]
public class UsersController : ControllerBase
{
    private readonly RaceDayContext _context;


public UsersController(RaceDayContext context)
    {
        _context = context;
    }

    [HttpGet]
    [SessionAuthorize("Organiser")]
    public async Task<ActionResult<IEnumerable<object>>> GetUsers()
    {
        var users = await _context.Users
            .AsNoTracking()
            .Select(u => new
            {
                u.UserId,
                u.FirstName,
                u.LastName,
                u.Email,
                u.Role,
                u.PhoneNumber,
                u.CreatedAt
            })
            .ToListAsync();

        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<object>> GetUser(int id)
    {
        var currentUserId = HttpContext.Session.GetInt32("UserId");
        var currentRole = HttpContext.Session.GetString("Role");

        if (currentRole != "Organiser" && currentUserId != id)
            return Forbid();

        var user = await _context.Users
            .AsNoTracking()
            .Where(u => u.UserId == id)
            .Select(u => new
            {
                u.UserId,
                u.FirstName,
                u.LastName,
                u.Email,
                u.Role,
                u.PhoneNumber,
                u.CreatedAt
            })
            .FirstOrDefaultAsync();

        if (user == null)
            return NotFound();

        return Ok(user);
    }


}
