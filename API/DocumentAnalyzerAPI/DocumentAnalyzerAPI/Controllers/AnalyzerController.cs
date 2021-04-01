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
            /* Receives a JSON [body] = {"owner": Integer, 
             *                    "url":"https://soafiles.blob.core.windows.net/files/..."}
             * 
             * 
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
            string nlpResult = NLPService.NLPController.AnalyzeDocument(data.Url, data.Owner); // Database insertion of result is done within the NLP Service.
            //FileMongo result = JsonConvert.DeserializeObject<FileMongo>(nlpResult);
            //mongo_repository.InsertOne(result);
            return Ok(nlpResult);
        }

        [HttpGet, Route("/documents/user={user_id}/")]
        public IActionResult GetUserDocuments([FromRoute(Name = "user_id")] int id)
        {
            // Finds documents associated with employee
            List<UserDocument> userFiles = EmployeeFinder.FindEmployeeDocuments(id, mongo_repository);
            // Serialize results into a JSON string
            string jsonFiles = System.Text.Json.JsonSerializer.Serialize(userFiles);
            /*
             * Returns JSON File = [{"Title":..., "Status":...}, {"Title":..., "Status":...}, ...]
             */
            return Ok(jsonFiles);
        }

        [HttpGet, Route("/documents/results/")]
        public IActionResult GetProcessingResult()
        {
            /*
             * Receives body JSON = {"Owner":Integer, "Title": String}
             * 
             * */

            AllowSync();

            var body = String.Empty;

            // reads request's body
            using (var reader = new StreamReader(Request.Body))
            {
                body = reader.ReadToEnd();
            }

            ResultRequest req = System.Text.Json.JsonSerializer.Deserialize<ResultRequest>(body);

            List<Match> processingResults = EmployeeFinder.FindEmployeeReferences(req, mongo_repository, unit_of_work);
            string jsonResult = System.Text.Json.JsonSerializer.Serialize(processingResults);

            EmployeeFinder.AddUserReferences(processingResults, req, unit_of_work);
            /*
             * Returns JSON = [{"Name":String, "Qty":Integer, "employeeId": Integer}, {"Name":String, "Qty":Integer, "employeeId": Integer}, ...]
             * */
            return Ok(jsonResult);
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
