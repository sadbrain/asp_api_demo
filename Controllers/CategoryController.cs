using api_demo.Data;
using api_demo.Models;
using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
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
        //trả về một task vụ bao bộc thằng như trên
        //do đó kết quả trả về cần có await ở phía trước
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
        //nó sẽ được tạo tại action có tên là GetCategory
        return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCategory(int id, Category category)
    {
        if(id != category.Id)
        {
            return BadRequest();
        }
        ////Entry method is used to get an EntityEntry object for the specified entity
        //An EntityEntry represents the metadata and operations for a given entity being tracked by the DbContext.
        //trạng thái của một entity in the context
        //such as: "added", "unchanged', 'modified", "Deleted", "Detached"
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
        //tìm kiếm và trả về boolean any
        return _db.Categories.Any(c => c.Id == id);
    }
}
