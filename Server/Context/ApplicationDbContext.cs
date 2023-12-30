using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using OrganizationApi.Entity;

namespace OrganizationApi.Context;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public DbSet<Organization> Organizations { get; set; }
    public DbSet<Industry> Industries { get; set; }
    public DbSet<Country> Countries { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // Soft-deleting the entities.
        builder.Entity<Industry>()
            .HasQueryFilter(b => !b.IsDeleted);
        builder.Entity<Organization>()
            .HasQueryFilter(o => !o.IsDeleted);
        builder.Entity<Country>()
            .HasQueryFilter(o => !o.IsDeleted);
    }

    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }
}