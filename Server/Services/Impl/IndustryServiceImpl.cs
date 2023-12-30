using Microsoft.EntityFrameworkCore;
using OrganizationApi.Context;
using OrganizationApi.Dto;
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
    
    public async Task<Industry> CreateOrThrow(IndustryModel model)
    {
        var exists = await FindOneByName(model.Name);
        if (exists != null)
        {
            throw new Exception("Industry already exists");
        }

        return await Create(new Industry { Name = model.Name });
    }

    public async Task<Industry> UpdateOrThrow(int id, IndustryModel model)
    {
        var industry = await FindOneById(id);
        if (industry == null)
        {
            throw new Exception("Industry not found");
        }

        if (industry.Name != model.Name)
        {
            var nameExists = await FindOneByName(model.Name);
            if (nameExists != null)
            {
                throw new Exception("Industry with same name already exists.");
            }

            industry.Name = model.Name;
        }

        return await Update(industry);
    }
}