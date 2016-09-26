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
        [Ignore]
        [TestMethod]
        public void ToString_ReservationWithTime_ShouldReturnOnlyTimePeriodText()
        {
            // arrange
            var expectedResult = "14:30 - 17:30";

            var reservation = new Reservation()
            {
                EventDate = new DateTime(2012, 4, 5, 11, 30, 0),
                EndDate = new DateTime(2012, 4, 5, 14, 30, 0)
            };

            // act
            var result = reservation.ToString();

            // assert
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void ToString_AllDayReservation_ShouldReturnOnlyAllDayText()
        {
            // arrange
            var expectedResult = "Весь день";

            var reservation = new Reservation()
            {
                IsAllDayEvent = true
            };

            // act
            var result = reservation.ToString();

            // assert
            Assert.AreEqual(expectedResult, result);
        }
    }
}
