using SandalsApi.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SandalsApi.Core.Models
{
    public class OrderMain
    {
        private const int SHORT_DESC_LENGTH = 15;
        public int orderMainId { get; set; }
        public int userId { get; set; }
        public DateTime createdDate { get; set; }
        public string shortDescription { get; set; }
        public decimal total { get; set; }
        public List<Dictionary<string, string>> uploadedImages { get; set; }
        public List<OrderDetail> orderDetails { get; set; }
        public string orderStatus { get; set; }
        public User user { get; set; }

        public string GetShortDescription()
        {
            string result = "";
            if (orderDetails != null)
            {
                for (int i = 0; i < orderDetails.Count; i++)
                {
                    result += Database.GetProduct(orderDetails[i].productId).description + ", ";
                }
                result = result.TrimEnd(new char[] { ' ', ',' });
                if (result.Length > SHORT_DESC_LENGTH)
                {
                    result = result.Substring(0, Math.Min(result.Length, SHORT_DESC_LENGTH));
                    result += "...";
                }
                return result;
            }
            return "";
        }
        public decimal GetTotal()
        {
            decimal total = 0;
            for (int i = 0; i < orderDetails.Count; i++)
            {
                total += orderDetails[i].retailPrice * orderDetails[i].quantity;
            }
            return total;
        }
    }
}
