namespace OrganizationApi.Services.Base;

public interface IBaseCrud<T> where T : class
{
    Task<T> Create(T entity);
    Task<T> Update(T entity);
    Task<T?> Delete(int entityId);
}