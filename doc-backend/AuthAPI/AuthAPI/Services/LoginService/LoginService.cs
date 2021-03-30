using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AuthLibrary.Authorization;

using DataHandlerSQL.Model;
using DataHandlerSQL.Factory;
using DataHandlerSQL.Repository;

using AuthAPI.Services.EncryptionService;
using AuthLibrary.Factory;
using AuthLibrary.Authentication;

namespace AuthAPI.Services.LoginService
{
    public class LoginService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<UserCredential> _userRepository;
        private readonly IAuthenticationService _authentication;

        public LoginService(IUnitOfWorkFactory unitOfWorkFactory, IAuthServiceFactory authServiceFactory)
        {
            _unitOfWork = unitOfWorkFactory.Create();
            _userRepository = _unitOfWork.GetRepository<UserCredential>();
            _authentication = authServiceFactory.Authentication;
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
                // AuthLibrary validates the info and generates the token
                string token = _authentication.Authenticate(
                    requestData.email,
                    encryptedPassword,
                    user.Email,
                    user.UserPassword
                );
                return token;
            } 
            catch
            {
                return null;
            }

        }

    }
}
