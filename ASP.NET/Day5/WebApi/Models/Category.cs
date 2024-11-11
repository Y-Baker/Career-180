using System.ComponentModel.DataAnnotations;

namespace WebApi.Models;

public class Category : BaseEntity
{
    public string? Description { get; set; }

    public virtual List<Product> Products { get; set; } = new List<Product>();
}
