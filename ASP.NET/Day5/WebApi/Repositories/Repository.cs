using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Repositories;

public class Repository<TEntity>
    where TEntity : BaseEntity
{
    ShopDbContext db;

    public Repository(ShopDbContext db)
    {
        this.db = db;
    }

    public List<TEntity> SelectAll(string? include = null)
    {
        if (include == null)
            return db.Set<TEntity>().ToList();
        return db.Set<TEntity>().Include(include).ToList();
    }

    public TEntity? SelectById(int id, string? include = null, bool track = true)
    {
        if (!track)
        {
            if (include == null)
                return db.Set<TEntity>().AsNoTracking().FirstOrDefault(e => e.Id == id);
            return db.Set<TEntity>().Include(include).AsNoTracking().FirstOrDefault(e => e.Id == id);
        }
        if (include == null)
            return db.Set<TEntity>().Find(id);
        return db.Set<TEntity>().Include(include).AsNoTracking().FirstOrDefault(e => e.Id == id);
    }

    public void Add(TEntity entity)
    {
        db.Set<TEntity>().Add(entity);
    }

    public void Update(TEntity entity)
    {
        db.Entry(entity).State = EntityState.Modified;
    }

    public void Delete(TEntity entity)
    {
        if (entity == null)
            throw new ArgumentNullException("Can't Delete Null");

        db.Set<TEntity>().Remove(entity);
    }
    public void Delete(int id)
    {
        TEntity? entity = db.Set<TEntity>().Find(id);

        if (entity is not null)
            Delete(entity);
    }
}
