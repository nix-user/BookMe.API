using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookMe.Infrastructure.MapperConfiguration;
using BookMe.ShareProint.Data;
using BookMe.ShareProint.Data.Converters.Concrete;
using BookMe.ShareProint.Data.Parsers.Concrete;
using BookMe.ShareProint.Data.Services.Concrete;
using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BookMe.IntegrationTests.SharePoint
{
    [TestClass]
    public class ReservationServiceTests
    {
        private ReservationService reservationService;

        [TestInitialize]
        public void Init()
        {
            AutoMapperConfiguration.Configure();

            DescriptionParser descriptionParser = new DescriptionParser();
            ResourceConverter resourceConverter = new ResourceConverter(descriptionParser);
            ReservationConverter reservationConverter = new ReservationConverter();
            ClientContext context = new ClientContext(Constants.BaseAddress);
            ResourceParser resourceParser = new ResourceParser(context);
            ReservationParser reservationParser = new ReservationParser(context);
            var resourceService = new ResourceService(resourceConverter, reservationConverter, resourceParser, reservationParser);
            this.reservationService = new ReservationService(reservationConverter, reservationParser, resourceService);
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

            //act
            var operationResult = this.reservationService.GetPossibleReservationsInInterval(startDateTime, endDateTime);

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
