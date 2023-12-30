using Microsoft.EntityFrameworkCore;
using OrganizationApi.Context;
using OrganizationApi.Entity;
using OrganizationApi.Services.Base;

namespace OrganizationApi.Services.Impl;

public class OrganizationServiceImpl : BaseCrud<Organization>, IOrganizationService
{
    public OrganizationServiceImpl(ApplicationDbContext context) : base(context.Organizations)
    {
    }


    public async Task<Organization?> FindOneByOrganizationId(string id)
    {
        return await dao.FirstOrDefaultAsync(o => o.OrganizationId == id);
    }
}