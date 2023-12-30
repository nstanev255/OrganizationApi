using Microsoft.EntityFrameworkCore;
using OrganizationApi.Entity.Base;

namespace OrganizationApi.Services.Base;

public abstract class BaseCrud<T> : IBaseCrud<T> where T : BaseEntity, new()
{
    protected readonly DbSet<T> dao;

    protected BaseCrud(DbSet<T> dao)
    {
        this.dao = dao;
    }

    public async Task<T> Create(T entity)
    {
        var industry = await dao.AddAsync(entity);
        await industry.Context.SaveChangesAsync();

        return industry.Entity;
    }

    public async Task<T> Update(T newEntity)
    {
        var updated = dao.Update(newEntity);
        await updated.Context.SaveChangesAsync();

        return updated.Entity;
    }

    public async Task Delete(int entityId)
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