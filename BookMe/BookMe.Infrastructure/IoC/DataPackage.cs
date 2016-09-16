using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookMe.Auth.Cryptography.Abstract;
using BookMe.Auth.Cryptography.Concrete;
using BookMe.BusinessLogic.Repository;
using BookMe.Data.Context;
using BookMe.Data.Repository;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using SimpleInjector.Packaging;

namespace BookMe.Infrastructure.IoC
{
    public class DataPackage : IPackage
    {
        public void RegisterServices(Container container)
        {
            var webApiRequestLifestyle = new WebApiRequestLifestyle();

            container.Register(typeof(IRepository<>), typeof(EFRepository<>), webApiRequestLifestyle);
            container.Register<DbContext, AppContext>(webApiRequestLifestyle);
        }
    }
}