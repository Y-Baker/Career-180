using WebApi.Models;

namespace WebApi.DTOs.Products;

public class AddProductDTO
{
    public required string ProductName { get; set; }
    public string? Description { get; set; }
    public double Price { get; set; }
    public int Amount { get; set; } = 0;
    public int CategoryId { get; set; }
}
