using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookMe.BusinessLogic.DTO;
using BookMe.BusinessLogic.Interfaces.SharePoint;
using BookMe.BusinessLogic.OperationResult;
using BookMe.WebApi.Controllers;
using BookMe.WebApi.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BookMe.UnitTests.Controllers
{
    [TestClass]
    public class RoomControllerTests
    {
        private Mock<ISharePointResourceService> resourceServiceMock;
        private RoomController roomController;

        [TestInitialize]
        public void Init()
        {
            this.resourceServiceMock = new Mock<ISharePointResourceService>();
            this.roomController = new RoomController(this.resourceServiceMock.Object);
        }

        [TestMethod]
        public void GetRoomCurrentReservations_Should_Return_Current_Room_Reservations_If_Request_Was_Successful()
        {
            //arrange
            var roomReservationsResult = new OperationResult<IEnumerable<ReservationDTO>>
            {
                IsSuccessful = true,
                Result = new List<ReservationDTO>() { new ReservationDTO(), new ReservationDTO(), new ReservationDTO() }
            };
            this.resourceServiceMock.Setup(m => m.GetRoomReservations(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>())).Returns(roomReservationsResult);

            //act
            var roomReservations = this.roomController.GetRoomCurrentReservations(new RoomReservationsRequestModel());

            //assert
            this.resourceServiceMock.Verify(m => m.GetRoomReservations(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>()), Times.Once);
            Assert.AreEqual(roomReservationsResult.Result.Count(), roomReservations.Count());
        }

        [TestMethod]
        public void GetRoomCurrentReservations_Should_Return_Null_If_Request_Failed()
        {
            //arrange
            var roomReservationsResult = new OperationResult<IEnumerable<ReservationDTO>> { IsSuccessful = false };
            this.resourceServiceMock.Setup(m => m.GetRoomReservations(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>())).Returns(roomReservationsResult);

            //act
            var roomReservations = this.roomController.GetRoomCurrentReservations(new RoomReservationsRequestModel());

            //assert
            this.resourceServiceMock.Verify(m => m.GetRoomReservations(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>()), Times.Once);
            Assert.IsNull(roomReservations);
        }
    }
}
