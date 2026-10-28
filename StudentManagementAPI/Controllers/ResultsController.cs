using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementAPI.Data;
using StudentManagementAPI.Filters;
using StudentManagementAPI.Models;

namespace StudentManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[SessionAuthorize]
public class ResultsController : ControllerBase
{
    private readonly RaceDayContext _context;


public ResultsController(RaceDayContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<StudentManagementAPI.Models.Result>>> GetResults()
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        var role = HttpContext.Session.GetString("Role");

        var query = _context.Results
            .Join(
                _context.Enrolments,
                r => r.EnrolmentId,
                e => e.EnrolmentId,
                (r, e) => new { Result = r, Enrolment = e });

        if (role != "Organiser")
            query = query.Where(x => x.Enrolment.ParticipantId == userId);

        return await query.Select(x => x.Result).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<StudentManagementAPI.Models.Result>> GetResult(int id)
    {
        var result = await _context.Results.FindAsync(id);

        if (result == null)
            return NotFound();

        var userId = HttpContext.Session.GetInt32("UserId");
        var role = HttpContext.Session.GetString("Role");

        if (role != "Organiser")
        {
            var ownsResult = await _context.Enrolments.AnyAsync(
                e => e.EnrolmentId == result.EnrolmentId &&
                     e.ParticipantId == userId);

            if (!ownsResult)
                return Forbid();
        }

        return result;
    }

    [HttpPost]
    [SessionAuthorize("Organiser")]
    public async Task<ActionResult<StudentManagementAPI.Models.Result>> CreateResult(
        StudentManagementAPI.Models.Result result)
    {
        var enrolmentExists = await _context.Enrolments
            .AnyAsync(e => e.EnrolmentId == result.EnrolmentId);

        if (!enrolmentExists)
            return BadRequest("The selected enrolment does not exist.");

        if (result.FinishingPosition is <= 0)
            return BadRequest("Finishing position must be greater than zero.");

        result.ResultId = 0;
        result.RecordedAt = DateTime.Now;

        _context.Results.Add(result);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return Conflict("A result may already exist for this enrolment.");
        }

        return CreatedAtAction(
            nameof(GetResult),
            new { id = result.ResultId },
            result);
    }

    [HttpPut("{id}")]
    [SessionAuthorize("Organiser")]
    public async Task<IActionResult> UpdateResult(
        int id,
        StudentManagementAPI.Models.Result updatedResult)
    {
        if (id != updatedResult.ResultId)
            return BadRequest("The result ID in the URL must match the request body.");

        if (updatedResult.FinishingPosition is <= 0)
            return BadRequest("Finishing position must be greater than zero.");

        var result = await _context.Results.FindAsync(id);

        if (result == null)
            return NotFound();

        result.FinishTime = updatedResult.FinishTime;
        result.FinishingPosition = updatedResult.FinishingPosition;
        result.ResultStatus = updatedResult.ResultStatus;
        result.RecordedAt = DateTime.Now;

        await _context.SaveChangesAsync();

        return NoContent();
    }


}
