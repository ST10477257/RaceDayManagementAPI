using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementAPI.Data;
using StudentManagementAPI.Filters;
using StudentManagementAPI.Models;

namespace StudentManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly RaceDayContext _context;


public CategoriesController(RaceDayContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
    {
        return await _context.Categories
            .AsNoTracking()
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Category>> GetCategory(int id)
    {
        var category = await _context.Categories.FindAsync(id);

        if (category == null)
            return NotFound();

        return category;
    }

    [HttpPost]
    [SessionAuthorize("Organiser")]
    public async Task<ActionResult<Category>> CreateCategory(Category category)
    {
        category.CategoryId = 0;

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetCategory),
            new { id = category.CategoryId },
            category);
    }

    [HttpPut("{id}")]
    [SessionAuthorize("Organiser")]
    public async Task<IActionResult> UpdateCategory(int id, Category updatedCategory)
    {
        if (id != updatedCategory.CategoryId)
            return BadRequest("The category ID in the URL must match the request body.");

        var category = await _context.Categories.FindAsync(id);

        if (category == null)
            return NotFound();

        category.CategoryName = updatedCategory.CategoryName;
        category.Description = updatedCategory.Description;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    [SessionAuthorize("Organiser")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var category = await _context.Categories.FindAsync(id);

        if (category == null)
            return NotFound();

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();

        return NoContent();
    }


}


