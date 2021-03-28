using Microsoft.VisualStudio.TestTools.UnitTesting;

using AuthLibrary.Configuration;
using AuthLibrary.Factory;

namespace AuthUnitTest
{
    [TestClass]
    public class AuthServiceTest
    {
        private IAuthServiceFactory authFactory;

        [TestInitialize]
        public void Initialize()
        {
            AuthServiceConfig.Config.SecretKey = "shfvoilf4ltf645sf4%";
            AuthServiceConfig.Config.IssuerToken = "Test Issuer";
            AuthServiceConfig.Config.ExpirationTime = 60;

            authFactory = new AuthServiceFactory();
        }

        [TestMethod]
        public void Authenticate()
        {
            string email = "test@email.company.com";
            string correctEmail = "test@email.company.com";
            string password = "fsifjosf";
            string correctPassword = "fsifjosf";

            string token = authFactory.Authentication.Authenticate(email, password, correctEmail, correctPassword);
            Assert.IsNotNull(token);
        }

        [TestMethod]
        public void Authorize()
        {
            string email = "test@email.company.com";
            string correctEmail = "test@email.company.com";
            string password = "fsifjosf";
            string correctPassword = "fsifjosf";

            string token = authFactory.Authentication.Authenticate(email, password, correctEmail, correctPassword);

            bool validToken = authFactory.Authorization.Authorize(token);
            Assert.IsTrue(validToken);


            string expiredToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6InRlc3RAZW1haWwuY29tcGFueS5jb20iLCJuYmYiOjE2MTY5NjE1NDksImV4cCI6MTYxNjk2MTYwOSwiaWF0IjoxNjE2OTYxNTQ5LCJpc3MiOiJUZXN0IElzc3VlciJ9.8LfFbqPY0XPrFJYlgVNtG5iGTu2cW1tkGN3JLoYgz3g";
            bool invalidToken = authFactory.Authorization.Authorize(expiredToken);
            Assert.IsFalse(invalidToken);
        }

        [TestMethod]
        public void FakeToken()
        {
            string fakeToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6InRlc3RAZW1haWwuY29tIiwibmJmIjoxNjE2ODE4MTgzLCJleHAiOjE2MTY4MTgyNDMsImlhdCI6MTYxNjgxODE4MywiaXNzIjoiYXV0aCBhcGkifQ.FoBf1fAe6TIn8Q8hjuwVT33pQl1ojKwELVaPjuQ3xnLZawPBJHAIjERjbjCq";

            bool invalidToken = authFactory.Authorization.Authorize(fakeToken);
            Assert.IsFalse(invalidToken);
        }
    }
}
