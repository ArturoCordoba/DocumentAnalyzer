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
        private static IUnitOfWorkFactory uow_factory;

        public AnalyzerController(IMongoRepositoryFactory factory, IUnitOfWorkFactory unit_factory)
        {
            mongo_repository = factory.Create<FileMongo>();
            uow_factory = unit_factory;
        }

        [HttpPost, Route("/documents/notify/")]
        public IActionResult NotifyDocument(NotificationData data)
        {
            /* Receives a JSON [body] = {"url":"https://soafiles.blob.core.windows.net/files/...",
             *                           "title": String
             *                           "owner": Integer}
             *                           
             * Owner: Integer (from header token)
             */

            // Process document
            NLPService.NLPController.Instance.AddDocument(data.Url, data.Owner.ToString());
            //FileMongo result = JsonConvert.DeserializeObject<FileMongo>(nlpResult);
            //mongo_repository.InsertOne(result);
            string jsonResult = String.Empty;

            List<Match> processingResults = new List<Match>();

            IUnitOfWork unit_of_work = uow_factory.Create();

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
            IUnitOfWork unit_of_work = uow_factory.Create();

            List<UserDocument> userFiles = EmployeeFinder.FindEmployeeDocuments(id, mongo_repository);
            string result = EmployeeFinder.GetDocumentReferences(userFiles, unit_of_work);

            /* Returns JSON [{"Title": String,"Status": Boolean,"DocumentId": String,"UserDocumentReferences":[{"Name":String,"Qty":Integer},{"Name":String,"Qty":Integer}, ...]}]
             */
            return Ok(result);
        }

        [HttpGet, Route("/documents/users/count")]
        public IActionResult GetUserGlobalCount()
        {
            IUnitOfWork unit_of_work = uow_factory.Create();

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
