using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using SandalsApi.Core.Data;
using SandalsApi.Core.Models;
using SandalsApi.Core.Other;
using SandalsApi.Core.Utilities;

namespace SandalsApi.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        IWebHostEnvironment _env;
        private readonly IHubContext<NotificationHub> _hubContext;
        public OrderController(IWebHostEnvironment env, IHubContext<NotificationHub> hubContext) 
        {
            _env = env;
            _hubContext = hubContext;
        }

        [HttpPost]
        [Route("create")]
        public IActionResult Create([FromForm] MultipartOrder order) 
        {
            ApiResponse response = new ApiResponse();
            List<Dictionary<string, string>> images = new List<Dictionary<string, string>>();
            try
            {
                if (order.images == null) 
                {
                    response.message = "At least 1 image required to create order";
                    response.result = "create_failed";
                    return StatusCode(StatusCodes.Status400BadRequest, response);
                }

                if (order.json == null)
                {
                    response.message = "Order data missing/invalid";
                    response.result = "create_failed";
                    return StatusCode(StatusCodes.Status400BadRequest, response);
                }

                OrderMain _order = JsonConvert.DeserializeObject<OrderMain>(order.json);
                string root = Path.Combine(_env.ContentRootPath, @"images\orders");

                if (!Directory.Exists(root))
                    Directory.CreateDirectory(root);

                foreach (var file in order.images) 
                {
                    if (file.ContentType != "image/png" && file.ContentType != "image/jpeg") 
                    {
                        response.message = "Png/Jpeg images only";
                        response.result = "create_failed";
                        return StatusCode(StatusCodes.Status400BadRequest, response);
                    }

                    string filename = Guid.NewGuid().ToString();
                    using (var fileStream = new FileStream(Path.Combine(root, filename), FileMode.Create, FileAccess.Write))
                    {
                        file.CopyTo(fileStream);
                        Dictionary<string, string> values = new Dictionary<string, string>();
                        values.Add("image", filename);
                        values.Add("mediaType", file.ContentType);
                        values.Add("path", Path.Combine(root, filename));
                        values.Add("extension", Path.GetExtension(file.FileName));
                        images.Add(values);
                    }
                }
                _order.uploadedImages = images;
                Database.CreateOrder(_order);
                response.message = "Order created successfully";
                response.result = "order_created";
                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception e) 
            {
                RemoveImages(images);
                response.message = "An exception occurred while creating order";
                response.result = "create_failed";
                Logger.Log(e);
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id) 
        {
            ApiResponse response = new ApiResponse();
            try
            {
                OrderMain order = Database.GetOrder(id);
                if (order != null) 
                {
                    response.message = "Order for Id: " + id;
                    response.result = order;
                    return StatusCode(StatusCodes.Status200OK, response);
                }
                response.message = "Invalid order id";
                response.result = "retrieval_failed";
                return StatusCode(StatusCodes.Status400BadRequest, response);
            }
            catch (Exception e) 
            {
                response.message = "Could not retrieve order, an error has occurred";
                response.result = "cancel_failed";
                Logger.Log(e);
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpGet("cancel/{id}")]
        public IActionResult Cancel(int id)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                OrderMain order = Database.GetOrder(id);
                if (order == null) 
                {
                    response.message = "Order does not exist";
                    response.result = "cancel_failed";
                    return StatusCode(StatusCodes.Status400BadRequest, response);
                }

                if (order.orderStatus == Other.Constants.ORDER_STATUS_PROCESSED || order.orderStatus == Other.Constants.ORDER_STATUS_PROCESSING) 
                {
                    response.message = "Cannot cancel an order that is "+order.orderStatus;
                    response.result = "cancel_failed";
                    return StatusCode(StatusCodes.Status400BadRequest, response);
                }

                Database.SetOrderStatus(id, Other.Constants.ORDER_STATUS_CANCELLED);
                response.message = "Order cancelled successfully";
                response.result = "order_cancelled";
                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception e) 
            {
                response.message = "Could not cancel order, an error has occurred";
                response.result = "cancel_failed";
                Logger.Log(e);
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpGet]
        [Route("all")]
        public IActionResult Orders()
        {
            Send("Get orders api called");
            ApiResponse response = new ApiResponse();
            try
            {
                if (HttpContext.User.FindFirst(ClaimTypes.Role) != null && HttpContext.User.FindFirst(ClaimTypes.Role).Value == "Administrator")
                {
                    response.message = "All orders";
                    response.result = Database.GetOrders();
                    return StatusCode(StatusCodes.Status200OK, response);
                }

                response.message = "Unauthorized api usage";
                response.result = "access_denied";
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            catch (Exception e) 
            {
                Logger.Log(e);
                response.message = "An error has occurred while processing your request";
                response.result = "retrieve_failed";
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        private async void Send(string message) 
        {
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", message);
        }

        private void RemoveImages(List<Dictionary<string, string>> images) 
        {
            foreach (var image in images) 
            {
                try
                {
                    string path = image["path"];
                    if (System.IO.File.Exists(path)) 
                    {
                        System.IO.File.Delete(path);
                    }
                }
                catch (Exception e) 
                {
                    Logger.Log(e);
                }
            }
        }
    }
}
