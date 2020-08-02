using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SandalsApi.Core.Data;
using SandalsApi.Core.Models;
using SandalsApi.Core.Utilities;

namespace SandalsApi.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        [HttpPost]
        [Route("login")]
        public IActionResult Login(User user) 
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (user.username != null && user.password != null) 
                {
                    if (PasswordManager.Authenticate(new string[] { user.username, user.password }))
                    {
                        User validatedUser = Database.GetUser(user.username, false);
                        response.message = "Credentials verified";
                        response.result.username = validatedUser.username;
                        response.result.firstName = validatedUser.firstName;
                        response.result.lastName = validatedUser.lastName;
                        response.result.emailAddress = validatedUser.email;
                        response.result.address = validatedUser.address;
                        response.result.country = validatedUser.country;
                        response.result.phone = validatedUser.phone;
                        response.result.userType = validatedUser.userType;
                        return StatusCode(StatusCodes.Status200OK, response);
                    }
                }
                response.message = "Invalid credentials";
                response.result = "access_denied";
                return StatusCode(StatusCodes.Status400BadRequest, response);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register(User user) 
        {
            ApiResponse response = new ApiResponse();
            try
            {
                string validationResult = user.ValidateRegistrationFields(user);
                if (validationResult == "")
                {
                    if (!Database.CheckUsernameExist(user.username))
                    {
                        if (!Database.CheckEmailExist(user.email))
                        {
                            user.hash = PasswordManager.GetHash(user.password);
                            Database.CreateUser(user);
                            response.message = "User Created Successfuly";
                            response.result = "account_created";
                            return StatusCode(StatusCodes.Status200OK, response);
                        }
                        else
                        {
                            response.message = "Email address already registered";
                            response.result = "registration_failed";
                            return StatusCode(StatusCodes.Status400BadRequest, response);
                        }
                    }
                    else
                    {
                        response.message = "Username already registered";
                        response.result = "registration_failed";
                        return StatusCode(StatusCodes.Status400BadRequest, response);
                    }
                }
                else
                {
                    response.message = "Invalid/Missing Fields";
                    response.result = validationResult;
                    return StatusCode(StatusCodes.Status400BadRequest, response);
                }
            }
            catch (Exception e)
            {
                response.message = e.Message;
                response.result.stacktrace = e.StackTrace;
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpGet]
        [Route("users")]
        [Authorize]
        public IActionResult Users()
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (HttpContext.User.FindFirst(ClaimTypes.Role) != null && HttpContext.User.FindFirst(ClaimTypes.Role).Value == "Administrator")
                {
                    response.message = "List of all users registered in Sandals DB";
                    response.result.records = Database.GetUsers(false);
                    return StatusCode(StatusCodes.Status200OK, response);
                }

                response.message = "Unauthorized api usage";
                response.result.records = "access_denied";
                return StatusCode(StatusCodes.Status401Unauthorized, response);
            }
            catch (Exception e) 
            {
                Logger.Log(e);
                response.message = "An error has occurred while processing your request";
                response.result.stacktrace = e.StackTrace;
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }
        

        [HttpPost]
        [Authorize]
        [Route("changepassword")]
        public IActionResult ChangePassword(Credentials credentials) 
        {
            ApiResponse response = new ApiResponse();
            if (credentials != null && credentials.username != null && credentials.password != null && credentials.newPassword != null)
            {
                if (!PasswordManager.CheckPasswordRequirement((string)credentials.newPassword)) 
                {
                    response.message = "Password does not meet the requirements.";
                    response.result = "request_failed";
                    return StatusCode(StatusCodes.Status400BadRequest, response);
                }

                try
                {
                    if (HttpContext.User.FindFirst(ClaimTypes.Name).Value == (string)credentials.username)
                    {
                        User user = Database.GetUser((string)credentials.username, true);
                        if (PasswordManager.Authenticate(new string[] { credentials.username, credentials.password }))
                        {
                            Database.UpdatePassword(user.username, PasswordManager.GetHash((string)credentials.newPassword));
                            response.message = "Password changed successfully";
                            response.result = "password_updated";
                            return StatusCode(StatusCodes.Status200OK, response);
                        }
                        else
                        {
                            response.message = "Could not verify your credentials";
                            response.result = "request_failed";
                            return StatusCode(StatusCodes.Status400BadRequest, response);
                        }
                    }
                    response.message = "Could not verify your credentials";
                    response.result = "request_failed";
                    return StatusCode(StatusCodes.Status400BadRequest, response);
                }
                catch (Exception e)
                {
                    response.message = "An error has occurred while processing your request";
                    response.result.stacktrace = e.StackTrace;
                    return StatusCode(StatusCodes.Status500InternalServerError, response);
                }
            }
            else 
            {
                response.message = "Missing fields";
                response.result = "request_failed";
                return StatusCode(StatusCodes.Status400BadRequest, response);
            }
        }

        [HttpPost]
        [Route("reset")]
        public IActionResult Reset([FromBody]string emailAddress) 
        {
            ApiResponse response = new ApiResponse();
            try
            {
                User user = Database.GetUserByEmail(emailAddress, false);
                if (user == null) 
                {
                    response.message = "This account does not exist";
                    response.result = "reset_failed";
                    return StatusCode(StatusCodes.Status400BadRequest, response);
                }
                Database.SetAccountRecoveryState(user.userId, true);
                //send recovery code here

                response.message = "Recovery code will be sent to your email address";
                response.result = "reset_sucessfull";
                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception e) 
            {
                Logger.Log(e);

                response.message = "An error has occurred while processing your request";
                response.result = "reset_failed";
                return StatusCode(StatusCodes.Status400BadRequest, response);
            }
        }

        [HttpGet]
        [Route("dashboard")]
        public IActionResult Dashboard() 
        {
            return Ok();
        }

        public class Credentials
        {
            public string username { get; set; }
            public string password { get; set; }
            public string newPassword { get; set; }
        }
    }
}
