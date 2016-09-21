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
    public class MonthlyPatternTests
    {
        [TestMethod]
        public void IsBusyInDate_IntervalDayOfMonthAndNumberOfOccurancesCaseWithBusyDate_ShouldReturnTrue()
        {
            // arrange 
            var today = new DateTime(2016, 9, 23);

            var pattern = new MonthlyPattern()
            {
                Interval = 2,
                NumberOfOccurrences = 10,
                StartDate = new DateTime(2016, 9, 8),
                DayOfMonth = 23
            };

            // act
            var result = pattern.IsBusyInDate(today);

            // assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsBusyInDate_IntervalDayOfMonthAndNumberOfOccurancesCaseWithNotBusyDate_ShouldReturnFalse()
        {
            // arrange 
            var today = new DateTime(2016, 9, 23);

            var pattern = new MonthlyPattern()
            {
                Interval = 2,
                NumberOfOccurrences = 10,
                StartDate = new DateTime(2016, 9, 8),
                DayOfMonth = 25
            };

            // act
            var result = pattern.IsBusyInDate(today);

            // assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsBusyInDate_IntervalDayOfMonthAndNumberOfOccurancesCaseWithOverflownInstances_ShouldReturnFalse()
        {
            // arrange 
            var today = new DateTime(2017, 3, 23);

            var pattern = new MonthlyPattern()
            {
                Interval = 2,
                NumberOfOccurrences = 1,
                StartDate = new DateTime(2016, 9, 8),
                DayOfMonth = 23
            };

            // act
            var result = pattern.IsBusyInDate(today);

            // assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsBusyInDate_IntervalDayOfMonthAndEndDateCaseWithBusyDate_ShouldReturnTrue()
        {
            // arrange 
            var today = new DateTime(2016, 9, 23);

            var pattern = new MonthlyPattern()
            {
                Interval = 1,
                EndDate = new DateTime(2016, 9, 24),
                StartDate = new DateTime(2016, 9, 8),
                DayOfMonth = 23
            };

            // act
            var result = pattern.IsBusyInDate(today);

            // assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsBusyInDate_IntervalDayOfMonthAndEndDateCaseWithNotBusyDate_ShouldReturnFalse()
        {
            // arrange 
            var today = new DateTime(2016, 9, 23);

            var pattern = new MonthlyPattern()
            {
                Interval = 1,
                EndDate = new DateTime(2016, 9, 22),
                StartDate = new DateTime(2016, 9, 8),
                DayOfMonth = 23
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
            var expectedResult = "Каждый 3 месяц 23 числа.";

            var pattern = new MonthlyPattern()
            {
                Interval = 3,
                DayOfMonth = 23
            };

            // act
            var result = pattern.ToString();

            // assert
            Assert.AreEqual(expectedResult, result);
        }
    }
}
