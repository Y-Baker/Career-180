using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.DTOs.Products;
using WebApi.UnitOfWorks;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaController : ControllerBase
    {
        const string mimeType = "image/jpeg";
        string uploadPath;
        string basePath;
        public MediaController(IConfiguration configuration)
        {
            string upload = configuration.GetSection("Upload-Path").Get<string>() ?? throw new Exception("Upload Path Doesn't Exists in appsettings.json");

            basePath = Directory.GetParent(AppContext.BaseDirectory)?.Parent?.Parent?.Parent?.FullName ?? throw new Exception("Error in Find Base Directory");
            uploadPath = Path.Combine(basePath, upload);
        }

        [SwaggerOperation("View Default Image", "Return a Default Image Named gojo.jpg")]
        [SwaggerResponse(200, "Successfully", typeof(PhysicalFileResult))]
        [SwaggerResponse(404, "Failed, File not found.")]
        [HttpGet]
        public IActionResult ViewMedia()
        {
            string filePath = Path.Combine(uploadPath, "gojo.jpg");
            //string filepath = @"c:\users\asus\programing\career 180\assignments\asp.net\day5\webapi\uploads\gojo.jpg";

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            return PhysicalFile(filePath, mimeType);
        }


        [SwaggerOperation("View Image By Name", "Return a Image Based on Name from upload folder")]
        [SwaggerResponse(200, "Successfully", typeof(PhysicalFileResult))]
        [SwaggerResponse(404, "Failed, File not found.")]
        [HttpGet("{id}")]
        public IActionResult ViewProductImage(string id)
        {
            string filePath = Path.Combine(uploadPath, id);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("File not found.");
            }

            return PhysicalFile(filePath, mimeType);
        }
    }
}
