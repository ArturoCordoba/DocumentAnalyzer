using Microsoft.VisualStudio.TestTools.UnitTesting;
using DocAnalyzerDataHandler.Repository;
using DocAnalyzerDataHandler;
using System.Collections.Generic;

namespace DocAnalyzerDataHandlerUnitTest
{
    [TestClass]
    public class UnitTestEmployee
    {
        private IUnitOfWork unitOfWork;

        [TestInitialize]
        public void Initialize()
        {
            string connString = "Server = 127.0.0.1; Port = 5432; Database = DocAnalyzer; User Id = postgres; Password = password;";
            unitOfWork = UnitOfWorkFactory.GetUnitOfWork(connString);
        }

        [TestMethod]
        public void TestInsert()
        {
            Employee employee = new Employee();
            employee.FullName = "Test employee #sYNdsDFK74=4?";
            unitOfWork.Employees.Insert(employee);
            unitOfWork.Commit();
        }

        [TestMethod]
        public void TestGetById()
        {
            Employee employee = unitOfWork.Employees.GetById(1);
        }

        [TestMethod]
        public void TestGetAll()
        {
            IEnumerable<Employee> employees = unitOfWork.Employees.Get();
            foreach (Employee employee in employees)
            {
                System.Console.WriteLine(employee.FullName);
            }
        }

        [TestMethod]
        public void TestUpdate()
        {
            string newName = "Test employee #sY*dr6GDd74=4? Updated";
            Employee employee = unitOfWork.Employees.GetById(1);
            employee.FullName = newName;
            unitOfWork.Employees.Update(employee);
            unitOfWork.Commit();
        }

        [TestMethod]
        public void TestDelete()
        {
            Employee employee = new Employee();
            employee.FullName = "Test employee #3434sd3f5s3fd";
            employee.UserId = 100000;

            unitOfWork.Employees.Insert(employee);
            unitOfWork.Commit();

            unitOfWork.Employees.Delete(employee);
            unitOfWork.Commit();
        }
    }
}
