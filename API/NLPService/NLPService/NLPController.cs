using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;


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

            // Download the blob file
            string blob_file = DataHandler.GetBlobText(blob.Url);
            // Obtain the blob file text
            string text = FileHandler.GetBlobText(blob_file);
            
            List<Employee> employees = new List<Employee>();
            // Obtain the text references/entities
            employees = NLPClient.EntityRecognition(text);

            List<string> references = new List<string>();
           
            foreach (Employee employee in employees)
            {
                string json_employee = JsonSerializer.Serialize(employee);
                references.Add(json_employee);
                Console.WriteLine(json_employee);
            }
            blob.References = references;
            /*
            for (int i = 0; i < employees.Count; i++)
                Console.WriteLine(employees[i].Name + " " + employees[i].Quantity); ;
            */
            // Set true the status of the nlp
            blob.Status = true;

            string jsonString = JsonSerializer.Serialize(blob);
            Console.WriteLine(jsonString);

        }
    }
}
