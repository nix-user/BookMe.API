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
    public class WeeklyPatternTests
    {
        [TestMethod]
        public void IsBusyInDate_IntervalDaysOfWeekAndNumberOfOccurancesCaseWithBusyDate_ShouldReturnTrue()
        {
            // arrange 
            var today = new DateTime(2016, 9, 23);

            var pattern = new WeeklyPattern()
            {
                Interval = 2,
                NumberOfOccurrences = 10,
                StartDate = new DateTime(2016, 9, 8),
                DaysOfTheWeek = new List<DayOfTheWeek>() { DayOfTheWeek.Weekday }
            };

            // act
            var result = pattern.IsBusyInDate(today);

            // assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsBusyInDate_IntervalDaysOfWeekAndNumberOfOccurancesCaseWithNotBusyDate_ShouldReturnFalse()
        {
            // arrange 
            var today = new DateTime(2016, 9, 23);

            var pattern = new WeeklyPattern()
            {
                Interval = 2,
                NumberOfOccurrences = 5,
                StartDate = new DateTime(2016, 9, 8),
                DaysOfTheWeek = new List<DayOfTheWeek>() { DayOfTheWeek.Weekday }
            };

            // act
            var result = pattern.IsBusyInDate(today);

            // assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsBusyInDate_IntervalEndDateAndEndDateCaseWithBusyDate_ShouldReturnTrue()
        {
            // arrange 
            var today = new DateTime(2016, 9, 23);

            var pattern = new WeeklyPattern()
            {
                Interval = 2,
                StartDate = new DateTime(2016, 9, 8),
                EndDate = new DateTime(2016, 9, 24),
                DaysOfTheWeek = new List<DayOfTheWeek>() { DayOfTheWeek.Wednesday, DayOfTheWeek.Friday }
            };

            // act
            var result = pattern.IsBusyInDate(today);

            // assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsBusyInDate_IntervalEndDateAndEndDateCaseWithNotBusyDate_ShouldReturFalse()
        {
            // arrange 
            var today = new DateTime(2016, 9, 23);

            var pattern = new WeeklyPattern()
            {
                Interval = 2,
                StartDate = new DateTime(2016, 9, 8),
                EndDate = new DateTime(2016, 9, 22),
                DaysOfTheWeek = new List<DayOfTheWeek>() { DayOfTheWeek.Weekday }
            };

            // act
            var result = pattern.IsBusyInDate(today);

            // assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsBusyInDate_UnsuitableDate_ShouldReturnFalse()
        {
            // arrange 
            var today = new DateTime(2016, 9, 23);

            var pattern = new WeeklyPattern()
            {
                Interval = 2,
                StartDate = new DateTime(2016, 9, 8),
                EndDate = new DateTime(2016, 9, 24),
                DaysOfTheWeek = new List<DayOfTheWeek>() { DayOfTheWeek.Wednesday }
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
            var expectedResult = "Каждую 2 неделю, каждый день.";

            var pattern = new WeeklyPattern()
            {
                Interval = 2,
                DaysOfTheWeek = new List<DayOfTheWeek>() { DayOfTheWeek.Day }
            };

            // act
            var result = pattern.ToString();

            // assert
            Assert.AreEqual(expectedResult, result);
        }
    }
}
