using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using BookMe.BusinessLogic.DTO;
using BookMe.Core.Enums;
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
    public class ResourceServiceTests
    {
        private ResourceService resourceService;
        private ReservationService reservationService;

        [TestInitialize]
        public void Init()
        {
            AutoMapperConfiguration.Configure();

            DescriptionParser descriptionParser = new DescriptionParser();
            ResourceConverter resourceConverter = new ResourceConverter(descriptionParser);
            ClientContext context = new ClientContext(UriConstants.BaseAddress);
            ResourceParser resourceParser = new ResourceParser(context, null);
            RecurrenceDataConverter recurrenceDataConverter = new RecurrenceDataConverter();
            ReservationConverter reservationConverter = new ReservationConverter(recurrenceDataConverter);
            ReservationParser reservationParser = new ReservationParser(context, null);
            this.resourceService = new ResourceService(resourceConverter, reservationConverter, resourceParser, reservationParser);
            this.reservationService = new ReservationService(resourceConverter, reservationConverter, resourceParser, reservationParser);
        }

        [TestMethod]
        public void GetAll_ShouldSuccessfullyReturnAllResources()
        {
            //act
            var operationResult = this.resourceService.GetAll();

            //assert
            Assert.IsTrue(operationResult.IsSuccessful);
        }

        [TestMethod]
        public void GetAvailableResourcesWithPolycom_ShouldReturnAllAvailableResourcesWithPolycom()
        {
            //arrange
            var filterParameters = new ResourceFilterParameters()
            {
                From = DateTime.Now,
                To = DateTime.Now.AddHours(1),
                HasPolycom = true
            };

            //act
            var operationResult = this.resourceService.GetAvailbleResources(filterParameters);

            //assert
            Assert.IsTrue(operationResult.IsSuccessful);
            foreach (var resource in operationResult.Result)
            {
                Assert.IsTrue(resource.HasPolycom);
            }
        }

        [TestMethod]
        public void GetAvailableResourcesWithLargeRoomSize_ShouldReturnAllAvailableResourcesWithLargeRoomSize()
        {
            //arrange
            var filterParameters = new ResourceFilterParameters()
            {
                From = DateTime.Now,
                To = DateTime.Now.AddHours(1),
                RoomSize = RoomSizeDTO.Large
            };

            //act
            var operationResult = this.resourceService.GetAvailbleResources(filterParameters);

            //assert
            Assert.IsTrue(operationResult.IsSuccessful);
            foreach (var resource in operationResult.Result)
            {
                Assert.IsTrue(resource.RoomSize == RoomSizeDTO.Large);
            }
        }

        [TestMethod]
        public void GetAvailableResources_ShouldReturnOnlyAvailableResourcesInInterval()
        {
            //arrange 
            var filterParameters = new ResourceFilterParameters()
            {
                From = DateTime.Now,
                To = DateTime.Now.AddHours(1)
            };

            //act
            var operationResult = this.resourceService.GetAvailbleResources(filterParameters);

            //assert
            var allIntersectingReservations = reservationService.GetPossibleReservationsInInterval(new IntervalDTO(filterParameters.From, filterParameters.To)).Result.ToList();
            Assert.IsTrue(operationResult.IsSuccessful);
            foreach (var resource in operationResult.Result)
            {
                Assert.IsFalse(allIntersectingReservations.Any(reservation => reservation.Resource != null && reservation.Resource.Id == resource.Id));
            }
        }

        [TestMethod]
        public void GetRoomReservations_ShouldReturnOnlyRequestedRoomReservations()
        {
            //arrange
            var roomId = this.resourceService.GetAll().Result.Last().Id;
            DateTime intervalStart = DateTime.Now;
            DateTime intervalEnd = DateTime.Now.AddHours(1);

            //act
            var operationResult = this.resourceService.GetRoomReservations(new IntervalDTO(intervalStart, intervalEnd), roomId);

            //assert
            Assert.IsTrue(operationResult.IsSuccessful);
            foreach (var reservation in operationResult.Result)
            {
                Assert.IsTrue(reservation.Resource.Id == roomId);
            }
        }

        [TestMethod]
        public void GetRoomReservations_ShouldReturnOnlyReservationsInInterval()
        {
            //arrange 
            var roomId = this.resourceService.GetAll().Result.Last().Id;
            DateTime intervalStart = DateTime.Now;
            DateTime intervalEnd = DateTime.Now.AddHours(1);

            //act
            var operationResult = this.resourceService.GetRoomReservations(new IntervalDTO(intervalStart, intervalEnd), roomId);

            //assert
            Assert.IsTrue(operationResult.IsSuccessful);
            foreach (var reservation in operationResult.Result)
            {
                Assert.IsTrue(reservation.EventDate.Date <= intervalEnd.Date && reservation.EndDate.Date >= intervalStart.Date);
            }
        }

    }
}
