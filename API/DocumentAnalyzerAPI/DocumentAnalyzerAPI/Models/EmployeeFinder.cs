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
        public static List<Match> FindEmployeeReferences(ResultRequest req, 
                                                             IMongoRepository<FileMongo> repository,
                                                             IUnitOfWork unit_of_work)
        {
            List<FileMongo> userFiles = repository.FilterBy(file => file.Title == req.Title && file.Owner == req.Owner).ToList();
            IRepository<Employee> employeeRepo = unit_of_work.GetRepository<Employee>();
            List<Employee> employeeList = employeeRepo.Get().ToList();
            List<Match> employeesIdentified = new List<Match>();

            foreach(FileMongo file in userFiles)
            {
                foreach(Reference fileRef in file.References)
                {
                    (bool, int) isReferenced = IsEmployeeReferenced(employeeList, fileRef.Name);
                    if (isReferenced.Item1)
                    {
                        employeesIdentified.Add(new Match(fileRef.Name, fileRef.Qty, isReferenced.Item2));
                    }
                }
            }
            return employeesIdentified;
        }

        private static (bool, int) IsEmployeeReferenced(List<Employee> employeeLst, string employeeName)
        {
            string[] splitSearchName = employeeName.ToLower().Split(' ');

            if (splitSearchName.Length > 1) // if it is only a name identification is not possible
            {
                foreach (Employee emp in employeeLst)
                {
                    string[] currentDBEmployeeName = emp.FullName.ToLower().Split(' ');

                    int matchCount = 0;

                    if (emp.FullName.Equals(employeeName)) // first verify if it's a total match
                    {
                        return (true, emp.EmployeeId);
                    }
                    else if (currentDBEmployeeName.First().Equals(splitSearchName.First()))
                    {
                        int similarCount = CountEmployeeOccurrences(currentDBEmployeeName, employeeLst);

                        foreach (string s in splitSearchName)
                        {
                            if (currentDBEmployeeName.Contains(s)) matchCount++;
                        }

                        int diff = currentDBEmployeeName.Length - matchCount;

                        if (diff == 1 && similarCount == 1) // if there is one lastname missing but there is only one employee with that name, it counts as a match
                        {
                            return (true, emp.EmployeeId);
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
            }
            return (false, -1);
        }

        private static int CountEmployeeOccurrences(string[] employeeSplit, List<Employee> employeeLst)
        {
            int count = 0;
            foreach(Employee employee in employeeLst)
            {
                string[] currentEmployee = employee.FullName.ToLower().Split(' ');

                if(currentEmployee.First().Equals(employeeSplit.First()) && currentEmployee[1].Equals(employeeSplit[1]))
                {
                    // compare first name and first last name.
                    count++;
                }
            }
            return count;
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

        public static void AddUserReferences(List<Match> processingResult,
                                             ResultRequest req,
                                             IUnitOfWork unit_of_work)
        {
            IRepository<EmployeeReferenceByDocument> referencesRepo = unit_of_work.GetRepository<EmployeeReferenceByDocument>();
            List<EmployeeReferenceByDocument> userRefs = referencesRepo.Get().ToList();

            foreach(Match match in processingResult)
            {
                EmployeeReferenceByDocument newMatch = new EmployeeReferenceByDocument();
                newMatch.DocumentId = req.Title;
                newMatch.EmployeeId = match.employeeId;
                newMatch.Ocurrences = match.qty;

                if (!ReferenceExists(userRefs, match.employeeId, req.Title))
                {
                    referencesRepo.Insert(newMatch);
                } else
                {
                    referencesRepo.Update(newMatch);
                }
                unit_of_work.Commit();
            }
        }

        private static bool ReferenceExists(List<EmployeeReferenceByDocument> currentRefs, int empId, string docId)
        {
            foreach (EmployeeReferenceByDocument eRef in currentRefs)
            {
                if (eRef.EmployeeId == empId && eRef.DocumentId.Equals(docId)) return true;
            }
            return false;
        }
    }
}
