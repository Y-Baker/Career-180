using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models;

public class Product : BaseEntity
{
    public string? Description { get; set; }

    [Column(TypeName = "money")]
    public double Price { get; set; }

    public int Amount { get; set; } = 0;

    [Column("photo")]
    public string? PhotoId { get; set; }

    [ForeignKey("Category")]
    public int CategoryId { get; set; }

    public virtual Category? Category { get; set; }
}
