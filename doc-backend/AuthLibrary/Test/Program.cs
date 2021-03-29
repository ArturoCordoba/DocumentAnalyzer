using System;
using AuthLibrary;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

using AuthLibrary.Configuration;
using AuthLibrary.Factory;

using System.Security.Claims;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {

            AuthServiceConfig.Config.SecretKey = "shfvoilf4ltf645sf4%";
            AuthServiceConfig.Config.IssuerToken = "Test Issuer";
            AuthServiceConfig.Config.ExpirationTime = 1;

            IAuthServiceFactory authFactory = new AuthServiceFactory();

            string email = "test@email.company.com";
            string correctEmail = "test@email.company.com";
            string password = "fsifjosf";
            string correctPassword = "fsifjosf";

            string token = authFactory.Authentication.Authenticate(email, password, correctEmail, correctPassword);

            Console.WriteLine(token);

            Console.WriteLine("");

            authFactory.Authorization.Authorize(token);

            ClaimsPrincipal claims = authFactory.Authorization.Claims;

            Console.WriteLine(claims.FindFirst(ClaimTypes.Email).Value);
        }
    }
}
