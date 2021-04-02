using System;
using System.Collections.Generic;
using System.Threading;


namespace NLPService
{
    public sealed class NLPController
    {
        private NLPController()
        {
        }

        private static NLPController instance = null;
        public static NLPController Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new NLPController();
                }
                return instance;
            }
        }

        private readonly Queue<Blob> blob_queue = new Queue<Blob>();

        public void EnqueueBlob(Blob item)
        {
            lock (blob_queue)
            {
                Console.WriteLine("Agregando documento en la cola...");
                blob_queue.Enqueue(item);
                if (blob_queue.Count == 1)
                {
                    // Wake up any blocked dequeue
                    Monitor.PulseAll(blob_queue);
                }
            }
        }
        public Blob DequeueBlob()
        {
            lock (blob_queue)
            {
                Console.WriteLine("Extrayendo documento de cola...");
                while (blob_queue.Count == 0)
                {
                    Monitor.Wait(blob_queue);
                    Console.WriteLine("Esperando documento en cola...");
                }
                Blob item = blob_queue.Dequeue();
                return item;
            }
        }

        public void AddDocument(string blob_url, string blob_owner)
        {
            // Obtain the blob title
            string blob_title = blob_url.Replace("https://soafiles.blob.core.windows.net/files/", "");
            // Create an empty list of references
            List<Employee> blob_references = new List<Employee>();
            // Create a new Document object with the metadata
            var blob = new Blob(blob_title, blob_url, blob_references, blob_owner, false);
            // Enqueue the new blob
            EnqueueBlob(blob);

            Console.WriteLine("Documento agregado...");
        }

        public void AnalyzeDocument()
        {
            while (true)
            {
                // Dequeue a new blob
                Blob blob = DequeueBlob();
                Console.WriteLine("Documento extraido...");

                Console.WriteLine("Analizando documento...");
                // Download the blob file
                string blob_file = DataHandler.GetBlobText(blob.Url);
                // Obtain the blob file text
                string text = FileHandler.GetBlobText(blob_file);
                // Obtain the text references/entities
                blob.References = NLPClient.EntityRecognition(text);
                

                // Print the recognized employees
                for (int i = 0; i < blob.References.Count; i++)
                    Console.WriteLine(blob.References[i].Name + " " + blob.References[i].Quantity);
            }
        }

        public void TestThread()
        {
            string url1 = "https://soafiles.blob.core.windows.net/files/prueba.txt";
            string url2 = "https://soafiles.blob.core.windows.net/files/prueba.docx";
            string url3 = "https://soafiles.blob.core.windows.net/files/prueba.pdf";
            string owner = "Fabian";

            /*
            while (true)
            {
                Thread.Sleep(3000);
                AddDocument(url1, owner);
                Thread.Sleep(5000);
                AddDocument(url2, owner);
                Thread.Sleep(4000);
                AddDocument(url3, owner);
            }
            */

            AddDocument(url1, owner);
            AddDocument(url2, owner);
            AddDocument(url3, owner);
        }

        public void StartService()
        {
            Thread analyze_thread = new Thread(new ThreadStart(AnalyzeDocument));
            Thread test_thread = new Thread(new ThreadStart(TestThread));
            analyze_thread.Start();
            test_thread.Start();
            analyze_thread.Join();
            test_thread.Join();
        }
    }
}
