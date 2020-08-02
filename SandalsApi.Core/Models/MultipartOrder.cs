using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SandalsApi.Core.Models
{
    public class MultipartOrder
    {
        public List<IFormFile> images { get; set; }
        public string json { get; set; }
    }
}
