using Microsoft.EntityFrameworkCore;
using OrganizationApi.Entity.Base;

namespace OrganizationApi.Services.Base;

public abstract class BaseCrud<T> : IBaseCrud<T> where T : BaseEntity
{
    protected readonly DbSet<T> dao;

    protected BaseCrud(DbSet<T> dao)
    {
        this.dao = dao;
    }

    public virtual List<T> ReadAll()
    {
        return dao.ToList();
    }

    /**
     * We will pass string as the param, the parse it to int.
     * This is done, so we can have flexibility if we want to override this method and search for something else...
     */
    public virtual async Task<T?> Read(string id)
    {
        return await dao.Where(e => e.Id == int.Parse(id)).FirstOrDefaultAsync();
    }

    public virtual async Task<T> Create(T entity)
    {
        var industry = await dao.AddAsync(entity);
        await industry.Context.SaveChangesAsync();

        return industry.Entity;
    }

    public virtual async Task<T> Update(T newEntity)
    {
        var updated = dao.Update(newEntity);
        await updated.Context.SaveChangesAsync();

        return updated.Entity;
    }

    public virtual async Task Delete(int entityId)
    {
        // Find the entity.
        var entity = await dao.Where(t => t.Id == entityId).FirstOrDefaultAsync();
        if (entity != null)
        {
            // And the soft-delete it.
            entity.IsDeleted = true;
            // Commit to database.
            await dao.Update(entity).Context.SaveChangesAsync();
        }
        else
        {
            throw new Exception("Entity cannot be deleted, as it does not exist");
        }
    }
}