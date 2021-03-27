using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthLibrary
{
    class AuthService: IAuthService
    {
        private string secretKey;
        private string issuerToken;
        private int expirationTime;

        public AuthService(string secretKey, string issuerToken, int expirationTime = 0)
        {
            this.secretKey = secretKey;
            this.issuerToken = issuerToken;
            this.expirationTime = expirationTime;
        }

        public ITokenGenerator TokenGenerator
        {
            get
            {
                return new TokenGenerator(secretKey, issuerToken, expirationTime);
            }
        }

        public ITokenValidator TokenValidator
        {
            get
            {
                return new TokenValidator(secretKey, issuerToken);
            }
        }
    }
}
