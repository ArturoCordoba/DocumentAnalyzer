using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DataHandlerSQL.Model;
using DataHandlerSQL.Factory;
using DataHandlerSQL.Repository;

using AuthAPI.Services.EncryptionService;

namespace AuthAPI.Services.SignupService
{
    public enum SignupResult
    {
        Success,
        EmailRegistered,
        MissingInfo,
    }

    public class SignupService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<UserCredential> _userRepository;
        public SignupService(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWork = unitOfWorkFactory.Create();
            _userRepository = _unitOfWork.GetRepository<UserCredential>();
        }

        public SignupResult Signup(SignupDto userData)
        {
            // Checks if the userData received is valid
            if (userData == null)
                return SignupResult.MissingInfo;

            if (userData.email == null || userData.fullName == null || userData.password == null) 
                return SignupResult.MissingInfo;
            
            // Checks if the email is available
            if (!isEmailAvailable(userData.email)) 
                return SignupResult.EmailRegistered;

            // The received password is decoded and encoded
            string receivedPassword = Encrypt.Base64Decode(userData.password);
            string password = Encrypt.EncodeSHA256(receivedPassword);
            password = Encrypt.Base64Encode(password);

            UserCredential newUser = new UserCredential();
            newUser.Email = userData.email;
            newUser.FullName = userData.fullName;
            newUser.UserPassword = password;

            _userRepository.Insert(newUser);
            _unitOfWork.Commit();

            return SignupResult.Success;
        }



        /// <summary>
        /// Method to check if an email is used
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        private bool isEmailAvailable(string email)
        {
            UserCredential user = _userRepository.Get(user => user.Email == email).FirstOrDefault();

            if (user != null) return false;

            return true;
        }
    }
}
