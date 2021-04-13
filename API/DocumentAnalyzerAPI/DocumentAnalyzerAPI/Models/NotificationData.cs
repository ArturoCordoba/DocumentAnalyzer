using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentAnalyzerAPI.Models
{
    public class NotificationData
    {
        public int Owner { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
    }
}
