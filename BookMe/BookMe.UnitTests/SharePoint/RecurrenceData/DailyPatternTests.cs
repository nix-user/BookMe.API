using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookMe.Core;
using BookMe.Core.Enums;
using BookMe.Core.Models.Recurrence;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BookMe.UnitTests.SharePoint.RecurrenceData
{
    [TestClass]
    public class DailyPatternTests
    {
        [TestMethod]
        public void IsBusyInDate_IntervalAndNumberOfOccurancesCaseWithBusyDate_ShouldReturnTrue()
        {
            // arrange 
            var today = new DateTime(2016, 9, 11);

            var pattern = new DailyPattern()
            {
                Interval = 3,
                NumberOfOccurrences = 5,
                StartDate = new DateTime(2016, 9, 8)
            };

            // act
            var result = pattern.IsBusyInDate(today);

            // assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsBusyInDate_IntervalAndNumberOfOccurancesCaseWithNotBusyDate_ShouldReturnFalse()
        {
            // arrange 
            var today = new DateTime(2016, 11, 9);

            var pattern = new DailyPattern()
            {
                Interval = 3,
                NumberOfOccurrences = 5,
                StartDate = new DateTime(2016, 9, 11)
            };

            // act
            var result = pattern.IsBusyInDate(today);

            // assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsBusyInDate_IntervalAndEndDateCaseWithBusyDate_ShouldReturnTrue()
        {
            // arrange 
            var today = new DateTime(2016, 9, 14);

            var pattern = new DailyPattern()
            {
                Interval = 3,
                StartDate = new DateTime(2016, 9, 8),
                EndDate = new DateTime(2016, 9, 15),
            };

            // act
            var result = pattern.IsBusyInDate(today);

            // assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsBusyInDate_IntervalAndEndDateCaseWithNotBusyDate_ShouldReturFalse()
        {
            // arrange 
            var today = new DateTime(2016, 9, 17);

            var pattern = new DailyPattern()
            {
                Interval = 3,
                StartDate = new DateTime(2016, 9, 8),
                EndDate = new DateTime(2016, 9, 15),
            };

            // act
            var result = pattern.IsBusyInDate(today);

            // assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsBusyInDate_DaysOfWeekAndNumberOfOccurancesCaseWithBusyDate_ShouldReturnTrue()
        {
            // arrange 
            var today = new DateTime(2016, 9, 14);

            var pattern = new DailyPattern()
            {
                DaysOfTheWeek = new List<DayOfTheWeek>() { DayOfTheWeek.Weekday },
                NumberOfOccurrences = 5,
                StartDate = new DateTime(2016, 9, 8)
            };

            // act
            var result = pattern.IsBusyInDate(today);

            // assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsBusyInDate_DaysOfWeeklAndNumberOfOccurancesCaseWithNotBusyDate_ShouldReturnFalse()
        {
            // arrange 
            var today = new DateTime(2016, 9, 14);

            var pattern = new DailyPattern()
            {
                DaysOfTheWeek = new List<DayOfTheWeek>() { DayOfTheWeek.Weekday },
                NumberOfOccurrences = 3,
                StartDate = new DateTime(2016, 9, 8)
            };

            // act
            var result = pattern.IsBusyInDate(today);

            // assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsBusyInDate_DaysOfWeekAndEndDateCaseWithBusyDate_ShouldReturnTrue()
        {
            // arrange 
            var today = new DateTime(2016, 9, 14);

            var pattern = new DailyPattern()
            {
                DaysOfTheWeek = new List<DayOfTheWeek>() { DayOfTheWeek.Weekday },
                StartDate = new DateTime(2016, 9, 8),
                EndDate = new DateTime(2016, 9, 15)
            };

            // act
            var result = pattern.IsBusyInDate(today);

            // assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsBusyInDate_DaysOfWeekAndEndDateCaseWithNotBusyDate_ShouldReturFalse()
        {
            // arrange 
            var today = new DateTime(2016, 9, 14);

            var pattern = new DailyPattern()
            {
                DaysOfTheWeek = new List<DayOfTheWeek>() { DayOfTheWeek.Weekday },
                StartDate = new DateTime(2016, 9, 8),
                EndDate = new DateTime(2016, 9, 13)
            };

            // act
            var result = pattern.IsBusyInDate(today);

            // assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ToString_RuleWithInterval_ShouldReturRightText()
        {
            // arrange 
            var expectedResult = "Каждый 3 день.";

            var pattern = new DailyPattern()
            {
                Interval = 3
            };

            // act
            var result = pattern.ToString();

            // assert
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void ToString_RuleWithDaysOfTheWeek_ShouldReturRightText()
        {
            // arrange 
            var expectedResult = "Каждый ПН, ПТ.";

            var pattern = new DailyPattern()
            {
                DaysOfTheWeek = new List<DayOfTheWeek>() { DayOfTheWeek.Monday, DayOfTheWeek.Friday }
            };

            // act
            var result = pattern.ToString();

            // assert
            Assert.AreEqual(expectedResult, result);
        }
    }
}
