using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookMe.BusinessLogic.DTO;
using BookMe.Infrastructure.MapperConfiguration;
using BookMe.ShareProint.Data;
using BookMe.ShareProint.Data.Constants;
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
        private ResourceService resourceService;

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
            this.reservationService = new ReservationService(resourceConverter, reservationConverter, resourceParser, reservationParser);
            this.resourceService = new ResourceService(resourceConverter, reservationConverter, resourceParser, reservationParser);
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
            var operationResult = this.reservationService.GetPossibleReservationsInInterval(new IntervalDTO(startDateTime, endDateTime));

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
