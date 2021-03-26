using System;
using DocAnalyzerDataHandler;
using DocAnalyzerDataHandler.Repository;

namespace AuthLibrary
{
    public class TokenHandler
    {
        private static readonly string tokenSeparator = "$$$"; // Delimiter between the email, password and expiration date

        /**
         * This method uses the email and password of an user to create an unique token
         * tokenExpirationTime: minutes until the token is invalid, the default value is 120 minutes
         * return: encrypted token
        */
        public static string generateToken(string email, string password, int tokenExpirationTime = 120)
        {
            // Get the expiration date
            string expirationDate = DateTime.UtcNow.AddMinutes(tokenExpirationTime).ToString();

            string newToken = email + tokenSeparator + password + tokenSeparator + expirationDate;

            // Encryption of the new token
            string encryptedToken = EncryptRijndael.EncryptAes256(newToken);

            return encryptedToken;
        }

        private static string DecryptToken(string encryptedToken)
        {
            return EncryptRijndael.DecryptAes256(encryptedToken);
        }

        /**
         * This method validates the token, if is valid the user's email and password is returned
         * return: if the return value is null, is because the token was invalid,
         *         otherwise the return will be a list with the email as first element and the
         *         password as second element 
        */
        public static string[] ValidateToken(string token)
        {
            try
            {
                string decryptedToken = DecryptToken(token);
                string[] userData = decryptedToken.Split(tokenSeparator);

                // Check if the token has the 3 components (email, password and expiration date)
                if(userData.Length != 3) return null;

                // Get the current date
                DateTime currentDate = DateTime.UtcNow;

                // Get the expiration date
                string expirationDateString = userData[2];
                DateTime expirationDate = Convert.ToDateTime(expirationDateString);

                // Checks if the expiration date has passed
                if(DateTime.Compare(expirationDate, currentDate) < 0) return null;
                
                // The token has the 3 components and is valid
                return userData; // The email is returned
            }
            catch
            {
                return null;
            }
        }
    }
}
