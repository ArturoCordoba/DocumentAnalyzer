﻿using System;
using System.Collections.Generic;
using System.Threading;
using DataHandlerAzureBlob;
using DataHandlerMongoDB.Repository;
using DataHandlerMongoDB.Model;
using DataHandlerMongoDB.Factory;
using DataHandlerMongoDB.Configuration;
using FileMongo = DataHandlerMongoDB.Model.File;


namespace NLPService
{
    public sealed class NLPController
    {   
        //NLPController constructor
        private NLPController()
        {
        }

        //NLP singleton object
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

        // Creates the processing queue for blob documents
        private readonly Queue<Blob> blob_queue = new Queue<Blob>();

        /**
         * Method which enqueues a document into the processing queue
         */
        public void EnqueueBlob(Blob item)
        {
            // Locks the processing queue
            lock (blob_queue)
            {
                Console.WriteLine("Agregando documento en la cola...");
                // Enqueue the document for processing
                blob_queue.Enqueue(item);
                // Condition to wake up any blocked dequeue
                if (blob_queue.Count == 1)
                {
                    // Wake up any blocked dequeue
                    Monitor.PulseAll(blob_queue);
                }
            }
        }

        /**
         * Method which dequeues a document from the processing queue.
         * Return: document object popped from the processing queue.
         */
        public Blob DequeueBlob()
        {
            lock (blob_queue)
            {
                Console.WriteLine("Extrayendo documento de cola...");
                // Condition for the analyzer thread to wait
                while (blob_queue.Count == 0)
                {
                    Monitor.Wait(blob_queue);
                    Console.WriteLine("Esperando documento en cola...");
                }
                // Dequeue a blolb document from the processing queue
                Blob item = blob_queue.Dequeue();
                return item;
            }
        }

        /**
         * Method which creates a document object and adds it to the processing queue.
         * blob_url: azure blob storage document url.
         * blob_owner: name of the employee who uploaded the document to the azure blob storage.
         */
        public void AddDocument(string blob_url, string blob_owner)
        {
            // Obtain the blob title
            string blob_title = blob_url.Replace("https://soafiles.blob.core.windows.net/files/", "");
            // Create an empty list of references
            List<Reference> blob_references = new List<Reference>();
            // Create a new Document object with the metadata
            var blob = new Blob(blob_title, blob_url, blob_references, blob_owner, false);
            // Enqueue the new blob
            EnqueueBlob(blob);
            Console.WriteLine("Documento agregado...");
            Console.WriteLine(" ");

            // Create the mongo database file
            FileMongo file = new FileMongo();
            file.Title = blob.Title;
            file.Url = blob.Url;
            file.References = blob.References.ToArray();
            file.Owner = int.Parse(blob.Owner);
            file.Status = false;

            // Insert the document into the database
            DataHandlerMongoDBConfig.Config.ConnectionString = "mongodb://localhost:27017";
            DataHandlerMongoDBConfig.Config.DataBaseName = "DocAnalyzerEntities";
            IMongoRepositoryFactory factory = new MongoRepositoryFactory();
            IMongoRepository<FileMongo> repository = factory.Create<FileMongo>();
            repository.InsertOne(file);
        }

        /**
         * Method of the analyzer thread in charge of recognizing the documents person entities
         */
        public void AnalyzeDocument()
        {
            while (true)
            {
                // Dequeue a new blob
                Blob blob = DequeueBlob();
                Console.WriteLine("Documento extraido...");
                Console.WriteLine(" ");


                // Download the blob file
                string blob_file = BlobHandler.GetBlobText(blob.Url);
                // Obtain the blob file text
                string text = FileHandler.GetBlobText(blob_file);
                // Obtain the text references/entities
                blob.References = NLPClient.EntityRecognition(text);
                
                // Print the recognized employees
                for (int i = 0; i < blob.References.Count; i++)
                    Console.WriteLine(blob.References[i].Name + " " + blob.References[i].Qty);
                Console.WriteLine(" ");

                // Update the document in the database
                DataHandlerMongoDBConfig.Config.ConnectionString = "mongodb://localhost:27017";
                DataHandlerMongoDBConfig.Config.DataBaseName = "DocAnalyzerEntities";
                IMongoRepositoryFactory factory = new MongoRepositoryFactory();
                IMongoRepository<FileMongo> repository = factory.Create<FileMongo>();
                FileMongo update = repository.FindOne(file => file.Title == blob.Title && file.Owner == int.Parse(blob.Owner));
                update.References = blob.References.ToArray();
                update.Status = true;
                repository.ReplaceOne(update);
            }
        }
    }
}


