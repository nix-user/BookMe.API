using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookMe.BusinessLogic.DTO;
using BookMe.BusinessLogic.Interfaces.SharePoint;
using BookMe.BusinessLogic.OperationResult;
using BookMe.Infrastructure.MapperConfiguration;
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
        private List<ReservationDTO> reservationsList = new List<ReservationDTO>();

        [TestInitialize]
        public void Init()
        {
            AutoMapperConfiguration.Configure();
            this.sharePointReservationServiceMock = new Mock<ISharePointReservationService>();
            for (int i = 0; i < 10; i++)
            {
                this.reservationsList.Add(new ReservationDTO());
            }
            var userActiveReservationResult = new OperationResult<IEnumerable<ReservationDTO>> { IsSuccessful = true, Result = reservationsList };
            this.sharePointReservationServiceMock.Setup(m => m.GetUserActiveReservations(It.IsAny<string>())).Returns(userActiveReservationResult);
            this.reservationController = new ReservationController(this.sharePointReservationServiceMock.Object);
        }

        [TestMethod]
        public void GetUserReservations_Should_Return_All_Reservations()
        {
            //act
            var userReservations = this.reservationController.GetUserReservations(string.Empty);

            //assert
            this.sharePointReservationServiceMock.Verify(m => m.GetUserActiveReservations(It.IsAny<string>()), Times.Once);
            Assert.AreEqual(this.reservationsList.Count, userReservations.Count());
        }
    }
}
