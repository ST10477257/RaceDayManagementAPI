using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementAPI.Data;
using StudentManagementAPI.Models;

namespace StudentManagementAPI.Controllers;

public record RegisterRequest(
string FirstName,
string LastName,
string Email,
string Password,
string? PhoneNumber,
string Role = "Participant");

public record LoginRequest(string Email, string Password);

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly RaceDayContext _context;
    private readonly PasswordHasher<RaceDayUser> _hasher = new();


public AuthController(RaceDayContext context)
    {
        _context = context;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.FirstName) ||
            string.IsNullOrWhiteSpace(request.LastName) ||
            string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest("First name, last name, email and password are required.");
        }

        if (request.Password.Length < 8)
            return BadRequest("Password must be at least 8 characters long.");

        if (!string.Equals(request.Role, "Participant", StringComparison.OrdinalIgnoreCase))
            return BadRequest("Public registration is only available for participants.");

        var email = request.Email.Trim();

        if (await _context.Users.AnyAsync(u => u.Email == email))
            return Conflict("An account with this email already exists.");

        var user = new RaceDayUser
        {
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            Email = email,
            Role = "Participant",
            PhoneNumber = request.PhoneNumber,
            CreatedAt = DateTime.Now
        };

        user.PasswordHash = _hasher.HashPassword(user, request.Password);

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Created($"/api/Users/{user.UserId}", new
        {
            user.UserId,
            user.FirstName,
            user.LastName,
            user.Email,
            user.Role
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var email = request.Email.Trim();

        var user = await _context.Users
            .SingleOrDefaultAsync(u => u.Email == email);

        if (user == null)
            return Unauthorized("Invalid email or password.");

        var verification = _hasher.VerifyHashedPassword(
            user, user.PasswordHash, request.Password);

        if (verification == PasswordVerificationResult.Failed)
            return Unauthorized("Invalid email or password.");

        HttpContext.Session.SetInt32("UserId", user.UserId);
        HttpContext.Session.SetString("Role", user.Role);

        return Ok(new
        {
            Message = "Login successful.",
            user.UserId,
            user.FirstName,
            user.LastName,
            user.Email,
            user.Role
        });
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return Ok(new { Message = "Logged out successfully." });
    }


}
