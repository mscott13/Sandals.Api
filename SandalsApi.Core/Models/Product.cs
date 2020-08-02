using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SandalsApi.Core.Models
{
    public class Product
    {
        public int productId { get; set; }
        public DateTime createdDate { get; set; }
        public string description { get; set; }
        public string image { get; set; }
        public decimal retailPrice { get; set; }
        public string mediaType { get; set; }
        public string extension { get; set; }
    }
}
