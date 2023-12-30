using OrganizationApi.Entity.Base;

namespace OrganizationApi.Entity;

public class Organization : BaseEntity
{
    public string OrganizationId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int NumberOfEmployees { get; set; }
    public Country Country { get; set; }
    public Industry Industry { get; set; }

    public int Founded { get; set; }
    public string Website { get; set; }
}