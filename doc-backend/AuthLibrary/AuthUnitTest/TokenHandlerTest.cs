using Microsoft.VisualStudio.TestTools.UnitTesting;
using AuthLibrary;

namespace AuthUnitTest
{
    [TestClass]
    public class TokenHandlerTest
    {
        [TestMethod]
        public void ValidToken()
        {
            string email = "test@email.company.com";
            string password = "$gdfif445s2ds/93";

            string encryptedToken = TokenHandler.generateToken(email, password);

            string[] userData = TokenHandler.ValidateToken(encryptedToken);

            Assert.IsNotNull(userData);

            Assert.AreEqual(userData[0], email, 
                            "Original email and token email should be equal");
            Assert.AreEqual(userData[1], password, 
                            "Original password and token password should be equal");
        }

        [TestMethod]
        public void ExpirationDatePassed()
        {
            string email = "test@email.company.com";
            string password = "$gdfif445s2ds/93";

            string encryptedToken = TokenHandler.generateToken(email, password, -120);
            
            string[] userData = TokenHandler.ValidateToken(encryptedToken);

            Assert.IsNull(userData);
        }

        [TestMethod]
        public void FakeToken()
        {
            string fakeToken = "FoBf1fAe6TIn8Q8hjuwVT33pQl1ojKwELVaPjuQ3xnLZawPBJHAIjERjbjCq";
            string[] userData = TokenHandler.ValidateToken(fakeToken);

            Assert.IsNull(userData);
        }
    }
}
