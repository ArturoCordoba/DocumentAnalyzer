using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace DocAnalyzerDataHandler.Repository
{
    public class UnitOfWorkFactory
    {
        public static IUnitOfWork GetUnitOfWork(string connectionString)
        {
            // Creation of the options object for the DB Context 
            var optionsBuilder = new DbContextOptionsBuilder<DocAnalyzerContext>();
            optionsBuilder.UseNpgsql(connectionString);

            // Creation of the DB Context
            DocAnalyzerContext dbContext = new DocAnalyzerContext(optionsBuilder.Options);

            return new UnitOfWork(dbContext);
        }
    }
}
