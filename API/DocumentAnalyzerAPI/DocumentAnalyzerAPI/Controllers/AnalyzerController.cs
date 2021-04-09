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
using DataHandlerMongoDB.Factory;
using DataHandlerMongoDB.Repository;
using DataHandlerMongoDB.Model;
using FileMongo = DataHandlerMongoDB.Model.File;
using DataHandlerSQL.Factory;
using DataHandlerSQL.Repository;
using DataHandlerSQL.Model;
using DocumentASnalyzerAPI.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DocumentAnalyzerAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AnalyzerController : Controller
    {
        private static IMongoRepository<FileMongo> mongo_repository;
        private static IUnitOfWork unit_of_work;

        public AnalyzerController(IMongoRepositoryFactory factory, IUnitOfWorkFactory unit_factory)
        {
            mongo_repository = factory.Create<FileMongo>();
            unit_of_work = unit_factory.Create();
        }

        [HttpPost, Route("/documents/notify/")]
        public IActionResult NotifyDocument()
        {
            /* Receives a JSON [body] = {"url":"https://soafiles.blob.core.windows.net/files/...",
             *                           "title": String
             *                           "owner": Integer}
             *                           
             * Owner: Integer (from header token)
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
            NLPService.NLPController.Instance.AddDocument(data.Url, data.Owner.ToString());
            //FileMongo result = JsonConvert.DeserializeObject<FileMongo>(nlpResult);
            //mongo_repository.InsertOne(result);
            string jsonResult = String.Empty;

            List<Match> processingResults = new List<Match>();

            while(processingResults.Count == 0)
            {
                processingResults = EmployeeFinder.FindEmployeeReferences(data, mongo_repository, unit_of_work);
                jsonResult = System.Text.Json.JsonSerializer.Serialize(processingResults);
            }
                
            EmployeeFinder.AddUserReferences(processingResults, unit_of_work);

            return Ok(jsonResult);
        }

        

        [HttpGet, Route("/documents/user={user_id}/")]
        public IActionResult GetUserDocuments([FromRoute(Name = "user_id")] int id)
        {
            List<UserDocument> userFiles = EmployeeFinder.FindEmployeeDocuments(id, mongo_repository);
            string result = EmployeeFinder.GetDocumentReferences(userFiles, unit_of_work);

            /* Returns JSON [{"Title": String,"Status": Boolean,"DocumentId": String,"UserDocumentReferences":[{"Name":String,"Qty":Integer},{"Name":String,"Qty":Integer}, ...]}]
             */
            return Ok(result);
        }

        [HttpGet, Route("/documents/users/count")]
        public IActionResult GetUserGlobalCount()
        {
            List<EmployeeCount>  counts = EmployeeFinder.GetEmployeeGlobalCounter(unit_of_work);
            return Ok(System.Text.Json.JsonSerializer.Serialize(counts));
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
