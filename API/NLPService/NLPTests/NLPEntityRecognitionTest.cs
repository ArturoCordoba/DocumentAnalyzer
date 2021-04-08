using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using NLPService;
using DataHandlerMongoDB.Model;

namespace DocumentAnalyzerTests
{
    [TestClass]
    public class NLPEntityRecognitionTest
    {
        [TestMethod]
        public void EntityRecognition()
        {
            string document = "Fabian Gonzalez and Arturo Cordoba and Marcelo Sanchez and Jose Montoya";
            List<Reference> entities = NLPClient.EntityRecognition(document);

            Assert.AreEqual("Fabian Gonzalez", entities[0].Name, null, "Entity name not correct");
            Assert.AreEqual("Arturo Cordoba", entities[1].Name, null, "Entity name not correct");
            Assert.AreEqual("Marcelo Sanchez", entities[2].Name, null, "Entity name not correct");
            Assert.AreEqual("Jose Montoya", entities[3].Name, null, "Entity name not correct");
        }
    }
}
