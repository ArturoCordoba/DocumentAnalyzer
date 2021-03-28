using System;
using System.Collections.Generic;
using Azure;
using Azure.AI.TextAnalytics;
namespace NLPService
{
    public class NLPClient
    {
        // Set the credentials of the client
        private static readonly AzureKeyCredential credentials = new AzureKeyCredential("cadb9f0784e1410ca2eb58015096f78c");
        // Set the endpoint of the client
        private static readonly Uri endpoint = new Uri("https://soa-nlp-api.cognitiveservices.azure.com/");

        public static List<string> EntityRecognition(string document)
        {
            //Create a client of the NLP API
            var nlp_client = new TextAnalyticsClient(endpoint, credentials);

            List<string> entities = new List<string>();
            var response = nlp_client.RecognizeEntities(document);
            Console.WriteLine("Named Entities:");
            foreach (var entity in response.Value)
            {
                if (entity.Category == "Person")
                {
                    entities.Add(entity.Text);
                }
            }
            return entities;
        }
    }
}
