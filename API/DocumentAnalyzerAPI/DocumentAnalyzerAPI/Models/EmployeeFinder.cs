using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataHandlerMongoDB.Factory;
using DataHandlerMongoDB.Repository;
using DataHandlerMongoDB.Model;
using FileMongo = DataHandlerMongoDB.Model.File;
using DataHandlerSQL.Model;
using DocumentAnalyzerAPI.Models;
using DataHandlerSQL.Repository;

namespace DocumentASnalyzerAPI.Models
{
    public class EmployeeFinder
    {
        public static List<Reference> FindEmployeeReferences(ResultRequest req, 
                                                             IMongoRepository<FileMongo> repository,
                                                             IUnitOfWork unit_of_work)
        {
            List<FileMongo> userFiles = repository.FilterBy(file => file.Title == req.Title && file.Owner == req.Owner).ToList();
            IRepository<Employee> employeeRepo = unit_of_work.GetRepository<Employee>();
            List<Employee> employeeList = employeeRepo.Get().ToList();
            List<Reference> employeesIdentified = new List<Reference>();

            foreach(FileMongo file in userFiles)
            {
                foreach(Reference fileRef in file.References)
                {
                    if(isEmployeeReferenced(employeeList, fileRef.Name))
                    {
                        employeesIdentified.Add(fileRef);
                    }
                }
            }
            return employeesIdentified;
        }

        private static bool isEmployeeReferenced(List<Employee> employeeLst, string employeeName)
        {
            foreach(Employee emp in employeeLst)
            {
                if(emp.FullName.Equals(employeeName))
                {
                    return true;
                }
            }
            return false;
        }

        public static List<UserDocument> FindEmployeeDocuments(int employeeId, IMongoRepository<FileMongo> repository)
        {
            List<FileMongo> userFiles = repository.FilterBy(file => file.Owner == employeeId).ToList();
            List<UserDocument> result = new List<UserDocument>();
            
            foreach(FileMongo file in userFiles)
            {
                result.Add(new UserDocument(file.Title, file.Status));
            }
            return result;
        }
    }
}
