using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthLibrary
{
    public interface IAuthServiceFactory
    {
        public IAuthenticationService GetAuthenticationService(string connectionString, string encryptPassword,
            string secretKey, string issuerToken, int expirationTime = 120);


    }
}
