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
        string uploadPath;
        string basePath;
        const string mediaEndPoint = $"api/media";
        const string mimeType = "image/jpeg";

        public ProductsController(UnitOfWork unit, IConfiguration configuration)
        {
            db = unit;
            string upload = configuration.GetSection("Upload-Path").Get<string>() ?? throw new Exception("Upload Path Doesn't Exists in appsettings.json");

            basePath = Directory.GetParent(AppContext.BaseDirectory)?.Parent?.Parent?.Parent?.FullName ?? throw new Exception("Error in Find Base Directory");
            uploadPath = Path.Combine(basePath, upload);
        }

        private ProductDTO ProductToDTO(Product product, string baseUrl)
        {
            string photoUrl = $"{baseUrl}/{mediaEndPoint}/{product.PhotoId}";

            ProductDTO productDTO = new ProductDTO()
            {
                ProductId = product.Id,
                ProductName = product.Name,
                Description = product.Description,
                Price = product.Price,
                Amount = product.Amount,
                CategoryName = product.Category?.Name ?? "No Category",
                PhotoUri = new Uri(photoUrl)
            };

            return productDTO;
        }
        private Product ToProduct(AddProductDTO productDTO, string? id)
        {
            Product product = new Product()
            {
                Name = productDTO.ProductName,
                Description = productDTO.Description,
                Price = productDTO.Price,
                Amount = productDTO.Amount,
                CategoryId = productDTO.CategoryId,
                Category = db.CategoryRepo.SelectById(productDTO.CategoryId),
                PhotoId = id 
            };

            return product;
        }
        private Product ToProduct(EditProductDTO productDTO, string? id)
        {
            Product product = new Product()
            {
                Id = productDTO.ProductId,
                Name = productDTO.ProductName,
                Description = productDTO.Description,
                Price = productDTO.Price,
                Amount = productDTO.Amount,
                CategoryId = productDTO.CategoryId,
                Category = db.CategoryRepo.SelectById(productDTO.CategoryId),
                PhotoId = id
            };

            return product;
        }


        [SwaggerOperation("Get All Products", "Return a list of all products in the system.")]
        [SwaggerResponse(200, "Successfully", typeof(List<ProductDTO>))]
        [Produces("application/json")]
        [HttpGet]
        public IActionResult GetAll()
        {
            string baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";

            List<Product> products = db.ProductRepo.SelectAll("Category");

            List<ProductDTO> productsDTO = new List<ProductDTO>();

            foreach (Product product in products)
            {
                productsDTO.Add(ProductToDTO(product, baseUrl));
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
            string baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";

            Product? product = db.ProductRepo.SelectById(id, "Category");

            if (product == null)
                return NotFound();

            return Ok(ProductToDTO(product, baseUrl));
        }


        [SwaggerOperation("View Image Of Product", "Return a Image Based on Product Id")]
        [SwaggerResponse(200, "Successfully", typeof(PhysicalFileResult))]
        [SwaggerResponse(404, "Failed, Product Not Found or File Not Found.")]
        [HttpGet("{id}/image")]
        public IActionResult GetImgByProductId(int id)
        {
            Product? product = db.ProductRepo.SelectById(id, "Category");

            if (product == null || product.PhotoId is null)
                return NotFound();

            string filePath = Path.Combine(uploadPath, product.PhotoId);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("File not found.");
            }

            return PhysicalFile(filePath, mimeType);
        }


        [SwaggerOperation("Get Products by Price", "Return a list of products that their price in the range givin as query (maxPrice, minPrice).")]
        [SwaggerResponse(200, "Successfully", typeof(List<ProductDTO>))]
        [Produces("application/json")]
        [HttpGet("price")]
        public IActionResult GetByPrice([FromQuery] int maxPrice, [FromQuery] int minPrice = 0)
        {
            string baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";

            List<Product> products = db.ProductRepo.SelectByPrice(minPrice, maxPrice, "Category");

            List<ProductDTO> productsDTO = new List<ProductDTO>();

            foreach (Product product in products)
            {
                productsDTO.Add(ProductToDTO(product, baseUrl));
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

            string baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
            string? photoId = null;
            if (productDTO.Photo is not null)
                photoId = AddPhoto(productDTO.Photo);

            Product product = ToProduct(productDTO, photoId);

            db.ProductRepo.Add(product);
            db.Save();

            ProductDTO productAdded = ProductToDTO(product, baseUrl);

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

            string baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";

            string? photoId = productBefore.PhotoId;
            string? newPhotoId = null;

            if (productDTO.Photo is not null)
            {
                if (productBefore.PhotoId is not null)
                    RemovePhoto(productBefore.PhotoId);
                newPhotoId = AddPhoto(productDTO.Photo);
            }
            Product product = ToProduct(productDTO, newPhotoId ?? productBefore.PhotoId);

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
            Guid guid = Guid.NewGuid();
            string fileExtension = Path.GetExtension(file.FileName);
            string fileName = $"{guid.ToString()}{fileExtension}";
            string path = Path.Combine(uploadPath, fileName);

            FileStream st = new(path, FileMode.Create, FileAccess.Write);
            file.CopyTo(st);

            return fileName;
        }

        void RemovePhoto(string id)
        {
            string path = Path.Combine(uploadPath, id.ToString());
            if (path is not null && System.IO.File.Exists(path))
                System.IO.File.Delete(path);
        }
    }
}
