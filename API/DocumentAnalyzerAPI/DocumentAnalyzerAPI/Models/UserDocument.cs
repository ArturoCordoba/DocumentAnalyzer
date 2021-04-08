using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentAnalyzerAPI.Models
{
    public class UserDocument
    {
        public UserDocument(string name, bool status)
        {
            Title = name;
            Status = status;
        } 

        public string Title { get; set; }
        public bool Status { get; set; }
    }
}
