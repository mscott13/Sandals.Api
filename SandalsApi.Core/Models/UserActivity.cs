using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SandalsApi.Core.Models
{
    public class UserActivity
    {
        public UserActivity() 
        {
            currentlyActiveUsers = 0;
            userActivityCounters = new Dictionary<string, int>();
            logEvents = new Dictionary<string, List<LogItem>>();
        }
        public int currentlyActiveUsers { get; set; }
        public Dictionary<string, int> userActivityCounters { get; set; }
        public Dictionary<string, List<LogItem>> logEvents { get; set; }
    }
}
