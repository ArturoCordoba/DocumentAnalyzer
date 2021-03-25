using NUnit.Framework;
using AuthLibrary;

namespace AuthUnitTest
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }


        [Test]
        public void Test2()
        {
            string result = Class1.HelloWorld();
            Assert.AreEqual(result, "Hello World from Class1!");
        }
    }
}