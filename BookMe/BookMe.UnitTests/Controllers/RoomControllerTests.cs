using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookMe.BusinessLogic.DTO;
using BookMe.BusinessLogic.Interfaces.SharePoint;
using BookMe.BusinessLogic.OperationResult;
using BookMe.BusinessLogic.Services.Abstract;
using BookMe.WebApi.Controllers;
using BookMe.WebApi.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BookMe.UnitTests.Controllers
{
    [TestClass]
    public class RoomControllerTests
    {
        private Mock<ISharePointResourceService> SPResourceServiceMock;
        private Mock<IResourceService> resourceService;
        private RoomController roomController;

        [TestInitialize]
        public void Init()
        {
            this.SPResourceServiceMock = new Mock<ISharePointResourceService>();
            this.resourceService = new Mock<IResourceService>();
            this.roomController = new RoomController(this.SPResourceServiceMock.Object, resourceService.Object);
            this.resourceService.Setup(m => m.GetAll()).Returns(new OperationResult<IEnumerable<ResourceDTO>>() { IsSuccessful = true, Result = new List<ResourceDTO>() });
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
            this.SPResourceServiceMock.Setup(m => m.GetRoomsReservations(It.IsAny<IntervalDTO>(), It.IsAny<IEnumerable<ResourceDTO>>())).Returns(roomReservationsResult);

            //act
            var roomReservationsRetrieval = this.roomController.GetRoomCurrentReservations(new RoomReservationsRequestModel());

            //assert
            this.SPResourceServiceMock.Verify(m => m.GetRoomsReservations(It.IsAny<IntervalDTO>(), It.IsAny<IEnumerable<ResourceDTO>>()), Times.Once);
            Assert.AreEqual(roomReservationsResult.Result.Count(), roomReservationsRetrieval.Result.Count());
        }

        [TestMethod]
        public void GetRoomCurrentReservations_Should_Return_Fail_Status_If_Request_Failed()
        {
            //arrange
            var roomReservationsResult = new OperationResult<IEnumerable<ReservationDTO>> { IsSuccessful = false };
            this.SPResourceServiceMock.Setup(m => m.GetRoomsReservations(It.IsAny<IntervalDTO>(), It.IsAny<IEnumerable<ResourceDTO>>())).Returns(roomReservationsResult);

            //act
            var roomReservationsRetrieval = this.roomController.GetRoomCurrentReservations(new RoomReservationsRequestModel());

            //assert
            this.SPResourceServiceMock.Verify(m => m.GetRoomsReservations(It.IsAny<IntervalDTO>(), It.IsAny<IEnumerable<ResourceDTO>>()), Times.Once);
            Assert.AreEqual(false, roomReservationsRetrieval.IsOperationSuccessful);
            Assert.IsNull(roomReservationsRetrieval.Result);
        }
    }
}
