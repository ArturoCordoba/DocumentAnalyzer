﻿using System;
using System.IO;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace DataHandlerAzureBlob
{
    public class BlobHandler
    {
        /**
         * Methods which downloads the blob document from the azure blob storage.
         * blob: client of blob document from the object storage
         */
        public static Response<BlobDownloadInfo> DownloadBlob(BlobClient blob)
        {
            // Download blob information
            Response<BlobDownloadInfo> download = blob.Download();
            return download;
        }

        /**
         * Method which identifies the type of content of the blob document.
         * blob: client of blob document from the object storage.
         */
        public static string GetBlobType(BlobClient blob)
        {
            // Get the blob properties
            BlobProperties properties = blob.GetProperties();
            // Get the blob content type
            string type = properties.ContentType;
            return type;
        }

        /**
         * Method which extract the text from a pdf, word or plain text file
         * blob_url: azure blob storage document url.
         */
        public static string GetBlobText(string blob_url)
        {
            // Create a new blob client
            BlobClient blob = new BlobClient(new Uri(blob_url));
            // Download the blob information
            Response<BlobDownloadInfo> download = DownloadBlob(blob);
            // Obtain the blob content type
            string blob_type = GetBlobType(blob);

            // Creates a pdf file with blob info
            if (blob_type.Equals("application/pdf"))
            {
                // Creates temporary pdf file
                string file_path = AppDomain.CurrentDomain.BaseDirectory + @"\file.pdf";
                if (File.Exists(file_path))
                {
                    File.Delete(file_path);
                }
                // Copy the downloaded blob info to the file
                using (FileStream file = File.OpenWrite(file_path))
                {
                    download.Value.Content.CopyTo(file);
                }
                return file_path;
            }
            // Creates a word file with blob info
            else if (blob_type.Equals("application/vnd.openxmlformats-officedocument.wordprocessingml.document"))
            {
                // Create a temporary word file
                string file_path = AppDomain.CurrentDomain.BaseDirectory + @"\file.docx";
                if (File.Exists(file_path))
                {
                    File.Delete(file_path);
                }
                // Copy the downloaded blob info to the file
                using (FileStream file = File.OpenWrite(file_path))
                {
                    download.Value.Content.CopyTo(file);
                }
                return file_path;
            }
            // Creates a plain text file with the blob info
            else if (blob_type.Equals("text/plain") || blob_type.Equals("application/octet-stream"))
            {
                // Create a temporary plain text file
                string file_path = AppDomain.CurrentDomain.BaseDirectory + @"\file.txt";
                if (File.Exists(file_path))
                {
                    File.Delete(file_path);
                }
                // Copy the downloaded blob info to the file
                using (FileStream file = File.OpenWrite(file_path))
                {
                    download.Value.Content.CopyTo(file);
                }
                return file_path;
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
