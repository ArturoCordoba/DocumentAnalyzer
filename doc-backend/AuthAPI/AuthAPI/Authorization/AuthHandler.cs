using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

using AuthLibrary.Factory;

namespace AuthAPI.Authorization
{
    public class AuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        IAuthServiceFactory authFactory;

        public AuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IAuthServiceFactory authFactory)
        : base(options, logger, encoder, clock)
        {
            this.authFactory = authFactory;
        }

        private const string API_TOKEN_PREFIX = "Bearer";

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string token = null;
            string authorization = Request.Headers["Authorization"];

            if (string.IsNullOrEmpty(authorization))
            {
                return AuthenticateResult.NoResult();
            }

            if (authorization.StartsWith(API_TOKEN_PREFIX, StringComparison.OrdinalIgnoreCase))
            {
                token = authorization.Substring(API_TOKEN_PREFIX.Length).Trim();
            }

            if (string.IsNullOrEmpty(token))
            {
                return AuthenticateResult.NoResult();
            }

            // does the token match ?
            bool validToken = authFactory.Authorization.Authorize(token);

            if (!validToken)
            {
                return AuthenticateResult.Fail($"token {API_TOKEN_PREFIX} not match");
            }
            else
            {
                token = "";
                var id = new ClaimsIdentity(
                    new Claim[] { new Claim("Key", token) },  // not safe , just as an example , should custom claims on your own
                    Scheme.Name
                );
                ClaimsPrincipal principal = new ClaimsPrincipal(id);
                var ticket = new AuthenticationTicket(principal, new AuthenticationProperties(), Scheme.Name);
                return AuthenticateResult.Success(ticket);
            }
        }
    }
}
