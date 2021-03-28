using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocAnalyzerDataHandler.Configuration
{
    public sealed class DataHandlerConfig
    {
        // The constructor is private because this class implements the Singleton Pattern
        private DataHandlerConfig() { }

        private static readonly Lazy<DataHandlerConfig> lazy = new Lazy<DataHandlerConfig>(() => new DataHandlerConfig());

        public static DataHandlerConfig GetConfig
        {
            get
            {
                return lazy.Value;
            }
        }
        // Server = 127.0.0.1; Port = 5432; Database = DocAnalyzer; User Id = postgres; Password = password;
        private string _Employee_ConnectionString;
        private string _Document_ConnectionString;

        public string Employee_ConnectionString
        {
            get { return _Employee_ConnectionString; }
            set { _Employee_ConnectionString = value; }
        }

        public string Document_ConnectionString
        {
            get { return _Document_ConnectionString; }
            set { _Document_ConnectionString = value; }
        }

    }
}
