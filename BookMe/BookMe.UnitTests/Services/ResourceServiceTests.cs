using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookMe.BusinessLogic.DTO;
using BookMe.BusinessLogic.Services.Concrete;
using BookMe.Core.Models;
using BookMe.Infrastructure.MapperConfiguration;
using BookMe.UnitTests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BookMe.UnitTests.Services
{
    [TestClass]
    public class ResourceServiceTests
    {
        [TestInitialize]
        public void Init()
        {
            AutoMapperConfiguration.Configure();
        }

        [TestMethod]
        public void GetAll_ShouldReturnAllTheResources()
        {
            // arrange
            var expectedIsSuccess = true;

            var resources = new List<Resource>
            {
                new Resource { Id = 1 },
                new Resource { Id = 2 },
                new Resource { Id = 3 }
            };

            var fakeProfileRepository = new FakeRepository<Resource>(resources);
            var resourceService = new ResourceService(fakeProfileRepository, null);

            // act
            var result = resourceService.GetAll();

            // assert 
            var resultList = result.Result.ToList();

            Assert.AreEqual(expectedIsSuccess, result.IsSuccessful);
            Assert.AreEqual(resources.Count, resultList.Count);

            for (var i = 0; i < resources.Count; i++)
            {
                Assert.AreEqual(resources[i].Id, resultList[i].Id);
            }
        }

        [TestMethod]
        public void AddResource_NewResource_ResourceShouldBeAdded()
        {
            // arrange
            var expectedIsSuccess = true;

            var fakeProfileRepository = new FakeRepository<Resource>();
            var resourceService = new ResourceService(fakeProfileRepository, null);

            var resourceToAdd = new ResourceDTO { Id = 1 };
            // act
            var result = resourceService.AddResource(resourceToAdd);

            // assert 
            var getAllResult = resourceService.GetAll();

            Assert.AreEqual(expectedIsSuccess, result.IsSuccessful);
            Assert.IsTrue(getAllResult.Result.Select(x => x.Id).Contains(resourceToAdd.Id));
        }
    }
}
