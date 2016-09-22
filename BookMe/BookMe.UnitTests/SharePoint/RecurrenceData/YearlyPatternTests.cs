using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookMe.Core.Enums;
using BookMe.Core.Models.Recurrence;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BookMe.UnitTests.SharePoint.RecurrenceData
{
    [TestClass]
    public class YearlyPatternTests
    {
        [TestMethod]
        public void IsBusyInDate_IntervalMonthDayOfMonthAndNumberOfOccurancesCaseWithBusyDate_ShouldReturnTrue()
        {
            // arrange 
            var today = new DateTime(2016, 4, 23);

            var pattern = new YearlyPattern()
            {
                Interval = 2,
                NumberOfOccurrences = 10,
                StartDate = new DateTime(2016, 3, 8),
                DayOfMonth = 23,
                Month = Month.April
            };

            // act
            var result = pattern.IsBusyInDate(today);

            // assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsBusyInDate_IntervalMonthDayOfMonthAndNumberOfOccurancesCaseWithNotBusyDate_ShouldReturnFalse()
        {
            // arrange 
            var today = new DateTime(2016, 4, 20);

            var pattern = new YearlyPattern()
            {
                Interval = 2,
                NumberOfOccurrences = 10,
                StartDate = new DateTime(2016, 9, 8),
                DayOfMonth = 23,
                Month = Month.April
            };

            // act
            var result = pattern.IsBusyInDate(today);

            // assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsBusyInDate_IntervalMonthDayOfMonthAndNumberOfOccurancesCaseWithOverflownInstances_ShouldReturnFalse()
        {
            // arrange 
            var today = new DateTime(2022, 4, 23);

            var pattern = new YearlyPattern()
            {
                Interval = 2,
                NumberOfOccurrences = 2,
                StartDate = new DateTime(2016, 9, 8),
                DayOfMonth = 23,
                Month = Month.April
            };

            // act
            var result = pattern.IsBusyInDate(today);

            // assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsBusyInDate_IntervalMonthDayOfMonthAndEndDateCaseWithBusyDate_ShouldReturnTrue()
        {
            // arrange 
            var today = new DateTime(2016, 4, 23);

            var pattern = new YearlyPattern()
            {
                Interval = 2,
                EndDate = new DateTime(2016, 9, 24),
                StartDate = new DateTime(2016, 3, 8),
                DayOfMonth = 23,
                Month = Month.April
            };

            // act
            var result = pattern.IsBusyInDate(today);

            // assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsBusyInDate_IntervalMonthDayOfMonthAndEndDateCaseWithNotBusyDate_ShouldReturnFalse()
        {
            // arrange 
            var today = new DateTime(2016, 4, 23);

            var pattern = new YearlyPattern()
            {
                Interval = 2,
                EndDate = new DateTime(2016, 4, 22),
                StartDate = new DateTime(2016, 9, 8),
                DayOfMonth = 23,
                Month = Month.April
            };

            // act
            var result = pattern.IsBusyInDate(today);

            // assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ToString_ShouldReturnRightText()
        {
            // arrange 
            var expectedResult = "Каждый 2 год, 23 апреля.";

            var pattern = new YearlyPattern()
            {
                Interval = 2,
                DayOfMonth = 23,
                Month = Month.April
            };

            // act
            var result = pattern.ToString();

            // assert
            Assert.AreEqual(expectedResult, result);
        }
    }
}
