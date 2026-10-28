using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementAPI.Data;
using StudentManagementAPI.Filters;
using StudentManagementAPI.Models;

namespace StudentManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly RaceDayContext _context;


public EventsController(RaceDayContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Event>>> GetEvents()
    {
        return await _context.Events
            .AsNoTracking()
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Event>> GetEvent(int id)
    {
        var raceEvent = await _context.Events.FindAsync(id);

        if (raceEvent == null)
            return NotFound();

        return raceEvent;
    }

    [HttpPost]
    [SessionAuthorize("Organiser")]
    public async Task<ActionResult<Event>> CreateEvent(Event raceEvent)
    {
        raceEvent.EventId = 0;

        _context.Events.Add(raceEvent);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetEvent),
            new { id = raceEvent.EventId },
            raceEvent);
    }

    [HttpPut("{id}")]
    [SessionAuthorize("Organiser")]
    public async Task<IActionResult> UpdateEvent(int id, Event updatedEvent)
    {
        if (id != updatedEvent.EventId)
            return BadRequest("The event ID in the URL must match the event ID in the request body.");

        var existingEvent = await _context.Events.FindAsync(id);

        if (existingEvent == null)
            return NotFound();

        existingEvent.OrganiserId = updatedEvent.OrganiserId;
        existingEvent.EventName = updatedEvent.EventName;
        existingEvent.Description = updatedEvent.Description;
        existingEvent.EventDate = updatedEvent.EventDate;
        existingEvent.StartTime = updatedEvent.StartTime;
        existingEvent.Location = updatedEvent.Location;
        existingEvent.Status = updatedEvent.Status;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    [SessionAuthorize("Organiser")]
    public async Task<IActionResult> DeleteEvent(int id)
    {
        var raceEvent = await _context.Events.FindAsync(id);

        if (raceEvent == null)
            return NotFound();

        _context.Events.Remove(raceEvent);
        await _context.SaveChangesAsync();

        return NoContent();
    }


}
