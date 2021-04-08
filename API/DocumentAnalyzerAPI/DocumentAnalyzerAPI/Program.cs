using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace DocumentAnalyzerAPI
{
    public class Program
    {

        public static void startAPI(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static void Main(string[] args)
        {
            Thread analyzeThread = new Thread(new ThreadStart(NLPService.NLPController.Instance.AnalyzeDocument));
            Thread apiThread = new Thread(()=>startAPI(args));

            analyzeThread.Start();
            apiThread.Start();

            analyzeThread.Join();
            apiThread.Join();
        }


        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
