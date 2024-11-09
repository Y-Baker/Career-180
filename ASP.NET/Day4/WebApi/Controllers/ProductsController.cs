using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.DTOs.Products;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        ShopDbContext db;

        public ProductsController(ShopDbContext dbContext)
        {
            db = dbContext;
        }

        private ProductDTO ProductToDTO(Product product)
        {
            ProductDTO productDTO = new ProductDTO()
            {
                ProductId = product.Id,
                ProductName = product.Name,
                Description = product.Description,
                Price = product.Price,
                Amount = product.Amount,
                CategoryName = product.Category?.Name ?? "No Category"
            };

            return productDTO;
        }


        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> products = db.Products.Include(e => e.Category).ToList();
            List<ProductDTO> productsDTO = new List<ProductDTO>();

            foreach (Product product in products)
            {
                productsDTO.Add(ProductToDTO(product));
            }

            return Ok(productsDTO);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            Product? product = db.Products.Include(e => e.Category).SingleOrDefault(x => x.Id == id);

            if (product == null)
                return NotFound();

            return Ok(ProductToDTO(product));
        }

        [HttpGet("price")]
        public IActionResult GetByPrice([FromQuery] int maxPrice, [FromQuery] int minPrice = 0)
        {
            List<Product> products = db.Products.Where(p => p.Price >= minPrice && p.Price <= maxPrice).Include(e => e.Category).ToList();
            List<ProductDTO> productsDTO = new List<ProductDTO>();

            foreach (Product product in products)
            {
                productsDTO.Add(new ProductDTO()
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Amount = product.Amount,
                    CategoryName = product.Category?.Name ?? "No Category"
                });
            }

            return Ok(productsDTO);
        }

        [HttpPost]
        public IActionResult AddProduct(AddProductDTO productDTO)
        {
            if (productDTO == null)
                return BadRequest();

            if (!db.Categories.Any(c => c.Id == productDTO.CategoryId))
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest();

            Product product = new Product()
            {
                Name = productDTO.ProductName,
                Description = productDTO.Description,
                Price = productDTO.Price,
                Amount = productDTO.Amount,
                CategoryId = productDTO.CategoryId,
                Category = db.Categories.Where(n => n.Id == productDTO.CategoryId).SingleOrDefault()
            };

            db.Products.Add(product);
            db.SaveChanges();

            ProductDTO productAdded = ProductToDTO(product);

            return CreatedAtAction(nameof(GetById), new { id = product.Id }, productAdded);
        }

        [HttpPut("{id}")]
        public IActionResult EditProduct(int id, EditProductDTO productDTO)
        {
            if (productDTO == null || productDTO.ProductId != id)
                return BadRequest();

            if (!db.Categories.Any(c => c.Id == productDTO.CategoryId))
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest();

            Product product = new Product()
            {
                Id = productDTO.ProductId,
                Name = productDTO.ProductName,
                Description = productDTO.Description,
                Price = productDTO.Price,
                Amount = productDTO.Amount,
                CategoryId = productDTO.CategoryId,
                Category = db.Categories.Where(n => n.Id == productDTO.CategoryId).SingleOrDefault()
            };

            db.Entry(product).State = EntityState.Modified;
            db.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            Product? product = db.Products.SingleOrDefault(p => p.Id == id);
            if (product == null)
                return NotFound();

            db.Products.Remove(product);
            db.SaveChanges();

            return GetAll();
        }
    }
}
