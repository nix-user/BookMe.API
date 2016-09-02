using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookMe.Core.Models;
using BookMe.ShareProint.Data.Converters.Concrete;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BookMe.UnitTests.SharePoint.Converters
{
    [TestClass]
    public class DescriptionParserTests
    {
        private const string DescriptionHasPolycom = "Polycom";
        private const string DescriptionHasTv = "TV";
        private const string EmptyDescription = "";
        private const string DescriptionHasSmallSize = "Size:S";
        private const string DescriptionHasMiddleSize = "Size:M";
        private const string DescriptionHasLargeSize = "Size:L";

        [TestMethod]
        public void HasPolycom_ArgumentNull_ShouldReturnFalse()
        {
            // arrange
            var parser = new DescriptionParser();

            // act
            var result = parser.HasPolycom(null);

            // assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void HasPolycom_DescriptionHasPolycom_ShouldReturnTrue()
        {
            // arrange
            var parser = new DescriptionParser();

            // act
            var result = parser.HasPolycom(DescriptionHasPolycom);

            // assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void HasPolycom_DescriptionDoesNotHavePolycom_ShouldReturnFalse()
        {
            // arrange
            var parser = new DescriptionParser();

            // act
            var result = parser.HasPolycom(EmptyDescription);

            // assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void HasTv_ArgumentNull_ShouldReturnFalse()
        {
            // arrange
            var parser = new DescriptionParser();

            // act
            var result = parser.HasTv(null);

            // assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void HasTv_DescriptionHasTv_ShouldReturnTrue()
        {
            // arrange
            var parser = new DescriptionParser();

            // act
            var result = parser.HasTv(DescriptionHasTv);

            // assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void HasTv_DescriptionDoesNotHaveTv_ShouldReturnFalse()
        {
            // arrange
            var parser = new DescriptionParser();

            // act
            var result = parser.HasTv(EmptyDescription);

            // assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ParseRoomSize_ArgumentNull_ShouldReturnNull()
        {
            // arrange
            var parser = new DescriptionParser();

            // act
            var result = parser.ParseRoomSize(null);

            // assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void ParseRoomSize_DescriptionHasSmallSize_ShouldReturnSmallSize()
        {
            // arrange
            var parser = new DescriptionParser();

            // act
            var result = parser.ParseRoomSize(DescriptionHasSmallSize);

            // assert
            Assert.AreEqual(RoomSize.Small, result.Value);
        }

        [TestMethod]
        public void ParseRoomSize_DescriptionHasMiddleSize_ShouldReturnMiddleSize()
        {
            // arrange
            var parser = new DescriptionParser();

            // act
            var result = parser.ParseRoomSize(DescriptionHasMiddleSize);

            // assert
            Assert.AreEqual(RoomSize.Middle, result.Value);
        }

        [TestMethod]
        public void ParseRoomSize_DescriptionHasLargeSize_ShouldReturnLargeSize()
        {
            // arrange
            var parser = new DescriptionParser();

            // act
            var result = parser.ParseRoomSize(DescriptionHasLargeSize);

            // assert
            Assert.AreEqual(RoomSize.Large, result.Value);
        }
    }
}
