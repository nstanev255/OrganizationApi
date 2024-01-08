using System.ComponentModel.DataAnnotations;

namespace OrganizationApi.Entity.Base;

public abstract class BaseEntity
{
    [Key]
    public int Id { get; set; }
    public bool IsDeleted { get; set; } = false;
}