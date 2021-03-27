using System;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace AuthLibrary
{
    class TokenGenerator : ITokenGenerator
    {
        private string secretKey;
        private int expirationTime; // Minutes untilt the token is invalid
        private string issuerToken;

        public TokenGenerator(string secretKey, string issuerToken, int expirationTime)
        {
            this.secretKey = secretKey;
            this.issuerToken = issuerToken;
            this.expirationTime = expirationTime;
        }

        /// <summary>
        /// This method implements the JWT Token Generator
        /// </summary>
        /// <param name="email">Email of the user</param>
        /// <returns>JWT Token</returns>
        public string GenerateToken(string email)
        {
            var securityKey = new SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(secretKey));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            // Create a Claims Identity
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Email, email) });

            // Create token to the user
            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var jwtSecurityToken = tokenHandler.CreateJwtSecurityToken(
                issuer: issuerToken,
                subject: claimsIdentity,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(expirationTime)),
                signingCredentials: signingCredentials);

            var jwtTokenString = tokenHandler.WriteToken(jwtSecurityToken);
            return jwtTokenString;
        }
    }
}
