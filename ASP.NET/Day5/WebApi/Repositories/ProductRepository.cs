using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Repositories;

public class ProductRepository : Repository<Product>
{
    private readonly ShopDbContext db;

    public ProductRepository(ShopDbContext db) : base(db)
    {
        this.db = db;
    }

    public List<Product> SelectByPrice(int minPrice, int maxPrice, string? include = null)
    {
        if (include == null)
            return db.Products.Where(p => p.Price >= minPrice && p.Price <= maxPrice).ToList();

        return db.Products.Where(p => p.Price >= minPrice && p.Price <= maxPrice).Include(include).ToList();
    }
}
