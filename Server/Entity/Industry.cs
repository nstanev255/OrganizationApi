namespace OrganizationApi.Entity;

public class Industry
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public List<Organization> Organizations { get; set; } = new List<Organization>();
}