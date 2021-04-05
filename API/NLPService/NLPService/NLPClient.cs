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

        /**
         * Method which recognizes all the entities of a document.
         * document: document text to be analyzed.
         */
        public static List<Employee> EntityRecognition(string document)
        {
            // Creates a client of the NLP API
            var nlp_client = new TextAnalyticsClient(endpoint, credentials);
            // Creates a list of Employee objects to store the recognized entities
            List<Employee> entities = new List<Employee>();
            // Obtain the recognized entities in a response
            var response = nlp_client.RecognizeEntities(document);
            // Adds the obtained entities into the list
            Console.WriteLine("Named Entities:");
            foreach (var entity in response.Value)
            {
                // Condition to identifies all the person entities
                if (entity.Category == "Person")
                {
                    // Name of the recognized entity
                    string name = entity.Text;
                    if (entities.Exists(person => person.Name == entity.Text)) 
                    {
                        var employee = entities.Find(person => person.Name == entity.Text);
                        employee.Quantity += 1;
                    }
                    else
                    {
                        entities.Add(new Employee(entity.Text, 1));
                    }
                }
            }
            return entities;
        }
    }
}
