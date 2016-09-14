using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using BookMe.Auth.Cryptography.Abstract;
using BookMe.Auth.Resources;
using Microsoft.Owin.Security;

namespace BookMe.Auth.Providers.Concrete
{
    public class ADAccessTokenFormat : ISecureDataFormat<AuthenticationTicket>
    {
        private const string TokenSeparator = " \n ";
        private const string AuthType = "bearer";

        private readonly ISymmetricCryptographyService symmetricCryptographyService;

        public ADAccessTokenFormat(ISymmetricCryptographyService symmetricCryptographyService)
        {
            this.symmetricCryptographyService = symmetricCryptographyService;
        }

        public string Protect(AuthenticationTicket data)
        {
            var userName = data.Identity.Claims.FirstOrDefault(x => x.Type == ExtendedClaimTypes.UserName)?.Value;
            var password = data.Identity.Claims.FirstOrDefault(x => x.Type == ExtendedClaimTypes.Password)?.Value;
            var name = data.Identity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

            var clearToken = userName + TokenSeparator + password + TokenSeparator + name;
            var encodedToken = this.symmetricCryptographyService.Encrypt(clearToken);

            return encodedToken;
        }

        public AuthenticationTicket Unprotect(string protectedText)
        {
            
            var clearToken = this.symmetricCryptographyService.Decrypt(protectedText);
            var userNamePasswordArray = clearToken.Split(new string[] { TokenSeparator }, StringSplitOptions.None);

            var userName = userNamePasswordArray[0];
            var password = userNamePasswordArray[1];
            var name = userNamePasswordArray[2];

            var claims = new List<Claim>
            {
                new Claim(ExtendedClaimTypes.UserName, userName),
                new Claim(ExtendedClaimTypes.Password, password),
                new Claim(ClaimTypes.Name, name)
            };

            var identity = new ClaimsIdentity(claims, AuthType);

            return new AuthenticationTicket(identity, null);
        }
    }
}
