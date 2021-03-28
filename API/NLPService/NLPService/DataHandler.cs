using System;
using System.IO;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace NLPService
{
    class DataHandler
    {
        public static Response<BlobDownloadInfo> DownloadBlob(BlobClient blob) 
        {   
            // Download blob information
            Response<BlobDownloadInfo> download = blob.Download();
            return download;
        }

        public static string GetBlobType(BlobClient blob)
        {
            // Get the blob properties
            BlobProperties properties = blob.GetProperties();
            // Get the blob content type
            string type = properties.ContentType;
            return type;
        }

        public static string GetBlobText(string blob_url) 
        {
            // Create a new blob client
            BlobClient blob = new BlobClient(new Uri(blob_url));
            // Download the blob information
            Response<BlobDownloadInfo> download = DownloadBlob(blob);
            // Obtain the blob content type
            string blob_type = GetBlobType(blob);

            // Obtain the text of a pdf file
            if (blob_type.Equals("application/pdf"))
            {   
                // Create a temporary pdf file
                string fileName = AppDomain.CurrentDomain.BaseDirectory + @"\file.pdf";
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
                // Copy the downloaded blob info to the file
                using (FileStream file = File.OpenWrite(fileName))
                {
                    download.Value.Content.CopyTo(file);
                }
                // Obtain the text from the pdf file
                string text = FileHandler.GetTextFromPDF(fileName);
                return text;
            }
            // Obtain the text of a word file
            else if (blob_type.Equals("application/vnd.openxmlformats-officedocument.wordprocessingml.document"))
            {   
                // Create a temporary word file
                string fileName = AppDomain.CurrentDomain.BaseDirectory + @"\file.docx";
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
                // Copy the downloaded blob info to the file
                using (FileStream file = File.OpenWrite(fileName))
                {
                    download.Value.Content.CopyTo(file);
                }
                // Obtain the text from the word file
                string text = FileHandler.GetTextFromWord(fileName);
                return text;
            }
            // Obtain the text of a txt file
            else if (blob_type.Equals("text/plain"))
            {
                // Create a temporary plain text file
                string fileName = AppDomain.CurrentDomain.BaseDirectory + @"\file.txt";
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
                // Copy the downloaded blob info to the file
                using (FileStream file = File.OpenWrite(fileName))
                {
                    download.Value.Content.CopyTo(file);
                }
                // Obtain the text from the plain text file
                string text = FileHandler.GetTextFromTxt(fileName);
                return text;
            }
            else
            {   
                // The format of the file is not a pdf, word or plain text
                string text = "El formato del documento no es soportado";
                return text;
            }
        }
    }
}
