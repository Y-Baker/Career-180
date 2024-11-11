using System.ComponentModel.DataAnnotations;

namespace WebApi.Models;

public class BaseEntity
{
    public int Id { get; set; }

    [StringLength(50)]
    public required string Name { get; set; }
}
