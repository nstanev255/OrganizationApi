using Microsoft.EntityFrameworkCore.Infrastructure;
using OrganizationApi.Entity.Base;

namespace OrganizationApi.Entity;

public class Organization : BaseEntity
{
    private ILazyLoader _lazyLoader;
    private Industry? _industry;
    private Country? _country;
    public string OrganizationId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int NumberOfEmployees { get; set; }
    public virtual Country? Country 
    { 
        get => _lazyLoader.Load(this, ref _country);
        set => _country = value;
    }
    public virtual Industry? Industry 
    { 
        get => _lazyLoader.Load(this, ref _industry);
        set => _industry = value; 
    }
    public int CountryId { get; set; }
    public int IndustryId { get; set; }
    public int Founded { get; set; }
    public string Website { get; set; }
    
    public Organization()
    {
    }
    
    private Organization(ILazyLoader loader)
    {
        _lazyLoader = loader;
    }
}