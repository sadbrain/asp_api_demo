using api_demo.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace api_demo.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> o) : base(o)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
}
