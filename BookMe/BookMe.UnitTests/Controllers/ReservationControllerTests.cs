using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookMe.BusinessLogic.DTO;
using BookMe.BusinessLogic.Interfaces.SharePoint;
using BookMe.BusinessLogic.OperationResult;
using BookMe.WebApi.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BookMe.UnitTests.Controllers
{
    [TestClass]
    public class ReservationControllerTests
    {
        private ReservationController reservationController;
        private Mock<ISharePointReservationService> sharePointReservationServiceMock;

        [TestInitialize]
        public void Init()
        {
            this.sharePointReservationServiceMock = new Mock<ISharePointReservationService>();
            this.sharePointReservationServiceMock.Setup(m => m.GetUserActiveReservations(It.IsAny<string>()))
                .Returns(new OperationResult<IEnumerable<ReservationDTO>>());
            this.reservationController = new ReservationController(this.sharePointReservationServiceMock.Object);
        }

        [TestMethod]
        public void GetUserReservations_Should_Call_ReservationService_GetUserActiveReservations()
        {
            //act
            this.reservationController.GetUserReservations(string.Empty);

            //assert
            this.sharePointReservationServiceMock.Verify(m => m.GetUserActiveReservations(It.IsAny<string>()), Times.Once);
        }
    }
}
