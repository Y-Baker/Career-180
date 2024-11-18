using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.DTOs.Products;
using WebApi.Models;
using WebApi.UnitOfWorks;
using System.IO;
using Swashbuckle.AspNetCore.Annotations;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        UnitOfWork db;
        IConfiguration configuration;

        public ProductsController(UnitOfWork unit, IConfiguration configuration)
        {
            db = unit;
            this.configuration = configuration;
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
                CategoryName = product.Category?.Name ?? "No Category",
                PhotoPath = product.PhotoPath
            };

            return productDTO;
        }


        [SwaggerOperation("Get All Products", "Return a list of all products in the system.")]
        [SwaggerResponse(200, "Successfully", typeof(List<ProductDTO>))]
        [Produces("application/json")]
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> products = db.ProductRepo.SelectAll("Category");

            List<ProductDTO> productsDTO = new List<ProductDTO>();

            foreach (Product product in products)
            {
                productsDTO.Add(ProductToDTO(product));
            }

            return Ok(productsDTO);
        }

        [SwaggerOperation("Get Product By Id", "Return an specific product based on its id givin in route in the system.")]
        [SwaggerResponse(200, "Successfully", typeof(ProductDTO))]
        [SwaggerResponse(404, "Failed,Id Not Found")]
        [Produces("application/json")]
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            Product? product = db.ProductRepo.SelectById(id, "Category");

            if (product == null)
                return NotFound();

            return Ok(ProductToDTO(product));
        }

        [SwaggerOperation("Get Products by Price", "Return a list of products that their price in the range givin as query (maxPrice, minPrice).")]
        [SwaggerResponse(200, "Successfully", typeof(List<ProductDTO>))]
        [Produces("application/json")]
        [HttpGet("price")]
        public IActionResult GetByPrice([FromQuery] int maxPrice, [FromQuery] int minPrice = 0)
        {
            List<Product> products = db.ProductRepo.SelectByPrice(minPrice, maxPrice, "Category");

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
                    CategoryName = product.Category?.Name ?? "No Category",
                    PhotoPath = product.PhotoPath
                });
            }

            return Ok(productsDTO);
        }


        [SwaggerOperation("Add New Product", "Add New Product given in the body to the system.")]
        [SwaggerResponse(201, "Successfully Added", typeof(ProductDTO))]
        [SwaggerResponse(400, "Failed")]
        [Produces("application/json")]
        [HttpPost]
        public IActionResult AddProduct(AddProductDTO productDTO)
        {
            if (productDTO == null)
                return BadRequest();

            if (db.CategoryRepo.SelectById(productDTO.CategoryId) is null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest();

            string? photoPath = null;
            if (productDTO.Photo is not null)
                photoPath = AddPhoto(productDTO.Photo);

            Product product = new Product()
            {
                Name = productDTO.ProductName,
                Description = productDTO.Description,
                Price = productDTO.Price,
                Amount = productDTO.Amount,
                CategoryId = productDTO.CategoryId,
                Category = db.CategoryRepo.SelectById(productDTO.CategoryId),
                PhotoPath = photoPath
            };

            db.ProductRepo.Add(product);
            db.Save();

            ProductDTO productAdded = ProductToDTO(product);

            return CreatedAtAction(nameof(GetById), new { id = product.Id }, productAdded);
        }


        [SwaggerOperation("Update a Product", "Update an Existing Product given in the body to the system.")]
        [SwaggerResponse(204, "Successfully Added")]
        [SwaggerResponse(404, "Product Not Exists")]
        [SwaggerResponse(400, "Failed")]
        [Produces("application/json")]
        [HttpPut("{id}")]
        public IActionResult EditProduct(int id, EditProductDTO productDTO)
        {
            if (productDTO == null || productDTO.ProductId != id)
                return BadRequest();

            if (db.CategoryRepo.SelectById(productDTO.CategoryId) is null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest();

            Product? productBefore = db.ProductRepo.SelectById(id, track: false);
            if (productBefore == null)
                return NotFound();

            string? photoPath = productBefore.PhotoPath;
            if (productDTO.Photo is not null)
            {
                if (productBefore.PhotoPath is not null)
                    RemovePhoto(productBefore.PhotoPath);
                photoPath = AddPhoto(productDTO.Photo);
            }

            Product product = new Product()
            {
                Id = productDTO.ProductId,
                Name = productDTO.ProductName,
                Description = productDTO.Description,
                Price = productDTO.Price,
                Amount = productDTO.Amount,
                CategoryId = productDTO.CategoryId,
                Category = db.CategoryRepo.SelectById(productDTO.CategoryId),
                PhotoPath = photoPath
            };

            db.ProductRepo.Update(product);
            db.Save();

            return NoContent();
        }


        [SwaggerOperation("Delete Product", "Delete an Existing Product by an id given in the rout from the system")]
        [SwaggerResponse(200, "Successfully Deleted", typeof(List<ProductDTO>))]
        [SwaggerResponse(404, "Product Not Exists")]
        [Produces("application/json")]
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            Product? product = db.ProductRepo.SelectById(id);
            if (product == null)
                return NotFound();

            db.ProductRepo.Delete(product);
            db.Save();

            return GetAll();
        }

        string AddPhoto(IFormFile file)
        {
            string uploadPath = configuration.GetSection("Upload-Path").Get<string>() ?? throw new Exception("Upload Path Doesn't Exists in appsettings.json");
            string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), uploadPath);
            string path = Path.Combine(directoryPath, file.FileName);

            FileStream st = new(path, FileMode.Create, FileAccess.Write);
            file.CopyTo(st);

            return path;
        }

        void RemovePhoto(string path)
        {
            if (path is not null && System.IO.File.Exists(path))
                System.IO.File.Delete(path);
        }
    }
}
