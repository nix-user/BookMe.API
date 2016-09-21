using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookMe.BusinessLogic.DTO;
using BookMe.BusinessLogic.Interfaces.SharePoint;
using BookMe.BusinessLogic.OperationResult;
using BookMe.BusinessLogic.Services.Abstract;
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
        private Mock<IReservationService> reservationServiceMock;

        [TestInitialize]
        public void Init()
        {
            AutoMapperConfiguration.Configure();
            this.sharePointReservationServiceMock = new Mock<ISharePointReservationService>();
            this.reservationServiceMock = new Mock<IReservationService>();
            for (int i = 0; i < 10; i++)
            {
                this.reservationsList.Add(new ReservationDTO());
            }

            this.reservationController = new ReservationController(this.sharePointReservationServiceMock.Object, this.reservationServiceMock.Object);
        }

        [TestMethod]
        public void GetUserReservations_Should_Return_All_Reservations_If_Request_Was_Successful()
        {
            //arrange
            var userActiveReservationResult = new OperationResult<IEnumerable<ReservationDTO>> { IsSuccessful = true, Result = reservationsList };
            this.sharePointReservationServiceMock.Setup(m => m.GetUserActiveReservations(It.IsAny<string>())).Returns(userActiveReservationResult);

            //act
            var userReservations = this.reservationController.Get();

            //assert
            this.sharePointReservationServiceMock.Verify(m => m.GetUserActiveReservations(It.IsAny<string>()), Times.Once);
            Assert.AreEqual(this.reservationsList.Count, userReservations.Result.Count());
        }

        [TestMethod]
        public void GetUserReservations_Should_Return_Failed_Status_If_Request_Failed()
        {
            //arrange
            var userActiveReservationResult = new OperationResult<IEnumerable<ReservationDTO>> { IsSuccessful = false };
            this.sharePointReservationServiceMock.Setup(m => m.GetUserActiveReservations(It.IsAny<string>())).Returns(userActiveReservationResult);

            //act
            var userReservationsRetrieval = this.reservationController.Get();

            //assert
            this.sharePointReservationServiceMock.Verify(m => m.GetUserActiveReservations(It.IsAny<string>()), Times.Once);
            Assert.AreEqual(false, userReservationsRetrieval.IsOperationSuccessful);
            Assert.IsNull(userReservationsRetrieval.Result);
        }

        [TestMethod]
        public void GetCurrentUserReservations_Should_Return_User_Reservations_Groups_If_Request_Was_Successful()
        {
            //arrange
            var userReservationsOperationResult = new OperationResult<UserReservationsDTO>()
            {
                IsSuccessful = true,
                Result = new UserReservationsDTO()
                {
                    TodayReservations = new List<ReservationDTO>()
                    {
                        new ReservationDTO() {Id = 1},
                        new ReservationDTO() {Id = 2},
                    },
                    AllReservations = new List<ReservationDTO>()
                    {
                        new ReservationDTO() {Id = 3},
                        new ReservationDTO() {Id = 4},
                        new ReservationDTO() {Id = 5}
                    },
                }
            };

            this.reservationServiceMock.Setup(m => m.GetUserReservations(It.IsAny<string>())).Returns(userReservationsOperationResult);

            //act
            var userReservations = this.reservationController.GetCurrentUserReservations();

            //assert
            this.reservationServiceMock.Verify(m => m.GetUserReservations(It.IsAny<string>()), Times.AtLeastOnce);
            Assert.IsTrue(userReservations.IsOperationSuccessful);
            Assert.AreEqual(userReservationsOperationResult.Result.TodayReservations.Count(), userReservations.Result.TodayReservations.Count());
            Assert.AreEqual(userReservationsOperationResult.Result.AllReservations.Count(), userReservations.Result.AllReservations.Count());
            foreach (var reservation in userReservations.Result.TodayReservations)
            {
                Assert.IsTrue(userReservationsOperationResult.Result.TodayReservations.Any(r => r.Id == reservation.Id));
            }
            foreach (var reservation in userReservations.Result.AllReservations)
            {
                Assert.IsTrue(userReservationsOperationResult.Result.AllReservations.Any(r => r.Id == reservation.Id));
            }
        }

        [TestMethod]
        public void GetCurrentUserReservations_Should_Return_Failed_Status_If_Operation_Failed()
        {
            //arrange
            var userReservationsOperationResult = new OperationResult<UserReservationsDTO>() { IsSuccessful = false };

            this.reservationServiceMock.Setup(m => m.GetUserReservations(It.IsAny<string>())).Returns(userReservationsOperationResult);

            //act
            var userReservations = this.reservationController.GetCurrentUserReservations();

            //assert
            this.reservationServiceMock.Verify(m => m.GetUserReservations(It.IsAny<string>()), Times.AtLeastOnce);
            Assert.IsFalse(userReservations.IsOperationSuccessful);
        }
    }
}
