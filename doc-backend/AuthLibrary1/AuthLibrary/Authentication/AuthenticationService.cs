using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DocAnalyzerDataHandler;
using DocAnalyzerDataHandler.Repository;

using AuthLibrary.Token;

namespace AuthLibrary
{
    class AuthenticationService : IAuthenticationService
    {
        private string encryptPassword; // Password to encrypt the user's passwords in the database
        private IUnitOfWork unitOfWork;
        private ITokenGenerator tokenGenerator;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="encryptPassword"> Password to encrypt the user's passwords in the database </param>
        /// <param name="unitOfWork"> Unit of work for the database operations </param>
        /// <param name="tokenGenerator"> Token generator </param>
        public AuthenticationService(string encryptPassword, IUnitOfWork unitOfWork, ITokenGenerator tokenGenerator)
        {
            this.encryptPassword = encryptPassword;
            this.unitOfWork = unitOfWork;
            this.tokenGenerator = tokenGenerator;
        }

        /// <summary>
        /// Method to authenticate an user
        /// </summary>
        /// <param name="email">Email of the alleged user</param>
        /// <param name="password">Password of the alleged user</param>
        /// <returns>
        ///     If during the process an error ocurred, an AuthenticationException will be thrown,
        ///     otherwise the token will be generated and returned
        /// </returns>
        public string Authenticate(string email, string password)
        {
            // Obtain the usercredential that has the specific email
            Usercredential usercredential = unitOfWork.Usercredentials.Get(
                usercredential => (usercredential.Email == email)
            ).FirstOrDefault();

            // Checks if the object is null
            if (usercredential == null) throw new AuthenticationException();

            // Check if the password matches
            string decodedPassword = Encryption.EncryptRijndael.EncryptAes256(password, encryptPassword);
            if (usercredential.UserPassword != password) throw new AuthenticationException();

            // The token is generated and returned
            return tokenGenerator.GenerateToken(email)   
        }
    }
}
