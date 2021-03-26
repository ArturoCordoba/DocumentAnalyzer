using Microsoft.VisualStudio.TestTools.UnitTesting;
using DocAnalyzerDataHandler.Repository;
using DocAnalyzerDataHandler;
using System.Collections.Generic;

namespace DocAnalyzerDataHandlerUnitTest
{
    [TestClass]
    public class UnitTestUsercredential
    {
        private IUnitOfWork unitOfWork;

        [TestInitialize]
        public void Initialize()
        {
            string connString = "Server = 127.0.0.1; Port = 5432; Database = DocAnalyzer; User Id = postgres; Password = password;";
            unitOfWork = UnitOfWorkFactory.GetUnitOfWork(connString);
        }

        [TestMethod]
        public void TestInsertAndDelete()
        {
            Usercredential usercredential = new Usercredential();
            usercredential.Email = "test@test#s45f48e4";
            usercredential.UserPassword = "RD=_SF:SF?ESff";
            usercredential.FullName = "Test usercredential #sfjifiesjfei";
            usercredential.UserId = 10000;
            unitOfWork.Usercredentials.Insert(usercredential);
            unitOfWork.Commit();

            unitOfWork.Usercredentials.Delete(10000);
            unitOfWork.Commit();
        }

        [TestMethod]
        public void TestUpdateAndGetById()
        {
            string email = "test@test#s45f48e4";
            Usercredential usercredential = new Usercredential();
            usercredential.Email = email;
            usercredential.UserPassword = "RD=_SF:SF?ESff";
            usercredential.FullName = "Test usercredential #sfjifiesjfei";
            usercredential.UserId = 10000;
            unitOfWork.Usercredentials.Insert(usercredential);
            unitOfWork.Commit();

            string updatedFullName = "Test usercredential updated";
            usercredential.FullName = updatedFullName;
            unitOfWork.Usercredentials.Update(usercredential);
            unitOfWork.Commit();

            Usercredential usercredential1 = unitOfWork.Usercredentials.GetById(10000);
            Assert.AreEqual(usercredential1.FullName, updatedFullName);

            unitOfWork.Usercredentials.Delete(10000);
            unitOfWork.Commit();
        }

        [TestMethod]
        public void TestGetAll()
        {
            IEnumerable<Usercredential> usercredential = unitOfWork.Usercredentials.Get();
        }
    }
}
