using System;
using System.Threading;
using NLPService;

namespace Test
{
    class Program
    {
        /**
         * Test thread to add documents to the queue
         */
        public static void TestThread()
        {   
            // Blob url of the large document
            //string url0 = "https://soafiles.blob.core.windows.net/files/Dise_o_Proyecto_3___Arquitectura_de_Computadores_I.pdf";
            // Blob url of the first document
            string url1 = "https://soafiles.blob.core.windows.net/files/prueba.txt";
            // Blob url of the second document
            string url2 = "https://soafiles.blob.core.windows.net/files/prueba.docx";
            // Blob url of the third document
            string url3 = "https://soafiles.blob.core.windows.net/files/prueba.pdf";
            // Blob owner of the documents
            int owner = 69;

            // Adds the large document to the queue
            //NLPController.Instance.AddDocument(url0, owner.ToString());
            // Adds the first document to the queue
            NLPController.Instance.AddDocument(url1, owner.ToString());
            // Adds the second document to the queue
            NLPController.Instance.AddDocument(url2, owner.ToString());
            // Adds the third document to the queue
            NLPController.Instance.AddDocument(url3, owner.ToString());
            
        }
        

        static void Main(string[] args)
        {
            // Creates the thread that analyzes the documents in queue
            Thread analyze_thread = new Thread(new ThreadStart(NLPController.Instance.AnalyzeDocument));
            // Creates the test thread to add documents into the queue
            Thread test_thread = new Thread(new ThreadStart(TestThread));
            // Starts the analyzer thread
            analyze_thread.Start();
            // Starts the test thread
            test_thread.Start();
            // Waits for the analyzer thread to finish
            analyze_thread.Join();
            // Waits for the test thread to finish
            test_thread.Join();

            Console.Write("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
