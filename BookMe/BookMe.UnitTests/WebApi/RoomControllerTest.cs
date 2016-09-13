using System.Collections.Generic;
using System.Linq;
using BookMe.BusinessLogic.DTO;
using BookMe.BusinessLogic.Interfaces.SharePoint;
using BookMe.BusinessLogic.OperationResult;
using BookMe.Core.Enums;
using BookMe.Infrastructure.MapperConfiguration;
using BookMe.WebApi.Controllers;
using BookMe.WebApi.Models;
using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BookMe.UnitTests.WebApi
{
    [TestClass]
    public class RoomControllertest
    {
        private RoomController controller;
        private Mock<ISharePointResourceService> resourceService;
        private OperationResult<IEnumerable<ResourceDTO>> getResourceIsSucess = new OperationResult<IEnumerable<ResourceDTO>>()
        {
            Result = new List<ResourceDTO>
            {
                new ResourceDTO() { Id = 1, Description = "1", HasPolycom = true, HasTv = false, RoomSize = RoomSizeDTO.Small, Title = "1"},
                new ResourceDTO() { Id = 2, Description = "2", HasPolycom = true, HasTv = false, RoomSize = RoomSizeDTO.Small, Title = "2" },
                new ResourceDTO() { Id = 3, Description = "3", HasPolycom = true, HasTv = false, RoomSize = RoomSizeDTO.Small, Title = "3" },
                new ResourceDTO() { Id = 4, Description = "4", HasPolycom = true, HasTv = false, RoomSize = RoomSizeDTO.Small, Title = "4" },
                new ResourceDTO() { Id = 5, Description = "5", HasPolycom = true, HasTv = false, RoomSize = RoomSizeDTO.Small, Title = "5" },
                new ResourceDTO() { Id = 6, Description = "6", HasPolycom = true, HasTv = false, RoomSize = RoomSizeDTO.Small, Title = "6" },
                new ResourceDTO() { Id = 7, Description = "7", HasPolycom = true, HasTv = false, RoomSize = RoomSizeDTO.Small, Title = "7" }
            },
            IsSuccessful = true
        };
        private OperationResult<IEnumerable<ResourceDTO>> getResourceIsFailed = new OperationResult<IEnumerable<ResourceDTO>>()
        {
            Result = new List<ResourceDTO>
            {
                new ResourceDTO() { Id = 1, Description = "1", HasPolycom = true, HasTv = false, RoomSize = RoomSizeDTO.Small, Title = "1" }
            },
            IsSuccessful = false
        };

        [TestInitialize]
        public void SetUp()
        {
            this.resourceService = new Mock<ISharePointResourceService>();
            AutoMapperConfiguration.Configure();
        }

        [TestMethod]
        public void Get_Should_Return_All_Rooms_If_Request_Was_Success()
        {
            //arrange
            this.resourceService.Setup(x => x.GetAll()).Returns(getResourceIsSucess);
            this.controller = new RoomController(resourceService.Object);
            //act
            List<Room> rooms = this.controller.Get().Result.ToList();

            //assert
            Assert.AreEqual(7, rooms.Count);
        }

        [TestMethod]
        public void Get_Should_Return_Null_If_Request_Was_Failed()
        {
            //arrange
            this.resourceService.Setup(x => x.GetAll()).Returns(getResourceIsFailed);
            this.controller = new RoomController(resourceService.Object);
            //act
           var rooms = this.controller.Get();

            //assert
            Assert.AreEqual(false, rooms.IsOperationSuccessful);
        }

        [TestMethod]
        public void Get_Should_Return_OneRoom_IfOperationIsSucces()
        {
            //arrange
            this.resourceService.Setup(x => x.GetAll()).Returns(getResourceIsSucess);
            this.controller = new RoomController(resourceService.Object);
            //act
            Room room = this.controller.Get(1).Result;

            //assert
            Assert.AreEqual(getResourceIsSucess.Result.First().Id, room.Id);
            Assert.AreEqual(getResourceIsSucess.Result.First().HasPolycom, room.IsHasPolykom);
        }

        [TestMethod]
        public void Get_Should_Return_OneRoom_IfOperationIsFailed()
        {
            this.resourceService.Setup(x => x.GetAll()).Returns(getResourceIsFailed);
            this.controller = new RoomController(resourceService.Object);
            //act
            Room room = this.controller.Get(1).Result;

            //assert
            Assert.AreEqual(null, room);
        }
    }
}
