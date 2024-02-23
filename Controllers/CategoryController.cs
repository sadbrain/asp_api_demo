using api_demo.Data;
using api_demo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace api_demo.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CategoryController : Controller
{
    private readonly ApplicationDbContext _db;
    public CategoryController(ApplicationDbContext db) 
    {
        _db = db;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
    {
        return await _db.Categories.ToListAsync();
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<Category>> GetCategory(int id)
    {
        var category = await _db.Categories.FindAsync(id);
        if(category == null)
        {
            return NotFound();
        }
        return  category;
    }
    [HttpPost]
    public async Task<ActionResult<Category>> PostCategory(Category category)
    {
        _db.Categories.Add(category);
        _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
    }
    public async Task<IActionResult> PutCategory(int id, Category category)
    {
        if(id != category.Id)
        {
            return BadRequest();
        }

        _db.Entry(category).State = EntityState.Modified;
        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CategoryExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var category = await _db.Categories.FindAsync(id);

        if (category == null)
        {
            return NotFound();
        }

        _db.Categories.Remove(category);
        await _db.SaveChangesAsync();

        return NoContent();
    }
    private bool CategoryExists(int id)
    {
        return _db.Categories.Any(c => c.Id == id);
    }
}
