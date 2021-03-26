using System;
using AuthLibrary;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            string email = "test@email.company.com";
            string password = "$gdfif445s2ds/93";

            string encryptedToken = TokenHandler.generateToken(email, password, -120);

            string[] userData = TokenHandler.ValidateToken(encryptedToken);

            Console.WriteLine(userData);
        }
    }
}
