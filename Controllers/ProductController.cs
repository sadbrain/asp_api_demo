using api_demo.Data;
using api_demo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace api_demo.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ProductController : Controller
{
    private readonly ApplicationDbContext _db;

    public ProductController(ApplicationDbContext db)
    {
        _db = db;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        return await _db.Products.ToListAsync();
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await _db.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        return product;
    }
    [HttpPost]
    public async Task<ActionResult<Product>> PostProduct(Product product)
    {
        _db.Products.Add(product);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
    }
    [HttpPut("{id}")]
    public async Task<ActionResult> PutProduct(int id, Product product)
    {
        if (id != product.Id)
        {
            return BadRequest();
        }
        _db.Entry(product).State = EntityState.Modified;
        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ProductExists(id))
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
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _db.Products.FindAsync(id);

        if (product == null)
        {
            return NotFound();
        }

        _db.Products.Remove(product);
        await _db.SaveChangesAsync();

        return NoContent();
    }

    private bool ProductExists(int id)
    {
        return _db.Products.Any(p => p.Id == id);
    }
}

