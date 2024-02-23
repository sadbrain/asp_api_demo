using api_demo.Data;
using api_demo.DTOs;
using api_demo.Models;
using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
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
    private readonly IMapper _mapper;
    public CategoryController(ApplicationDbContext db, IMapper mapper) 
    {
        _db = db;
        _mapper = mapper;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
    {
        //trả về một task vụ bao bộc thằng như trên
        //do đó kết quả trả về cần có await ở phía trướcS
        var categories = await _db.Categories.ToListAsync();
        //lấy những thực thể category trong database thành một list CategoryDto
        //The Map method performs the mapping based on the configuration provided by the mapper
        //The result is a list of ProductDto, which is then returned as the response.
        return _mapper.Map<List<CategoryDto>>(categories);
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryDto>> GetCategory(int id)
    {
        var category = await _db.Categories.FindAsync(id);
        if(category == null)
        {
            return NotFound();
        }
        return _mapper.Map<CategoryDto>(category);
    }
    [HttpPost]
    public async Task<ActionResult<CategoryDto>> PostCategory(Category category)
    {
        _db.Categories.Add(category);
        _db.SaveChangesAsync();
        //nó sẽ được tạo tại action có tên là GetCategory
        return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, _mapper.Map<CategoryDto>(category));
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCategory(int id, CategoryDto categoryDto)
    {
        if(id != categoryDto.Id)
        {
            return BadRequest();
        }
        ////Entry method is used to get an EntityEntry object for the specified entity
        //An EntityEntry represents the metadata and operations for a given entity being tracked by the DbContext.
        //trạng thái của một entity in the context
        //such as: "added", "unchanged', 'modified", "Deleted", "Detached"
        var category = _mapper.Map<Category>(categoryDto);
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
