using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

using DataHandlerMongoDB.Configuration;
using DataHandlerMongoDB.Factory;
using DataHandlerMongoDB.Repository;
using DataHandlerMongoDB.Model;

namespace Test
{
    class MongoDB
    {
        static void Main(string[] args)
        {
            DataHandlerMongoDBConfig.Config.ConnectionString = "mongodb://localhost:27017";
            DataHandlerMongoDBConfig.Config.DataBaseName = "DocAnalyzer";

            IMongoRepositoryFactory repositoryFactory = new MongoRepositoryFactory();

            IMongoRepository<File> repository = repositoryFactory.Create<File>();

            /*File file = new File();
            file.Title = "Introduction to MongoDb with .NET";
            file.Owner = 4;
            file.Url = "http://www.google.co.cr";

            repository.InsertOne(file);*/

            string title = "Introduction to MongoDb with .NET";

            File file = repository.FindOne(file => file.Title == title);

            Console.WriteLine(file.Id);

            file.Owner = 10;

            repository.ReplaceOne(file);
        }
    }
}
