using WebApi.Models;
using WebApi.Repositories;

namespace WebApi.UnitOfWorks;

public class UnitOfWork
{
    ShopDbContext dbContext;
    ProductRepository? productRepo;
    Repository<Category>? categoryRepo;


    public UnitOfWork(ShopDbContext db)
    {
        dbContext = db;
    }

    public ProductRepository ProductRepo
    { 
        get
        {
            if (productRepo == null)
                productRepo = new ProductRepository(dbContext);
            return productRepo;
        }
    }

    public Repository<Category> CategoryRepo
    {
        get
        {
            if (categoryRepo == null)
                categoryRepo = new Repository<Category>(dbContext);
            return categoryRepo;
        }
    }


    public void Save()
    {
        dbContext.SaveChanges();
    }
}
