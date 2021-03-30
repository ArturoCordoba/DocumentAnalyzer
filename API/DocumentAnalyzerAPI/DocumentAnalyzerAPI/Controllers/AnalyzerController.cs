using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentAnalyzerAPI.Controllers
{
    public class AnalyzerController : Controller
    {
        [HttpPost, Route("/documents/notify/documentId={id}/")]
        public IActionResult NotifyDocument([FromRoute(Name = "id")] string id) 
        {
            // connect to cloud storage
            // locate document
            // connect to data employee finder
            // process document
            // store result on NoSQL database
            return Ok();
        }

        [HttpGet, Route("/documents/status/")]
        public IActionResult GetDocumentStatus()
        {
            // check which document is currently processing
            // if is still processing report status = IN PROCESS
            // Otherwise report = READY
            return Ok();
        }

        [HttpGet, Route("/documents/user={user_id}/")]
        public IActionResult GetUserDocuments([FromRoute(Name = "user_id")] string id)
        {
            // use Employee Finder (?) to look for the documents of the user through the data handler.
            // Build JSON with documents and return them.
            return Ok();
        }

        [HttpGet, Route("/documents/results/documentId={id}/")]
        public IActionResult GetProcessingResult([FromRoute(Name = "id")] string id)
        {
            // use Employee Finder (?) to look for the documents of the user through the data handler.
            // Build JSON with documents and return them.
            return Ok();
        }
    }
}
