using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementAPI.Data;
using StudentManagementAPI.Filters;

namespace StudentManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoutesController : ControllerBase
{
    private readonly RaceDayContext _context;


public RoutesController(RaceDayContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetRoutes()
    {
        var routes = await _context.Routes
            .AsNoTracking()
            .ToListAsync();

        return Ok(routes);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetRoute(int id)
    {
        var route = await _context.Routes.FindAsync(id);

        if (route == null)
            return NotFound();

        return Ok(route);
    }

    [HttpPost]
    [SessionAuthorize("Organiser")]
    public async Task<IActionResult> CreateRoute(Models.Route route)
    {
        if (route.DistanceKm <= 0)
            return BadRequest("Distance must be greater than zero.");

        var eventExists = await _context.Events
            .AnyAsync(e => e.EventId == route.EventId);

        if (!eventExists)
            return BadRequest("The selected event does not exist.");

        if (await _context.Routes.AnyAsync(r => r.EventId == route.EventId))
            return Conflict("A route already exists for this event.");

        route.RouteId = 0;
        _context.Routes.Add(route);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetRoute),
            new { id = route.RouteId },
            route);
    }

    [HttpPut("{id}")]
    [SessionAuthorize("Organiser")]
    public async Task<IActionResult> UpdateRoute(int id, Models.Route updatedRoute)
    {
        if (id != updatedRoute.RouteId)
            return BadRequest("The route ID in the URL must match the request body.");

        if (updatedRoute.DistanceKm <= 0)
            return BadRequest("Distance must be greater than zero.");

        var route = await _context.Routes.FindAsync(id);

        if (route == null)
            return NotFound();

        var eventExists = await _context.Events
            .AnyAsync(e => e.EventId == updatedRoute.EventId);

        if (!eventExists)
            return BadRequest("The selected event does not exist.");

        var duplicate = await _context.Routes.AnyAsync(
            r => r.EventId == updatedRoute.EventId && r.RouteId != id);

        if (duplicate)
            return Conflict("A route already exists for this event.");

        route.EventId = updatedRoute.EventId;
        route.RouteName = updatedRoute.RouteName;
        route.DistanceKm = updatedRoute.DistanceKm;
        route.RouteDescription = updatedRoute.RouteDescription;
        route.RouteMapUrl = updatedRoute.RouteMapUrl;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    [SessionAuthorize("Organiser")]
    public async Task<IActionResult> DeleteRoute(int id)
    {
        var route = await _context.Routes.FindAsync(id);

        if (route == null)
            return NotFound();

        _context.Routes.Remove(route);
        await _context.SaveChangesAsync();

        return NoContent();
    }


}


