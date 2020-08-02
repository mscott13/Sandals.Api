using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SandalsApi.Core.Models
{
    public class LogItem
    {
        public string message { get; set; }
        public DateTime date { get; set; }
        public string dateFormatted { get;set; }
    }
}
