using System;
using NLPService;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = "https://soafiles.blob.core.windows.net/files/prueba.txt";
            //string url = "https://soafiles.blob.core.windows.net/files/prueba.docx";
            //string url = "https://soafiles.blob.core.windows.net/files/prueba.pdf";
            string owner = "Fabian";
            NLPController.AnalyzeDocument(url, owner);
            Console.Write("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
