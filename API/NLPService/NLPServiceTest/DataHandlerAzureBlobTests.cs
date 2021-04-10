using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using DataHandlerAzureBlob;

namespace DocumentAnalyzerTest
{
    [TestClass]
    public class DataHandlerAzureBlobTests
    {
        [TestMethod]
        public void ValidPdfBlob()
        {
            // File blob storage url
            string blob_url = "https://soafiles.blob.core.windows.net/files/prueba.pdf";
            // Download the blob file
            string blob_text = BlobHandler.GetBlobText(blob_url);
            // Extension of the blob file
            string extension = Path.GetExtension(blob_text);

            Assert.AreEqual(".pdf", extension, null, "Blob type not correct");
        }

        [TestMethod]
        public void ValidWordBlob()
        {
            // File blob storage url
            string blob_url = "https://soafiles.blob.core.windows.net/files/prueba.docx";
            // Download the blob file
            string blob_text = BlobHandler.GetBlobText(blob_url);
            // Extension of the blob file
            string extension = Path.GetExtension(blob_text);

            Assert.AreEqual(".docx", extension, null, "Blob type not correct");
        }

        [TestMethod]
        public void ValidTxtBlob()
        {
            // File blob storage url
            string blob_url = "https://soafiles.blob.core.windows.net/files/prueba.txt";
            // Download the blob file
            string blob_text = BlobHandler.GetBlobText(blob_url);
            // Extension of the blob file
            string extension = Path.GetExtension(blob_text);

            Assert.AreEqual(".txt", extension, null, "Blob type not correct");
        }
    }
}
