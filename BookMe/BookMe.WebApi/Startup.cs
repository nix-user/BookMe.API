using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Dependencies;
using System.Web.Mvc;
using BookMe.Auth.Cryptography.Abstract;
using BookMe.Auth.Providers;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;

[assembly: OwinStartup(typeof(BookMe.WebApi.Startup))]

namespace BookMe.WebApi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();

            this.ConfigureOAuth(app);

            WebApiConfig.Register(config);
            app.UseWebApi(config);
        }

        public void ConfigureOAuth(IAppBuilder app)
        {
            var symmetricCryptographyService = (ISymmetricCryptographyService)GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(ISymmetricCryptographyService));
            const int ExpirationTimeInDays = 365 * 100;

            var oAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(ExpirationTimeInDays),
                Provider = new ADAuthorizationServerProvider(),
                AccessTokenProvider = new AuthenticationTokenProvider(symmetricCryptographyService)
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(oAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
    }
}