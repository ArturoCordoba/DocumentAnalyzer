using System;
using System.Collections.Generic;


namespace NLPService
{
    public class Blob
    {
        // Blob class constructor
        public Blob(string title, string url, List<string> references, string owner, bool status) 
        {
            Title = title;
            Url = url;
            References = references;
            Owner = owner;
            Status = status;
        }

        // Atribbutes getters and setters
        public string Title { get; set; }
        public string Url { get; set; }
        public List<string> References { get; set; }
        public string Owner { get; set; }
        public bool Status { get; set; }
    }
}
