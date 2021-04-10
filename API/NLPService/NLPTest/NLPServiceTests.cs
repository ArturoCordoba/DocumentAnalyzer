using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using NLPService;
using DataHandlerMongoDB.Model;

namespace DocumentAnalyzerTest
{
    [TestClass]
    public class NLPServiceTests
    {
        [TestMethod]
        public void ValidEntityNameRecognition()
        {

            string document = "Fabian Gonzalez and Arturo Cordoba and Marcelo Sanchez and Jose Montoya";
            List<Reference> entities = NLPClient.EntityRecognition(document);

            Assert.AreEqual("Fabian Gonzalez", entities[0].Name, null, "Entity name not correct");
            Assert.AreEqual("Arturo Cordoba", entities[1].Name, null, "Entity name not correct");
            Assert.AreEqual("Marcelo Sanchez", entities[2].Name, null, "Entity name not correct");
            Assert.AreEqual("Jose Montoya", entities[3].Name, null, "Entity name not correct");
        }

        [TestMethod]
        public void ValidEntityQtyRecognition()
        {
            string document = "Fabian Gonzalez and Arturo Cordoba and Fabian Gonzalez and Jose Montoya";
            List<Reference> entities = NLPClient.EntityRecognition(document);

            Assert.AreEqual(2, entities[0].Qty, 0.001, "Entity quantity not correct");
            Assert.AreEqual(1, entities[1].Qty, 0.001, "Entity quantity not correct");
            Assert.AreEqual(1, entities[2].Qty, 0.001, "Entity quantity not correct");
        }


        [TestMethod]
        public void NotValidEntityRecognition()
        {
            string document = "Hola mundo, este es el proyecto 1 de arquitectura de software";
            List<Reference> entities = NLPClient.EntityRecognition(document);

            Assert.AreEqual(0, entities.Count, 0.001, "Entity not recognized");
        }
    }
}
