using OrganizationApi.Entity.Base;

namespace OrganizationApi.Entity;

public class Country : BaseEntity
{
    public string? Code { get; set; }
    public string Name { get; set; }
    public List<Organization> Organizations { get; set; } = new List<Organization>();
}