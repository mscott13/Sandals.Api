using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SandalsApi.Core.Models
{
    public class Dashboard
    {
        public decimal thisMonthTotal { get; set; }
        public decimal prevMonthTotal { get; set; }
        public decimal difference { get; set; }
        public List<Dictionary<string, decimal>> monthlyTotals { get; set; }
        public Dictionary<string, int> generalStatistics { get; set; }
        public List<OrderMain> orders { get; set; }
        public UserActivity userActivity { get; set; }
    }
}
