using Microsoft.VisualStudio.TestTools.UnitTesting;
using AuthLibrary;

namespace AuthUnitTest
{
    [TestClass]
    public class AuthServiceTest
    {
        private IAuthService authService;

        [TestInitialize]
        public void Initialize()
        {
            string secreyKey = "clave ultra secreta";
            string issuerToken = "auth api";
            int expirationTime = 1;

            authService = AuthServiceFactory.GetAuthService(secreyKey, issuerToken, expirationTime);
        }

        [TestMethod]
        public void ValidToken()
        {
            string email = "test@email.company.com";
            string token = authService.TokenGenerator.GenerateToken(email);

            bool validToken = authService.TokenValidator.VerifyToken(token);

            Assert.IsTrue(validToken);
        }

        [TestMethod]
        public void ExpirationDatePassed()
        {
            string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6InRlc3RAZW1haWwuY29tIiwibmJmIjoxNjE2ODE4MTgzLCJleHAiOjE2MTY4MTgyNDMsImlhdCI6MTYxNjgxODE4MywiaXNzIjoiYXV0aCBhcGkifQ.OVsjMFeghd2mA44esltTS6c5zVpMd_8X6hbKmFUnKIQ";

            bool validToken = authService.TokenValidator.VerifyToken(token);

            Assert.IsFalse(validToken);
        }

        [TestMethod]
        public void FakeToken()
        {
            string fakeToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6InRlc3RAZW1haWwuY29tIiwibmJmIjoxNjE2ODE4MTgzLCJleHAiOjE2MTY4MTgyNDMsImlhdCI6MTYxNjgxODE4MywiaXNzIjoiYXV0aCBhcGkifQ.FoBf1fAe6TIn8Q8hjuwVT33pQl1ojKwELVaPjuQ3xnLZawPBJHAIjERjbjCq";
            
            bool validToken = authService.TokenValidator.VerifyToken(fakeToken);

            Assert.IsFalse(validToken);
        }
    }
}
