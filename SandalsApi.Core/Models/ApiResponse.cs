using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace SandalsApi.Core.Models
{
    public class ApiResponse
    {
        public ApiResponse()
        {
            result = new ExpandoObject();
            message = "";
        }
        public string message { get; set; }
        public dynamic result { get; set; }
    }
}
