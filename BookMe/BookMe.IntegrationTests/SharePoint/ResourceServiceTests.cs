using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookMe.BusinessLogic.DTO;
using BookMe.ShareProint.Data;
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

        [TestInitialize]
        public void Init()
        {
            DescriptionParser descriptionParser = new DescriptionParser();
            ResourceConverter resourceConverter = new ResourceConverter(descriptionParser);
            ClientContext context = new ClientContext(Constants.BaseAddress);
            ResourceParser resourceParser = new ResourceParser(context);
            ReservationConverter reservationConverter = new ReservationConverter();
            ReservationParser reservationParser = new ReservationParser(context);
            this.resourceService = new ResourceService(resourceConverter, reservationConverter, resourceParser, reservationParser);
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
        public void GetAvailableResources_ShouldReturnAllAvailableResources()
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
            Assert.IsTrue(operationResult.IsSuccessful);
        }
    }
}
