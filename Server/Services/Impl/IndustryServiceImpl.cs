using Microsoft.EntityFrameworkCore;
using OrganizationApi.Context;
using OrganizationApi.Entity;
using OrganizationApi.Services.Base;

namespace OrganizationApi.Services.Impl;

public class IndustryServiceImpl : BaseCrud<Industry>, IIndustryService
{

    public IndustryServiceImpl(ApplicationDbContext context) : base(context.Industries)
    {
    }


    public async Task<Industry?> FindOneById(int id)
    {
        return await dao.FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<Industry?> FindOneByName(string name)
    {
        return await dao.FirstOrDefaultAsync(i => i.Name == name);
    }
}