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
        var attached = dao.Attach(new T { Id = entityId });
        dao.Remove(attached.Entity);
        await attached.Context.SaveChangesAsync();
    }
}