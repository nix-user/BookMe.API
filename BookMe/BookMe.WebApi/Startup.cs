﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Dependencies;
using System.Web.Mvc;
using BookMe.Auth.Cryptography.Abstract;
using BookMe.Auth.Providers;
using BookMe.Auth.Providers.Concrete;
using BookMe.Infrastructure.IoC;
using BookMe.Infrastructure.MapperConfiguration;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler;
using Microsoft.Owin.Security.OAuth;
using Owin;

[assembly: OwinStartup(typeof(BookMe.WebApi.Startup))]

namespace BookMe.WebApi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            this.ConfigureOAuth(app);
            app.UseWebApi(GlobalConfiguration.Configuration);
        }

        public void ConfigureOAuth(IAppBuilder app)
        {
            var symmetricCryptographyService = (ISymmetricCryptographyService)GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(ISymmetricCryptographyService));
            var adAccessTokenFormat = new ADAccessTokenFormat(symmetricCryptographyService);

            var oAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                Provider = new ADAuthorizationServerProvider(),
                AccessTokenFormat = adAccessTokenFormat
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(oAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions()
            {
                AccessTokenFormat = adAccessTokenFormat
            });
        }
    }
}