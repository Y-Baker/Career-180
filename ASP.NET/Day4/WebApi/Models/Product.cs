using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models;

public class Product
{
    public int Id { get; set; }

    [StringLength(50)]
    public required string Name { get; set; }

    public string? Description { get; set; }

    [Column(TypeName = "money")]
    public double Price { get; set; }

    public int Amount { get; set; } = 0;

    [ForeignKey("Category")]
    public int CategoryId { get; set; }

    public virtual Category? Category { get; set; }
}
