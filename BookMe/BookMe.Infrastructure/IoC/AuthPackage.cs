﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookMe.Auth.Cryptography.Abstract;
using BookMe.Auth.Cryptography.Concrete;
using BookMe.Auth.Providers.Abstract;
using BookMe.Auth.Providers.Concrete;
using BookMe.BusinessLogic.Interfaces.SharePoint;
using BookMe.ShareProint.Data.Services.Concrete;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using SimpleInjector.Packaging;

namespace BookMe.Infrastructure.IoC
{
    public class AuthPackage : IPackage
    {
        public void RegisterServices(Container container)
        {
            var webApiRequestLifestyle = new WebApiRequestLifestyle();

            container.Register<ISymmetricCryptographyService, AESCryptographyService>(Lifestyle.Transient);
            container.Register<ISimetricCryptographyKeyProvider, AppConfigSimetricCryptographyKeyProvider>(Lifestyle.Transient);
            container.Register<ICredentialsProvider, CredentialsProvider>(webApiRequestLifestyle);
        }
    }
}