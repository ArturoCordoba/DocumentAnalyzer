using System;
using System.Collections.Generic;


namespace NLPService
{
    public class NLPController
    {
        public static void AnalyzeDocument(string blob_url, string blob_owner)
        {
            // Obtain the blob title
            string blob_title = blob_url.Replace("https://soafiles.blob.core.windows.net/files/", "");
            // Create an empty list of references
            List<string> blob_references = new List<string>();
            // Create a new Document object with the metadata
            var blob = new Blob(blob_title, blob_url, blob_references, blob_owner, false);

            // Thread !!!!!!!
            // Create a queue for processing documents
            Queue<Blob> doc_queue = new Queue<Blob>();
            doc_queue.Enqueue(blob);

            // Obtain the blob text
            string text = DataHandler.GetBlobText(blob_url);
            // Obtain the text references/entities
            blob_references = NLPClient.EntityRecognition(text);
            blob.References = blob_references;

            for (int i = 0; i < blob.References.Count; i++)
                Console.WriteLine(blob.References[i]); ;
        
        }
    }
}
