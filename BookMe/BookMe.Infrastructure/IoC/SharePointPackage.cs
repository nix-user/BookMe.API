using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookMe.BusinessLogic.Interfaces.SharePoint;
using BookMe.ShareProint.Data.Parsers.Abstract;
using BookMe.ShareProint.Data.Parsers.Concrete;
using BookMe.ShareProint.Data.Services.Concrete;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using SimpleInjector.Packaging;

namespace BookMe.Infrastructure.IoC
{
    public class SharePointPackage : IPackage
    {
        public void RegisterServices(Container container)
        {
            this.RegisterSPServices(container);
            this.RegisterSPParsers(container);
        }

        private void RegisterSPServices(Container container)
        {
            var webApiRequestLifestyle = new WebApiRequestLifestyle();
            container.Register<ISharePointReservationService, ReservationService>(webApiRequestLifestyle);
            container.Register<ISharePointResourceService, ResourceService>(webApiRequestLifestyle);
        }

        private void RegisterSPParsers(Container container)
        {
            var webApiRequestLifestyle = new WebApiRequestLifestyle();
            container.Register<IResourceParser, ResourceParser>(webApiRequestLifestyle);
            container.Register<IReservationParser, ReservationParser>(webApiRequestLifestyle);
        }
    }
}
