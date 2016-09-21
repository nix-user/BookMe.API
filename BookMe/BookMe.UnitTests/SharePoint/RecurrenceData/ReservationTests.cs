using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookMe.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RecurrenceDataType = BookMe.Core.Models.Recurrence.RecurrenceData;

namespace BookMe.UnitTests.SharePoint.RecurrenceData
{
    [TestClass]
    public class ReservationTests
    {
        [TestMethod]
        public void TextPeriod_NotResurrenceWithTime_ShouldReturnOnlyTimePeriodText()
        {
            // arrange
            var expectedResult = "14:30 - 17:30. ";

            var reservation = new Reservation()
            {
                EventDate = new DateTime(2012, 4, 5, 11, 30, 0),
                EndDate = new DateTime(2012, 4, 5, 14, 30, 0)
            };

            // act
            var result = reservation.TextPeriod;

            // assert
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void TextPeriod_NotResurrenceAllDay_ShouldReturnOnlyAllDayText()
        {
            // arrange
            var expectedResult = "Весь день. ";

            var reservation = new Reservation()
            {
                IsAllDayEvent = true
            };

            // act
            var result = reservation.TextPeriod;

            // assert
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void TextPeriod_ResurrenceWithTime_ShouldReturnTimePeriodAndRecurrenceDataText()
        {
            // arrange
            var expectedRecurrenceDataToString = "Каждую 2 неделю, каждый день.";
            var expectedResult = "14:30 - 17:30. " + expectedRecurrenceDataToString;

            var recurrenceDataMock = new Mock<RecurrenceDataType>();
            recurrenceDataMock
                .Setup(x => x.ToString())
                .Returns(expectedRecurrenceDataToString);

            var reservation = new Reservation()
            {
                EventDate = new DateTime(2012, 4, 5, 11, 30, 0),
                EndDate = new DateTime(2012, 4, 5, 14, 30, 0),
                RecurrenceData = recurrenceDataMock.Object,
                IsRecurrence = true
            };

            // act
            var result = reservation.TextPeriod;

            // assert
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void TextPeriod_ResurrenceAllDay_ShouldReturnAllDayAndRecurrenceDataText()
        {
            // arrange
            var expectedRecurrenceDataToString = "Каждую 2 неделю, каждый день.";
            var expectedResult = "Весь день. " + expectedRecurrenceDataToString;

            var recurrenceDataMock = new Mock<RecurrenceDataType>();
            recurrenceDataMock
                .Setup(x => x.ToString())
                .Returns(expectedRecurrenceDataToString);

            var reservation = new Reservation()
            {
                IsAllDayEvent = true,
                RecurrenceData = recurrenceDataMock.Object,
                IsRecurrence = true
            };

            // act
            var result = reservation.TextPeriod;

            // assert
            Assert.AreEqual(expectedResult, result);
        }
    }
}
