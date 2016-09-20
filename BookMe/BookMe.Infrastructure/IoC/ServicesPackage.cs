using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookMe.BusinessLogic.Services.Abstract;
using BookMe.BusinessLogic.Services.Concrete;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using SimpleInjector.Packaging;

namespace BookMe.Infrastructure.IoC
{
    public class ServicesPackage : IPackage
    {
        public void RegisterServices(Container container)
        {
            var webApiRequestLifestyle = new WebApiRequestLifestyle();

            container.Register<IProfileService, ProfileService>(webApiRequestLifestyle);
            container.Register<IResourceService, ResourceService>(webApiRequestLifestyle);
        }
    }
}