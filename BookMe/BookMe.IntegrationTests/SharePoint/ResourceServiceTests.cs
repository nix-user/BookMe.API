using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using BookMe.BusinessLogic.DTO;
using BookMe.BusinessLogic.Repository;
using BookMe.Core.Enums;
using BookMe.Core.Models;
using BookMe.Data.Context;
using BookMe.Data.Repository;
using BookMe.Infrastructure.MapperConfiguration;
using BookMe.ShareProint.Data;
using BookMe.ShareProint.Data.Constants;
using BookMe.ShareProint.Data.Converters.Concrete;
using BookMe.ShareProint.Data.Parsers.Concrete;
using BookMe.ShareProint.Data.Services.Concrete;
using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BLLServices = BookMe.BusinessLogic.Services.Concrete;
namespace BookMe.IntegrationTests.SharePoint
{
    [TestClass]
    public class ResourceServiceTests
    {
        private ResourceService SPResourceService;
        private ReservationService reservationService;
        private BLLServices.ResourceService resourceService;

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
            IRepository<Resource> resourcesRepository = new EFRepository<Resource>(new AppContext());
            this.SPResourceService = new ResourceService(resourceConverter, reservationConverter, resourceParser, reservationParser);
            this.resourceService = new BLLServices.ResourceService(resourcesRepository, this.SPResourceService);
            this.reservationService = new ReservationService(resourceConverter, reservationConverter, resourceParser, reservationParser);
        }

        [TestMethod]
        public void GetAll_ShouldSuccessfullyReturnAllResources()
        {
            //act
            var operationResult = this.SPResourceService.GetAll();

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
            var allResources = this.resourceService.GetAll().Result;

            //act
            var operationResult = this.SPResourceService.GetAvailableResources(filterParameters, allResources);

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
            var allResources = this.resourceService.GetAll().Result;

            //act           
            var operationResult = this.SPResourceService.GetAvailableResources(filterParameters, allResources);

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
                From = DateTime.Today.AddHours(16),
                To = DateTime.Today.AddHours(17)
            };
            var allResources = this.resourceService.GetAll().Result.ToList();

            //act
            var operationResult = this.SPResourceService.GetAvailableResources(filterParameters, allResources);
            var interval = new IntervalDTO(filterParameters.From, filterParameters.To);

            //assert
            var allIntersectingReservations = reservationService.GetPossibleReservationsInInterval(interval, allResources).Result.ToList();
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
            var rooms = this.resourceService.GetAll().Result;
            var interval = new IntervalDTO(DateTime.Now, DateTime.Now.AddHours(1));

            //act
            var operationResult = this.SPResourceService.GetRoomsReservations(interval, rooms);

            //assert
            Assert.IsTrue(operationResult.IsSuccessful);
            foreach (var reservation in operationResult.Result)
            {
                Assert.IsTrue(rooms.Any(r => r.Id == reservation.Resource.Id));
            }
        }

        [TestMethod]
        public void GetRoomReservations_ShouldReturnOnlyReservationsInInterval()
        {
            //arrange 
            var rooms = this.resourceService.GetAll().Result;
            var interval = new IntervalDTO(DateTime.Now, DateTime.Now.AddHours(1));

            //act
            var operationResult = this.SPResourceService.GetRoomsReservations(interval, rooms);

            //assert
            Assert.IsTrue(operationResult.IsSuccessful);
            foreach (var reservation in operationResult.Result)
            {
                Assert.IsTrue(reservation.EventDate.Date <= interval.End.Date && reservation.EndDate.Date >= interval.Start.Date);
            }
        }

    }
}
