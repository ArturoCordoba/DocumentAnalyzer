
using System;



namespace TestNPL
{
    class Program
    {
        static void Main(string[] args)
        {

            string url = "https://soafiles.blob.core.windows.net/files/prueba.txt";
            NPL_Handler.AnalyzeDocument(url);
            Console.Write("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
