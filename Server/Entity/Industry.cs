using OrganizationApi.Entity.Base;

namespace OrganizationApi.Entity;

public class Industry : BaseEntity
{
    public string? Name { get; set; }
    public List<Organization> Organizations { get; set; } = new List<Organization>();
}