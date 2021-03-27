using System;
using AuthLibrary;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            string email = "test@email.com";

            string secreyKey = "clave ultra secreta";
            string issuerToken = "auth api";
            int expirationTime = 1;

            IAuthService authService = AuthServiceFactory.GetAuthService(secreyKey, issuerToken, expirationTime);

            string token = authService.TokenGenerator.GenerateToken(email);

            string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6InRlc3RAZW1haWwuY29tIiwibmJmIjoxNjE2ODE4MTgzLCJleHAiOjE2MTY4MTgyNDMsImlhdCI6MTYxNjgxODE4MywiaXNzIjoiYXV0aCBhcGkifQ.OVsjMFeghd2mA44esltTS6c5zVpMd_8X6hbKmFUnKIQ";

            Console.WriteLine(token);
            bool validToken = authService.TokenValidator.VerifyToken(token);


            Console.WriteLine(validToken);
        }
    }
}
