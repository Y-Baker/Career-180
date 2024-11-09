using WebApi.Models;

namespace WebApi.DTOs.Products;

public class ProductDTO
{
    public int ProductId { get; set; }
    public required string ProductName { get; set; }
    public string? Description { get; set; }
    public double Price { get; set; }
    public int Amount { get; set; } = 0;
    public string? CategoryName { get; set; }
}
