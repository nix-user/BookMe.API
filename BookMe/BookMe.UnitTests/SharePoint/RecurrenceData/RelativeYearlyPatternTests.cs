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
    public class RelativeYearlyPatternTests
    {
        [TestMethod]
        public void IsBusyInDate_IntervalDaysOfTheWeekDayOfTheWeekIndexNumberOfOccurancesCaseWithBusyDate_ShouldReturnTrue()
        {
            // arrange 
            var today = new DateTime(2016, 9, 4);

            var pattern = new RelativeYearlyPattern()
            {
                Interval = 2,
                NumberOfOccurrences = 10,
                StartDate = new DateTime(2014, 7, 8),
                DayOfTheWeekIndex = DayOfTheWeekIndex.First,
                DaysOfTheWeek = new List<DayOfTheWeek>() { DayOfTheWeek.Sunday, DayOfTheWeek.Monday },
                Month = Month.September
            };

            // act
            var result = pattern.IsBusyInDate(today);

            // assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsBusyInDate_IntervalDaysOfTheWeekDayOfTheWeekIndexNumberOfOccurancesCaseWithNotBusyDate_ShouldReturnFalse()
        {
            // arrange 
            var today = new DateTime(2016, 9, 1);

            var pattern = new RelativeYearlyPattern()
            {
                Interval = 2,
                NumberOfOccurrences = 10,
                StartDate = new DateTime(2014, 7, 8),
                DayOfTheWeekIndex = DayOfTheWeekIndex.First,
                DaysOfTheWeek = new List<DayOfTheWeek>() { DayOfTheWeek.Sunday, DayOfTheWeek.Monday },
                Month = Month.September
            };

            // act
            var result = pattern.IsBusyInDate(today);

            // assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsBusyInDate_IntervalDaysOfTheWeekDayOfTheWeekIndexNumberOfOccurancesCaseWithOverflownInstances_ShouldReturnFalse()
        {
            // arrange 
            var today = new DateTime(2016, 9, 3);

            var pattern = new RelativeYearlyPattern()
            {
                Interval = 2,
                NumberOfOccurrences = 8,
                StartDate = new DateTime(2014, 9, 2),
                DayOfTheWeekIndex = DayOfTheWeekIndex.First,
                DaysOfTheWeek = new List<DayOfTheWeek>() { DayOfTheWeek.Day },
                Month = Month.September
            };

            // act
            var result = pattern.IsBusyInDate(today);

            // assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsBusyInDate_IntervalDaysOfTheWeekDayOfTheWeekIndexEndDateCaseWithBusyDate_ShouldReturnTrue()
        {
            // arrange 
            var today = new DateTime(2016, 9, 4);

            var pattern = new RelativeYearlyPattern()
            {
                Interval = 2,
                EndDate = new DateTime(2016, 9, 5),
                StartDate = new DateTime(2014, 7, 8),
                DayOfTheWeekIndex = DayOfTheWeekIndex.First,
                DaysOfTheWeek = new List<DayOfTheWeek>() { DayOfTheWeek.Sunday, DayOfTheWeek.Monday },
                Month = Month.September
            };

            // act
            var result = pattern.IsBusyInDate(today);

            // assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsBusyInDate_IntervalDaysOfTheWeekDayOfTheWeekIndexEndDateCaseWithNotBusyDate_ShouldReturnFalse()
        {
            // arrange 
            var today = new DateTime(2016, 9, 4);

            var pattern = new RelativeYearlyPattern()
            {
                Interval = 2,
                EndDate = new DateTime(2016, 9, 3),
                StartDate = new DateTime(2014, 7, 8),
                DayOfTheWeekIndex = DayOfTheWeekIndex.First,
                DaysOfTheWeek = new List<DayOfTheWeek>() { DayOfTheWeek.Sunday, DayOfTheWeek.Monday },
                Month = Month.September
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
            var expectedResult = "Каждый 2 год, каждый ВС, ПН первой недели сентября.";

            var pattern = new RelativeYearlyPattern()
            {
                Interval = 2,
                DayOfTheWeekIndex = DayOfTheWeekIndex.First,
                DaysOfTheWeek = new List<DayOfTheWeek>() { DayOfTheWeek.Sunday, DayOfTheWeek.Monday },
                Month = Month.September
            };

            // act
            var result = pattern.ToString();

            // assert
            Assert.AreEqual(expectedResult, result);
        }
    }
}
