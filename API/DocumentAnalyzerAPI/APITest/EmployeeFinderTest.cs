using Microsoft.VisualStudio.TestTools.UnitTesting;
using DocumentASnalyzerAPI.Models;
using DataHandlerSQL.Repository;
using DataHandlerMongoDB.Repository;
using DocumentAnalyzerAPI.Models;

namespace APITest
{
    [TestClass]
    public class EmployeeFinderTest
    {
        [TestMethod]
        public void FindEmployees()
        {
            NotificationData data = new NotificationData();
            data.Owner = 69;
            data.Url = "https://soafiles.blob.core.windows.net/files/...";
            data.Title = "UnitTesting.txt";


        }
    }
}
