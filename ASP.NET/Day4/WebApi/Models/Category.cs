using System.ComponentModel.DataAnnotations;

namespace WebApi.Models;

public class Category
{
    public int Id { get; set; }

    [StringLength(50)]
    public required string Name { get; set; }

    public string? Description { get; set; }

    public virtual List<Product> Products { get; set; } = new List<Product>();
}
