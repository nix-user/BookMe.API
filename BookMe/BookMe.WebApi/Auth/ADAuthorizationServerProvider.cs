using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Owin.Security.OAuth;

namespace BookMe.WebApi.Auth
{
    public class ADAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        private const string InvalidGrantKey = "invalid_grant";
        private const string InvalidGrantMessage = "The user name or password is incorrect";

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            using (var principalContext = new PrincipalContext(ContextType.Domain))
            {
                var isValid = principalContext.ValidateCredentials(context.UserName, context.Password);
                if (!isValid)
                {
                    context.SetError(InvalidGrantKey, InvalidGrantMessage);
                    return;
                }

                var userPrincipal = UserPrincipal.FindByIdentity(principalContext, context.UserName);
                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                identity.AddClaim(new Claim(ClaimTypes.Name, userPrincipal.Name));
                context.Validated(identity);
            }
        }
    }
}