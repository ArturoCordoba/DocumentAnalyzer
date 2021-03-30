using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using DocumentAnalyzerAPI.Models;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Http.Features;

namespace DocumentAnalyzerAPI.Controllers
{
    public class AnalyzerController : Controller
    {
        [HttpPost, Route("/documents/notify/")]
        public IActionResult NotifyDocument() 
        {
            /* Receives a JSON [body] = {"owner":"SOME OWNER", 
             *                    "url":"https://soafiles.blob.core.windows.net/files/..."}
             * 
             * Authentication data comes from header.
             */

            // Necessary to read the request's body
            AllowSync();

            var body = String.Empty;

            // reads request's body
            using (var reader = new StreamReader(Request.Body))
            {
                body = reader.ReadToEnd();
            }

            // serialize body into NotificationData Object
            NotificationData data = JsonConvert.DeserializeObject<NotificationData>(body);

            // Process document
            string result = NLPService.NLPController.AnalyzeDocument(data.Url, data.Owner);

            // Deserialize result for easier handling
            NLPResult NlpResult = JsonConvert.DeserializeObject<NLPResult>(result);

            // todo: use employee finder to update global counters 

            // todo: pass result to data handler to store result.

            return Ok();
        }

        [HttpGet, Route("/documents/status/")]
        public IActionResult GetDocumentStatus()
        {
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

        private void AllowSync()
        {
            var syncIOFeature = HttpContext.Features.Get<IHttpBodyControlFeature>();

            if (syncIOFeature != null)
            {
                syncIOFeature.AllowSynchronousIO = true;
            }
        }
    }
}
