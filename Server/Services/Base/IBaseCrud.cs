using OrganizationApi.Entity.Base;

namespace OrganizationApi.Services.Base;

public interface IBaseCrud<T> where T : BaseEntity
{
    List<T> ReadAll(int page, int pageSize);
    Task<T?> Read(string id);
    Task<T> Create(T entity);
    Task<T> Update(T entity);
    Task Delete(int entityId);
}