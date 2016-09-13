using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookMe.BusinessLogic.Interfaces.SharePoint;
using BookMe.Core.Models;
using BookMe.Core.Models.Recurrence;
using BookMe.ShareProint.Data;
using BookMe.ShareProint.Data.Converters.Abstract;
using BookMe.ShareProint.Data.Converters.Concrete;
using BookMe.ShareProint.Data.Parsers.Abstract;
using BookMe.ShareProint.Data.Parsers.Concrete;
using BookMe.ShareProint.Data.Services.Concrete;
using Microsoft.SharePoint.Client;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using SimpleInjector.Packaging;

namespace BookMe.Infrastructure.IoC
{
    public class SharePointPackage : IPackage
    {
        private Container container;

        public void RegisterServices(Container container)
        {
            this.container = container;
            this.RegisterSPServices();
            this.RegisterSPParsers();
            this.RegisterSPConverters();
            this.RegisterSPContext();
        }

        private void RegisterSPServices()
        {
            var webApiRequestLifestyle = new WebApiRequestLifestyle();
            this.container.Register<ISharePointReservationService, ReservationService>(webApiRequestLifestyle);
            this.container.Register<ISharePointResourceService, ResourceService>(webApiRequestLifestyle);
        }

        private void RegisterSPParsers()
        {
            var webApiRequestLifestyle = new WebApiRequestLifestyle();
            this.container.Register<IResourceParser, ResourceParser>(webApiRequestLifestyle);
            this.container.Register<IReservationParser, ReservationParser>(webApiRequestLifestyle);
            this.container.Register<IDescriptionParser, DescriptionParser>(webApiRequestLifestyle);
        }

        private void RegisterSPConverters()
        {
            var webApiRequestLifestyle = new WebApiRequestLifestyle();
            this.container.Register<IConverter<IDictionary<string, object>, Resource>, ResourceConverter>(webApiRequestLifestyle);
            this.container.Register<IConverter<IDictionary<string, object>, Reservation>, ReservationConverter>(webApiRequestLifestyle);
            this.container.Register<IConverter<IDictionary<string, object>, RecurrenceData>, RecurrenceDataConverter>(webApiRequestLifestyle);
        }

        private void RegisterSPContext()
        {
            var webApiRequestLifestyle = new WebApiRequestLifestyle();
            this.container.Register(() => new ClientContext(Constants.BaseAddress), webApiRequestLifestyle);
        }
    }
}