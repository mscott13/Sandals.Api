using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SandalsApi.Core.Data;
using SandalsApi.Core.Models;
using SandalsApi.Core.Utilities;

namespace SandalsApi.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        IWebHostEnvironment _env;
        public ImageController(IWebHostEnvironment env) 
        {
            _env = env;
        }

        [HttpGet("product/{id}")]
        public IActionResult Product(int id) 
        {
            ApiResponse response = new ApiResponse();
            try
            {
                Product product = Database.GetProduct(id);
                string root = Path.Combine(_env.ContentRootPath, @"images\products");
                string path = Path.Combine(root, product.image);
                var stream = new FileStream(path, FileMode.Open);
                return new FileStreamResult(stream, product.mediaType);
            }
            catch (Exception e) 
            {
                response.message = "Image not found";
                response.result = "get_failed";
                Logger.Log(e);
                return StatusCode(StatusCodes.Status404NotFound, response);
            }
        }

        [HttpGet("order/{image}")]
        public IActionResult Order(string image) 
        {
            ApiResponse response = new ApiResponse();
            try
            {
                string root = Path.Combine(_env.ContentRootPath, @"images\orders");
                string path = Path.Combine(root, image);
                var stream = new FileStream(path, FileMode.Open);
                return new FileStreamResult(stream, Database.GetUploadedImage(image)["mediaType"]);
            }
            catch (Exception e) 
            {
                response.message = "Image not found";
                response.result = "get_failed";
                Logger.Log(e);
                return StatusCode(StatusCodes.Status404NotFound, response);
            }
        }
    }
}
