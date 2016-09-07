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
            var converter = new ReservationConverter();
            ClientContext context = new ClientContext(Constants.BaseAddress);
            var reservationParser = new ReservationParser(context);
            this.reservationService = new ReservationService(converter, reservationParser);
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
    }
}
