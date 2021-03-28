using System;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace AuthLibrary.Token
{
    class TokenValidator : ITokenValidator
    {
        private string secretKey;
        private string issuerToken;

        public TokenValidator(string secretKey, string issuerToken)
        {
            this.secretKey = secretKey;
            this.issuerToken = issuerToken;
        }

        /// <summary>
        /// This method verify an encrypted token
        /// </summary>
        /// <param name="token">Encrypted token</param>
        /// <returns>true if the token is valid, false otherwise</returns>
        public bool VerifyToken(string token)
        {
            try
            {
                SecurityToken validatedToken;

                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = TokenValidationParameters();

                // This method throws an exception if the token is invalid
                tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

                return true;
            }
            catch (SecurityTokenValidationException)
            {
                return false;
            }
        }

        /// <summary>
        /// This method encapsulates the construction of the TokenValidationParameters object,
        /// needed to validate the token
        /// </summary>
        /// <returns>Validation parameters object</returns>
        private TokenValidationParameters TokenValidationParameters()
        {
            var securityKey = new SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(secretKey));

            return new TokenValidationParameters()
            {
                ValidateAudience = false,
                ValidateIssuer = true,
                ValidIssuer = issuerToken,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = securityKey,
                ValidateLifetime = true,
                LifetimeValidator = this.LifetimeValidator,
            };
        }

        /// <summary>
        /// This method checks if the token has expired
        /// </summary>
        /// <param name="notBefore"></param>
        /// <param name="expires"></param>
        /// <param name="securityToken"></param>
        /// <param name="validationParameters"></param>
        /// <returns></returns>
        private bool LifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            if (expires != null)
            {
                if (DateTime.UtcNow < expires) return true;
            }
            return false;
        }
    }
}
