using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentAnalyzerAPI.Models
{
    public class EmployeeCount
    {
        public EmployeeCount(string name, int? ctr)
        {
            EmployeeName = name;
            Count = ctr;
        }
        public string EmployeeName { get; set; }
        public int? Count { get; set; }
    }
}
