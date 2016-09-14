using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookMe.Auth.Cryptography.Abstract;
using Microsoft.Owin.Security.Infrastructure;

namespace BookMe.Auth.Providers
{
    public class AuthenticationTokenProvider : IAuthenticationTokenProvider
    {
        private readonly ISymmetricCryptographyService symmetricCryptographyService;

        public AuthenticationTokenProvider(ISymmetricCryptographyService symmetricCryptographyService)
        {
            this.symmetricCryptographyService = symmetricCryptographyService;
        }

        public void Create(AuthenticationTokenCreateContext context)
        {
            throw new NotImplementedException();
        }

        public Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            throw new NotImplementedException();
        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
            throw new NotImplementedException();
        }

        public Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            throw new NotImplementedException();
        }
    }
}
