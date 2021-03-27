using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthLibrary
{
    public class AuthServiceFactory
    {
        public static IAuthService GetAuthService(string secretKey, string issuerToken, int expirationTime = 0)
        {
            return new AuthService(secretKey, issuerToken, expirationTime);
        }
    }
}
