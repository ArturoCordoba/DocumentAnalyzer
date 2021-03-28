using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataHandlerSQL.Configuration
{
    public sealed class DataHandlerSQLConfig
    {
        // The constructor is private because this class implements the Singleton Pattern
        private DataHandlerSQLConfig() { }

        private static readonly Lazy<DataHandlerSQLConfig> lazy = new Lazy<DataHandlerSQLConfig>(() => new DataHandlerSQLConfig());

        public static DataHandlerSQLConfig GetConfig
        {
            get
            {
                return lazy.Value;
            }
        }
        // Server = 127.0.0.1; Port = 5432; Database = DocAnalyzer; User Id = postgres; Password = password;
        private string _Employee_ConnectionString;

        public string Employee_ConnectionString
        {
            get { return _Employee_ConnectionString; }
            set { _Employee_ConnectionString = value; }
        }
    }
}
