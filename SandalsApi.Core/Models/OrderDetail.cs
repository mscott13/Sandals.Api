using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SandalsApi.Core.Models
{
    public class OrderDetail
    {
        public int orderMainId { get; set; }
        public int productId { get; set; }
        public string productDescription { get; set; }
        public decimal retailPrice { get; set; }
        public int quantity { get; set; }
        public string image { get; set; }
    }
}
