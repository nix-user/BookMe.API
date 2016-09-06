using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            this.resourceService = new ResourceService(resourceConverter, resourceParser);
        }

        [TestMethod]
        public void GetAll_ShouldSuccessfullyReturnAllResources()
        {
            var operationResult = this.resourceService.GetAll();
            Assert.AreEqual(true, operationResult.IsSuccessful);
        }
    }
}
