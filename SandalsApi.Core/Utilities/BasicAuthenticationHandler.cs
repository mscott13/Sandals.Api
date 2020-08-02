using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SandalsApi.Core.Data;
using SandalsApi.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace SandalsApi.Core.Utilities
{
    public class ValidateBase64CredentialsSchemeOptions : AuthenticationSchemeOptions 
    { }

    public class BasicAuthenticationHandler : AuthenticationHandler<ValidateBase64CredentialsSchemeOptions> 
    {
        public BasicAuthenticationHandler(
            IOptionsMonitor<ValidateBase64CredentialsSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {

        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return Task.FromResult(AuthenticateResult.Fail("Authorization required"));
            }

            var authenticationHeaderValue = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
            var bytes = Convert.FromBase64String(authenticationHeaderValue.Parameter);
            string[] credentials = Encoding.UTF8.GetString(bytes).Split(":");

            if (PasswordManager.Authenticate(credentials)) 
            {
                User user = Database.GetUser(credentials[0], false);
                var claims = new[] {new Claim(ClaimTypes.Name, user.username), new Claim(ClaimTypes.Role, user.userType) };
                var claimsIdentity = new ClaimsIdentity(claims, nameof(BasicAuthenticationHandler));
                var ticket = new AuthenticationTicket(new ClaimsPrincipal(claimsIdentity), this.Scheme.Name);
                return Task.FromResult(AuthenticateResult.Success(ticket));
            }
            else
                return Task.FromResult(AuthenticateResult.Fail("Invalid credentials"));
        }
    }
}
