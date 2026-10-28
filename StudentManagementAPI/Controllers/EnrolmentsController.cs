using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementAPI.Data;
using StudentManagementAPI.Filters;
using StudentManagementAPI.Models;

namespace StudentManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[SessionAuthorize]
public class EnrolmentsController : ControllerBase
{
    private readonly RaceDayContext _context;


public EnrolmentsController(RaceDayContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Enrolment>>> GetEnrolments()
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        var role = HttpContext.Session.GetString("Role");

        var query = _context.Enrolments.AsNoTracking();

        if (role != "Organiser")
            query = query.Where(e => e.ParticipantId == userId);

        return await query.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Enrolment>> GetEnrolment(int id)
    {
        var enrolment = await _context.Enrolments.FindAsync(id);

        if (enrolment == null)
            return NotFound();

        var userId = HttpContext.Session.GetInt32("UserId");
        var role = HttpContext.Session.GetString("Role");

        if (role != "Organiser" && enrolment.ParticipantId != userId)
            return Forbid();

        return enrolment;
    }

    [HttpPost]
    [SessionAuthorize("Participant")]
    public async Task<ActionResult<Enrolment>> CreateEnrolment(Enrolment enrolment)
    {
        var userId = HttpContext.Session.GetInt32("UserId");

        if (userId == null)
            return Unauthorized();

        var categoryExists = await _context.EventCategories
            .AnyAsync(ec => ec.EventCategoryId == enrolment.EventCategoryId);

        if (!categoryExists)
            return BadRequest("The selected event category does not exist.");

        enrolment.EnrolmentId = 0;
        enrolment.ParticipantId = userId.Value;
        enrolment.EnrolmentDate = DateTime.Now;
        enrolment.EnrolmentStatus = "Registered";

        _context.Enrolments.Add(enrolment);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return Conflict("The enrolment could not be saved. You may already be enrolled in this category.");
        }

        return CreatedAtAction(
            nameof(GetEnrolment),
            new { id = enrolment.EnrolmentId },
            enrolment);
    }

    [HttpPut("{id}/cancel")]
    [SessionAuthorize("Participant")]
    public async Task<IActionResult> CancelEnrolment(int id)
    {
        var userId = HttpContext.Session.GetInt32("UserId");

        var enrolment = await _context.Enrolments.FindAsync(id);

        if (enrolment == null)
            return NotFound();

        if (enrolment.ParticipantId != userId)
            return Forbid();

        if (enrolment.EnrolmentStatus == "Cancelled")
            return BadRequest("This enrolment is already cancelled.");

        enrolment.EnrolmentStatus = "Cancelled";

        await _context.SaveChangesAsync();

        return NoContent();
    }


}

