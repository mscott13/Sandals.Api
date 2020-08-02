using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using SandalsApi.Core.Models;
using SandalsApi.Core.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Net.Http;
using SandalsApi.Core.Utilities;
using Newtonsoft.Json;
using System.IO.Compression;
using System.Net.Mime;

namespace SandalsApi.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private IWebHostEnvironment _env;
        public AdminController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpPost]
        [Authorize]
        [Route("product")]
        public IActionResult Product([FromForm] MultipartProduct product)
        {
            ApiResponse response = new ApiResponse();
            if (HttpContext.User.FindFirst(ClaimTypes.Role) != null && HttpContext.User.FindFirst(ClaimTypes.Role).Value == "Administrator")
            {
                try
                {
                    if (product.image == null)
                    {
                        response.message = "Image required for a new product";
                        response.result = "create_failed";
                        return StatusCode(StatusCodes.Status400BadRequest, response);
                    }

                    if (product.json == null)
                    {
                        response.message = "Product data is missing/invalid";
                        response.result = "create_failed";
                        return StatusCode(StatusCodes.Status400BadRequest, response);
                    }

                    Product _product = JsonConvert.DeserializeObject<Product>(product.json);
                    string root = Path.Combine(_env.ContentRootPath, @"images\products");
                    string filename = Guid.NewGuid().ToString();

                    if (!Directory.Exists(root))
                        Directory.CreateDirectory(root);

                    using (var fileStream = new FileStream(Path.Combine(root, filename), FileMode.Create, FileAccess.Write))
                    {
                        product.image.CopyTo(fileStream);
                    }

                    _product.image = filename;
                    _product.mediaType = product.image.ContentType;
                    _product.extension = Path.GetExtension(product.image.FileName);
                    Database.CreateProduct(_product);

                    response.message = "Product was created successfully";
                    response.result = "product_created";
                    return StatusCode(StatusCodes.Status201Created, response);

                }
                catch (Exception e)
                {
                    Logger.Log(e);
                    response.message = e.Message;
                    response.result = "create_failed";
                    return StatusCode(StatusCodes.Status500InternalServerError, response);
                }
            }
            response.message = "Unauthorized api usage";
            response.result.records = "access_denied";
            return StatusCode(StatusCodes.Status401Unauthorized, response);
        }

        [HttpGet]
        [Authorize]
        [Route("orders")]
        public IActionResult Orders()
        {
            ApiResponse response = new ApiResponse();
            try
            {
                response.message = "All orders";
                response.result = Database.GetOrders();
                return StatusCode(StatusCodes.Status200OK);
            }
            catch
            {
                response.message = "Internal server error";
                response.result = "get_orders_failed";
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpGet("monthlyTotals/{year}")]
        [Authorize]
        public IActionResult GetMonthlyTotals(int year)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (HttpContext.User.FindFirst(ClaimTypes.Role) != null && HttpContext.User.FindFirst(ClaimTypes.Role).Value == "Administrator")
                {
                    DateTime date = new DateTime(year, DateTime.Now.Month, 1);
                    response.message = "Monthly totals for: " + date.ToShortDateString();
                    response.result = Database.GetMonthlyTotals(date);
                    return StatusCode(StatusCodes.Status200OK, response);
                }
                response.message = "Unauthorized api usage";
                response.result = "access_denied";
                return StatusCode(StatusCodes.Status401Unauthorized, response);
            }
            catch (Exception e)
            {
                Logger.Log(e);
                response.message = "An error has occured while processing your request";
                response.result = "retrieval failed";
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpGet("order/{orderId}/images")]
      
        public IActionResult GetOrderImages(string orderId) 
        {
            var images = Database.GetUploadedImagesForOrder(orderId);
            byte[] archiveFile;
            List<InMemoryFile> files = new List<InMemoryFile>();

            foreach (var image in images)
            {
                string imageName = image["image"];
                string mediaType = image["mediaType"];
                string extension = image["extension"];

                string root = Path.Combine(_env.ContentRootPath, @"images\orders");
                string path = Path.Combine(root, imageName);

                try
                {
                    files.Add(new InMemoryFile
                    {
                        FileName = imageName += extension,
                        Content = System.IO.File.ReadAllBytes(path)
                    });
                }
                catch (Exception e) 
                {
                    Logger.Log(e);
                }
            }

            if (images.Count == 0 || files.Count == 0) 
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }

            using (var archiveStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(archiveStream, ZipArchiveMode.Create, true))
                {
                    foreach (var file in files)
                    {
                        var zipArchiveEntry = archive.CreateEntry(file.FileName, CompressionLevel.Fastest);
                        using (var zipStream = zipArchiveEntry.Open())
                            zipStream.Write(file.Content, 0, file.Content.Length);
                    }
                }

                archiveFile = archiveStream.ToArray();
            }

            return File(archiveFile, MediaTypeNames.Application.Zip, orderId + $"_{DateTime.Now.ToLongDateString()}_images.zip");
        }

        [HttpGet("dashboard")]
        [Authorize]
        public IActionResult Dashboard()
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (HttpContext.User.FindFirst(ClaimTypes.Role) != null && HttpContext.User.FindFirst(ClaimTypes.Role).Value == "Administrator")
                {
                    response.message = "Dashboard data";
                    response.result = Database.GetDashboard();
                    return StatusCode(StatusCodes.Status200OK, response);
                }
                response.message = "Unauthorized api usage";
                response.result = "access_denied";
                return StatusCode(StatusCodes.Status401Unauthorized, response);
            }
            catch (Exception e)
            {
                Logger.Log(e);
                response.message = "An error has occured while processing your request";
                response.result = "retrieval failed";
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpGet("userActivity")]
        [Authorize]
        public IActionResult UserActivity()
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (HttpContext.User.FindFirst(ClaimTypes.Role) != null && HttpContext.User.FindFirst(ClaimTypes.Role).Value == "Administrator")
                {
                    response.message = "User activity";
                    response.result = Database.GetUserActivity();
                    return StatusCode(StatusCodes.Status200OK, response);
                }
                response.message = "Unauthorized api usage";
                response.result = "access_denied";
                return StatusCode(StatusCodes.Status401Unauthorized, response);
            }
            catch (Exception e)
            {
                Logger.Log(e);
                response.message = "An error has occured while processing your request";
                response.result = "retrieval failed";
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        private class InMemoryFile
        {
            public string FileName { get; set; }
            public byte[] Content { get; set; }
        }
    }
}
