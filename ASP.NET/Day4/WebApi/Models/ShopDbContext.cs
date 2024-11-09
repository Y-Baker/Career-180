using Microsoft.EntityFrameworkCore;

namespace WebApi.Models;

public class ShopDbContext : DbContext
{
    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<Product> Products { get; set; }

    public ShopDbContext(DbContextOptions options) : base(options)
    {
    }
}
