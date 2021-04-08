using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;


using DataHandlerSQL.Model;
using DataHandlerSQL.Factory;
using DataHandlerSQL.Repository;

using AuthAPI.Services.EncryptionService;
using AuthLibrary.Factory;
using AuthLibrary.Token;

namespace AuthAPI.Services.LoginService
{
    public class LoginService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<UserCredential> _userRepository;
        private readonly ITokenGenerator _tokenGenerator;

        public LoginService(IUnitOfWorkFactory unitOfWorkFactory, IAuthServiceFactory authServiceFactory)
        {
            _unitOfWork = unitOfWorkFactory.Create();
            _userRepository = _unitOfWork.GetRepository<UserCredential>();
            _tokenGenerator = authServiceFactory.TokenGenerator;
        }

        /// <summary>
        /// Method to validate the request data and determine if it is valid or not
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns>Null if is something wrong, otherwise it will return a new token</returns>
        public string Login(LoginDto requestData)
        {
            // Checks if the request data is valid
            if (requestData == null) return null;

            // Checks if the email is valid
            if (requestData.email == null) return null;

            // Checks if is an user is registered with the received email
            UserCredential user = _userRepository.Get(user => user.Email == requestData.email).FirstOrDefault();
            if (user == null) return null;

            // Decode and encode the received password 
            string receivedPassword = Encrypt.Base64Decode(requestData.password);
            string encryptedPassword = Encrypt.EncodeSHA256(receivedPassword);
            encryptedPassword = Encrypt.Base64Encode(encryptedPassword);

            string a = encryptedPassword;

            try
            {
                // The information received is validated
                if (!Authenticate(requestData.email,
                    encryptedPassword,
                    user.Email,
                    user.UserPassword))
                    return null;

                // The claim list includes the email and userid
                var claims = new List<KeyValuePair<string, string>>() {
                    new KeyValuePair<string, string>(ClaimTypes.Email, user.Email),
                    new KeyValuePair<string, string>(ClaimTypes.NameIdentifier, user.UserId.ToString())
                };

                // The new token is generated
                string token = _tokenGenerator.GenerateToken(claims);
                
                return token;
            } 
            catch
            {
                return null;
            }

        }

        /// <summary>
        /// Method to check if the info received and the info stored are equal
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="correctEmail"></param>
        /// <param name="correctPassword"></param>
        /// <returns></returns>
        private bool Authenticate(string email, string password, string correctEmail, string correctPassword)
        {
            // Checks if one of the arguments is null
            if (email == null || password == null || correctEmail == null || correctPassword == null)
                return false;

            // Checks if the email are equal
            if (email != correctEmail)
                return false;

            // Checks if the password matches
            if (password != correctPassword)
                return false;

            // The information is valid
            return true;
        }

    }
}
