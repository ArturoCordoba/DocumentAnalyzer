using System;
using DataHandlerSQL.Repository;
using DataHandlerSQL;
using System.Collections.Generic;
using System.Linq;

using Microsoft.EntityFrameworkCore;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            string connString = "Server = 127.0.0.1; Port = 5432; Database = DocAnalyzer; User Id = postgres; Password = password;";
            var optionsBuilder = new DbContextOptionsBuilder<DocAnalyzerContext>();
            optionsBuilder.UseNpgsql(connString);
            DocAnalyzerContext dbContext = new DocAnalyzerContext(optionsBuilder.Options);


            IUnitOfWork unitOfWork = new UnitOfWork(dbContext);
            IRepository<Usercredential> rpUsercredential = unitOfWork.Repository<Usercredential>();
            IRepository<Employee> rpEmployee = unitOfWork.Repository<Employee>();

            Usercredential usercredential = new Usercredential();
            usercredential.FullName = "Prueba 1";
            usercredential.UserPassword = "sfeojfs";
            usercredential.Email = "prueba1@gmail.com";

            rpUsercredential.Insert(usercredential);

            Employee employee = new Employee();
            employee.FullName = "Prueba 2";

            rpEmployee.Insert(employee);


            unitOfWork.Commit();
        }
    }
}
