using DataHandlerMongoDB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentAnalyzerAPI.Models
{
    public class UserDocument
    {
        public UserDocument(string name, string docId, bool status)
        {
            Title = name;
            Status = status;
            DocumentId = docId;
            UserDocumentReferences = new List<Reference>();
        } 

        public string Title { get; set; }
        public bool Status { get; set; }
        public string DocumentId { get; set; }

        public List<Reference> UserDocumentReferences { get; set; }
    }
}
