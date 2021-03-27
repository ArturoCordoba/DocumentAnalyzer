using System;
using NLPService;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = "https://soafiles.blob.core.windows.net/files/prueba.txt";
            NLPService.NPLHandler.AnalyzeDocument(url);
            Console.Write("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
