using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Spire.Pdf;
using Spire.Doc;


namespace NLPService
{
    public class FileHandler
    {
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
    }
}
