using Microsoft.EntityFrameworkCore;
using OrganizationApi.Entity;

namespace OrganizationApi.Context;

public class ApplicationDbContext : DbContext
{
    public DbSet<Organization> Organizations { get; set; }
    public DbSet<Industry> Industries { get; set; }
    public DbSet<Country> Countries { get; set; }

    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }
}