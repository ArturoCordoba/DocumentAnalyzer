using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AuthLibrary.Token;

namespace AuthLibrary.Authentication
{
    class AuthenticationService : IAuthenticationService
    {
        private readonly ITokenGenerator tokenGenerator;

        public AuthenticationService(ITokenGenerator tokenGenerator)
        {
            this.tokenGenerator = tokenGenerator;
        }

        /// <summary>
        /// Method that checks if the information received is valid and generates a new token
        /// </summary>
        /// <param name="email">Supposed email</param>
        /// <param name="password">Supposed password</param>
        /// <param name="correctEmail">Correct email</param>
        /// <param name="correctPassword">Correct password</param>
        /// <returns>
        ///     This method raises an AuthenticationException if is somewrong,
        ///     otherwise the token will be returned
        /// </returns>
        public string Authenticate(string email, string password, string correctEmail, string correctPassword)
        {

            // Checks if one of the arguments is null
            if (email == null || password == null || correctEmail == null || correctPassword == null)
                throw new AuthenticationException("Received null arguments");

            // Checks if the email are equal
            if (email != correctEmail)
                throw new AuthenticationException("Emails doesn't match");

            // Checks if the password matches
            if (password != correctPassword)
                throw new AuthenticationException("Passwords doesn't match");

            // The token is generated and returned
            return tokenGenerator.GenerateToken(email);   
        }
    }
}
