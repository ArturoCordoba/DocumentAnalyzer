using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Azure.AI.TextAnalytics;
using Spire.Pdf;
using Spire.Doc;

namespace NLPService
{
    public class NPLHandler
    {
        private static readonly AzureKeyCredential credentials = new AzureKeyCredential("cadb9f0784e1410ca2eb58015096f78c");
        private static readonly Uri endpoint = new Uri("https://soa-nlp-api.cognitiveservices.azure.com/");

        public static string GetTextFromPDF(string path)
        {
            PdfDocument document = new PdfDocument();
            document.LoadFromFile(path);
            StringBuilder text = new StringBuilder();
            foreach (PdfPageBase page in document.Pages)
            {
                text.Append(page.ExtractText());
            }
            return text.ToString();
        }

        public static string GetTextFromWord(string path)
        {
            Document doc = new Document();
            doc.LoadFromFile(path);
            string text = doc.GetText();
            return text;
        }

        public static string GetTextFromTxt(string path)
        {
            string text = System.IO.File.ReadAllText(path);
            return text.ToString();
        }

        public static List<string> EntityRecognition(TextAnalyticsClient client, string document)
        {
            List<string> entities = new List<string>();
            var response = client.RecognizeEntities(document);
            Console.WriteLine("Named Entities:");
            foreach (var entity in response.Value)
            {
                if (entity.Category == "Person")
                {
                    entities.Add(entity.Text);
                    //Console.WriteLine($"\tText: {entity.Text},\tCategory: {entity.Category},\tSub-Category: {entity.SubCategory}");
                    //Console.WriteLine($"\t\tScore: {entity.ConfidenceScore:F2},\tLength: {entity.Length},\tOffset: {entity.Offset}\n");
                }
            }
            return entities;
        }

        public static void GetBlobProperties(BlobClient blob)
        {
            // Get the blob properties
            BlobProperties properties = blob.GetProperties();

            // Display some of the blob's property values
            Console.WriteLine($" ContentType: {properties.ContentType}");
            Console.WriteLine($" CreatedOn: {properties.CreatedOn}");
            Console.WriteLine($" LastModified: {properties.LastModified}");
        }

        public static string GetBlobType(BlobClient blob)
        {
            // Get the blob properties
            BlobProperties properties = blob.GetProperties();

            // Get the blob content type
            string type = properties.ContentType;
            return type;
        }

        public static void AnalyzeDocument(string blob_url)
        {
            var nlp_client = new TextAnalyticsClient(endpoint, credentials);

            //string connectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");
            //Console.WriteLine(connectionString);

            BlobClient blob = new BlobClient(new Uri(blob_url));
            Response<BlobDownloadInfo> download = blob.Download();
            string blob_type = GetBlobType(blob);

            List<string> entities = new List<string>();

            //Console.WriteLine(blob_type);
            if (blob_type.Equals("application/pdf"))
            {
                string fileName = @"C:\Users\fagon\source\repos\TestNPL\TestNPL\file.pdf";
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
                using (FileStream file = File.OpenWrite(fileName))
                {
                    download.Value.Content.CopyTo(file);
                }
                var text = GetTextFromPDF(fileName);
                entities = EntityRecognition(nlp_client, text);
            }
            else if (blob_type.Equals("application/vnd.openxmlformats-officedocument.wordprocessingml.document"))
            {
                string fileName = @"C:\Users\fagon\source\repos\TestNPL\TestNPL\file.docx";
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
                using (FileStream file = File.OpenWrite(fileName))
                {
                    download.Value.Content.CopyTo(file);
                }
                var text = GetTextFromWord(fileName);
                entities = EntityRecognition(nlp_client, text);
            }
            else if (blob_type.Equals("text/plain"))
            {
                string fileName = @"C:\Users\fagon\source\repos\TestNPL\TestNPL\file.txt";
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
                using (FileStream file = File.OpenWrite(fileName))
                {
                    download.Value.Content.CopyTo(file);
                }
                var text = GetTextFromTxt(fileName);
                entities = EntityRecognition(nlp_client, text);
            }

            for (int i = 0; i < entities.Count; i++)
                Console.WriteLine(entities[i]);

            //GetBlobProperties(blob);
        }
    }
}
