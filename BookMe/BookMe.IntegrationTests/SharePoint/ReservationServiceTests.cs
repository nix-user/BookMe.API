using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookMe.BusinessLogic.DTO;
using BookMe.BusinessLogic.Interfaces.SharePoint;
using BookMe.BusinessLogic.Repository;
using BookMe.BusinessLogic.Services.Abstract;
using BookMe.BusinessLogic.Services.Concrete;
using BookMe.Core.Models;
using BookMe.Data.Context;
using BookMe.Data.Repository;
using BookMe.Infrastructure.MapperConfiguration;
using BookMe.ShareProint.Data;
using BookMe.ShareProint.Data.Constants;
using BookMe.ShareProint.Data.Converters.Concrete;
using BookMe.ShareProint.Data.Parsers.Concrete;
using BookMe.ShareProint.Data.Services.Concrete;
using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ResourceService = BookMe.BusinessLogic.Services.Concrete.ResourceService;

namespace BookMe.IntegrationTests.SharePoint
{
    [TestClass]
    public class ReservationServiceTests
    {
        private ReservationService reservationService;
        private IResourceService resourceService;

        [TestInitialize]
        public void Init()
        {
            AutoMapperConfiguration.Configure();

            DescriptionParser descriptionParser = new DescriptionParser();
            ResourceConverter resourceConverter = new ResourceConverter(descriptionParser);
            RecurrenceDataConverter recurrenceDataConverter = new RecurrenceDataConverter();
            ReservationConverter reservationConverter = new ReservationConverter(recurrenceDataConverter);
            ClientContext context = new ClientContext(UriConstants.BaseAddress);
            ResourceParser resourceParser = new ResourceParser(context, null);
            ReservationParser reservationParser = new ReservationParser(context, null);
            IRepository<Resource> resourcesRepository = new EFRepository<Resource>(new AppContext());
            ISharePointResourceService sharePointResourceService =
                new ShareProint.Data.Services.Concrete.ResourceService(resourceConverter, reservationConverter,
                    resourceParser, reservationParser);
            resourceService = new ResourceService(resourcesRepository, sharePointResourceService);
            this.reservationService = new ReservationService(resourceConverter, reservationConverter, resourceParser, reservationParser);
        }

        [TestMethod]
        public void GetUserActiveReservations_ShouldReturnUserReservations()
        {
            //act
            string userName = "Elena Kovalova";
            var operationResult = this.reservationService.GetUserActiveReservations(userName);

            //assert
            Assert.AreEqual(true, operationResult.IsSuccessful);
            foreach (var reservation in operationResult.Result)
            {
                Assert.AreEqual(userName, reservation.OwnerName);
            }
        }

        [TestMethod]
        public void GetPossibleReservationsInInterval_ShouldReturnAllPossiblyIntersectingReservations()
        {
            //arrange
            DateTime startDateTime = DateTime.Now;
            DateTime endDateTime = DateTime.Now.AddHours(1);
            var allResources = this.resourceService.GetAll().Result;
            //act
            var operationResult = this.reservationService.GetPossibleReservationsInInterval(new IntervalDTO(startDateTime, endDateTime), allResources);

            //assert
            Assert.AreEqual(true, operationResult.IsSuccessful);
            foreach (var possibleReservation in operationResult.Result)
            {
                if (possibleReservation.IsRecurrence)
                {
                    Assert.IsTrue(possibleReservation.EndDate > DateTime.Now);
                }
                else
                {
                    Assert.IsTrue(possibleReservation.EventDate < endDateTime && possibleReservation.EndDate > startDateTime);
                }
            }
        }
    }
}
