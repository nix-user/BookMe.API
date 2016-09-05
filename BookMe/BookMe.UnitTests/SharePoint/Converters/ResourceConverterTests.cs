using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookMe.Core.Enums;
using BookMe.ShareProint.Data.Converters.Abstract;
using BookMe.ShareProint.Data.Converters.Concrete;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BookMe.UnitTests.SharePoint.Converters
{
    [TestClass]
    public class ResourceConverterTests
    {
        private const string IdKey = "ID";
        private const string TitleKey = "Title";
        private const string DescriptionKey = "Description";

        private const RoomSize ExpectedRoomSize = RoomSize.Small;
        private const bool ExpectedHasPolycom = true;
        private const bool ExpectedHasTv = false;
        private const string ExpectedDescription = "Size: S, Polycom";

        private IDescriptionParser ArrangeDescriptionParser()
        {
            var descriptionParserMock = new Mock<IDescriptionParser>();

            descriptionParserMock.Setup(x => x.ParseRoomSize(ExpectedDescription)).Returns(ExpectedRoomSize);
            descriptionParserMock.Setup(x => x.HasPolycom(ExpectedDescription)).Returns(ExpectedHasPolycom);
            descriptionParserMock.Setup(x => x.HasTv(ExpectedDescription)).Returns(ExpectedHasTv);

            return descriptionParserMock.Object;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Convert_ArgumentNull_ShouldThrowArgumentNullException()
        {
            // arrange
            var descriptionParser = this.ArrangeDescriptionParser();
            var resourceConverter = new ResourceConverter(descriptionParser);

            // act
            resourceConverter.Convert((IDictionary<string, object>) null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Convert_IEnumerableArgumentNull_ShouldThrowArgumentNullException()
        {
            // arrange
            var descriptionParser = this.ArrangeDescriptionParser();
            var resourceConverter = new ResourceConverter(descriptionParser);

            // act
            resourceConverter.Convert((IEnumerable<IDictionary<string, object>>) null);
        }

        [TestMethod]
        public void Convert_DictionaryWithNeededProperties_PropertiesMappedRightOnModel()
        {
            // arrange
            var descriptionParser = this.ArrangeDescriptionParser();
            var resourceConverter = new ResourceConverter(descriptionParser);

            const int expectedId = 1;
            const string expectedTitle = "title1";

            var value = new Dictionary<string, object>()
            {
                {IdKey, expectedId},
                {TitleKey, expectedTitle},
                {DescriptionKey, ExpectedDescription}
            };

            // act
            var result = resourceConverter.Convert(value);

            // assert 
            Assert.AreEqual(expectedId, result.Id);
            Assert.AreEqual(expectedTitle, result.Title);
            Assert.AreEqual(ExpectedDescription, result.Description);
            Assert.AreEqual(ExpectedRoomSize, result.RoomSize);
            Assert.AreEqual(ExpectedHasPolycom, ExpectedHasPolycom);
            Assert.AreEqual(ExpectedHasTv, ExpectedHasTv);
        }

        [TestMethod]
        public void Convert_DictionaryCollectionWithNeededProperties_PropertiesMappedRightOnModel()
        {
            // arrange
            var descriptionParser = this.ArrangeDescriptionParser();
            var resourceConverter = new ResourceConverter(descriptionParser);

            const int expectedId1 = 1;
            const string expectedTitle1 = "title1";
            const int expectedId2 = 2;
            const string expectedTitle2 = "title2";

            var value1 = new Dictionary<string, object>()
            {
                {IdKey, expectedId1},
                {TitleKey, expectedTitle1},
                {DescriptionKey, ExpectedDescription}
            };

            var value2 = new Dictionary<string, object>()
            {
                {IdKey, expectedId2},
                {TitleKey, expectedTitle2},
                {DescriptionKey, ExpectedDescription}
            };

            var dictionaries = new List<Dictionary<string, object>>() {value1, value2};

            // act
            var result = resourceConverter.Convert(dictionaries);

            // assert 
            var resourcesList = result.ToList();

            Assert.AreEqual(dictionaries.Count, result.Count());

            Assert.AreEqual(expectedId1, resourcesList[0].Id);
            Assert.AreEqual(expectedTitle1, resourcesList[0].Title);
            Assert.AreEqual(ExpectedDescription, resourcesList[0].Description);
            Assert.AreEqual(ExpectedRoomSize, resourcesList[0].RoomSize);
            Assert.AreEqual(ExpectedHasPolycom, ExpectedHasPolycom);
            Assert.AreEqual(ExpectedHasTv, ExpectedHasTv);

            Assert.AreEqual(expectedId2, resourcesList[1].Id);
            Assert.AreEqual(expectedTitle2, resourcesList[1].Title);
            Assert.AreEqual(ExpectedDescription, resourcesList[1].Description);
            Assert.AreEqual(ExpectedRoomSize, resourcesList[1].RoomSize);
            Assert.AreEqual(ExpectedHasPolycom, ExpectedHasPolycom);
            Assert.AreEqual(ExpectedHasTv, ExpectedHasTv);
        }
    }
}
