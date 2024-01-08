using System.Collections.Immutable;
using OrganizationApi.Dto;
using OrganizationApi.Entity;
using OrganizationApi.Services.Base;

namespace OrganizationApi.Services;

public interface IIndustryService : IBaseCrud<Industry>
{
    ImmutableSortedDictionary<string, int> GetAllSortedDict();
    Task<Industry?> FindOneById(int id);
    Task<Industry?> FindOneByName(string name);
    public Task<Industry> CreateOrThrow(IndustryModel model);
    public Task<Industry> UpdateOrThrow(int id, IndustryModel model);

}