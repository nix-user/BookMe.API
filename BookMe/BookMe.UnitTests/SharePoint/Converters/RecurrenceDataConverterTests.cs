﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookMe.Core.Enums;
using BookMe.Core.Models;
using BookMe.Core.Models.Recurrence;
using BookMe.ShareProint.Data.Converters.Concrete;
using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BookMe.UnitTests.SharePoint.Converters
{
    [TestClass]
    public class RecurrenceDataConverterTests
    {
        private const string RecurrenceDataKey = "RecurrenceData";
        private const string EventDateKey = "EventDate";

        [TestMethod]
        public void Convert_DailyPatternXML_DailyPattern()
        {
            // arrange
            const string xml = @"<recurrence>
                          <rule>
                            <firstDayOfWeek>su</firstDayOfWeek>
                            <repeat><daily dayFrequency='1' /></repeat>
                            <repeatForever>FALSE</repeatForever>
                          </rule>
                        </recurrence>";

            var expectedStartDate = DateTime.Now;

            var fildValuesDictionary = new Dictionary<string, object>()
            {
                { RecurrenceDataKey, xml },
                { EventDateKey, expectedStartDate },
            };

            var expectedRecurrenceData = new DailyPattern()
            {
                StartDate = expectedStartDate,
                Interval = 1,
                EndDate = null,
                NumberOfOccurrences = null
            };

            var converter = new RecurrenceDataConverter();

            // act
            var result = converter.Convert(fildValuesDictionary);

            // assert
            var dailyResult = result as DailyPattern;

            Assert.AreEqual(expectedRecurrenceData.StartDate, dailyResult.StartDate);
            Assert.AreEqual(expectedRecurrenceData.Interval, dailyResult.Interval);
            Assert.AreEqual(expectedRecurrenceData.EndDate, dailyResult.EndDate);
            Assert.AreEqual(expectedRecurrenceData.NumberOfOccurrences, dailyResult.NumberOfOccurrences);
        }

        [TestMethod]
        public void Convert_WeeklyPatternXML_WeeklyPattern()
        {
            // arrange
            const string xml = @"<recurrence>
                                   <rule>
                                     <firstDayOfWeek>su</firstDayOfWeek>
                                     <repeat><weekly mo='TRUE' tu='TRUE' we='TRUE' weekFrequency='1' /></repeat>
                                     <windowEnd>2007-05-31T22:00:00Z</windowEnd>
                                   </rule>
                                 </recurrence>";

            var expectedStartDate = DateTime.Now;

            var fildValuesDictionary = new Dictionary<string, object>()
            {
                { RecurrenceDataKey, xml },
                { EventDateKey, expectedStartDate },
            };

            var expectedRecurrenceData = new WeeklyPattern()
            {
                StartDate = expectedStartDate,
                Interval = 1,
                EndDate = new DateTime(2007, 5, 31, 22, 0, 0),
                NumberOfOccurrences = null,
                DaysOfTheWeek = new List<DayOfTheWeek>() { DayOfTheWeek.Monday, DayOfTheWeek.Tuesday, DayOfTheWeek.Wednesday }
            };

            var converter = new RecurrenceDataConverter();

            // act
            var result = converter.Convert(fildValuesDictionary);

            // assert
            var weeklyResult = result as WeeklyPattern;

            Assert.AreEqual(expectedRecurrenceData.StartDate, weeklyResult.StartDate);
            Assert.AreEqual(expectedRecurrenceData.Interval, weeklyResult.Interval);
            Assert.AreEqual(expectedRecurrenceData.EndDate, weeklyResult.EndDate);
            Assert.AreEqual(expectedRecurrenceData.NumberOfOccurrences, weeklyResult.NumberOfOccurrences);

            for (var i = 0; i < expectedRecurrenceData.DaysOfTheWeek.Count(); i++)
            {
                Assert.AreEqual(expectedRecurrenceData.DaysOfTheWeek.ToList()[i], weeklyResult.DaysOfTheWeek.ToList()[i]);
            }
        }

        [TestMethod]
        public void Convert_MonthlyPatternXML_MonthlyPattern()
        {
            // arrange
            const string xml = @"<recurrence>
                                   <rule>
                                     <firstDayOfWeek>su</firstDayOfWeek>
                                     <repeat><monthly day='15' monthFrequency='4' /></repeat>
                                     <repeatInstances>5</repeatInstances>
                                   </rule>
                                 </recurrence>";

            var expectedStartDate = DateTime.Now;

            var fildValuesDictionary = new Dictionary<string, object>()
            {
                { RecurrenceDataKey, xml },
                { EventDateKey, expectedStartDate },
            };

            var expectedRecurrenceData = new MonthlyPattern()
            {
                StartDate = expectedStartDate,
                Interval = 4,
                EndDate = null,
                NumberOfOccurrences = 5,
            };

            var converter = new RecurrenceDataConverter();

            // act
            var result = converter.Convert(fildValuesDictionary);

            // assert
            var monthlyResult = result as MonthlyPattern;

            Assert.AreEqual(expectedRecurrenceData.StartDate, monthlyResult.StartDate);
            Assert.AreEqual(expectedRecurrenceData.Interval, monthlyResult.Interval);
            Assert.AreEqual(expectedRecurrenceData.EndDate, monthlyResult.EndDate);
            Assert.AreEqual(expectedRecurrenceData.NumberOfOccurrences, monthlyResult.NumberOfOccurrences);
        }

        [TestMethod]
        public void Convert_MonthlyByDayPatternXML_MonthlyByDayPattern()
        {
            // arrange
            const string xml = @"<recurrence>
                                   <rule>
                                     <firstDayOfWeek>su</firstDayOfWeek>
                                     <repeat><monthlyByDay weekend_day='TRUE' weekdayOfMonth='last' monthFrequency='2' /></repeat>
                                     <repeatInstances>5</repeatInstances>
                                   </rule>
                                 </recurrence>";

            var expectedStartDate = DateTime.Now;

            var fildValuesDictionary = new Dictionary<string, object>()
            {
                { RecurrenceDataKey, xml },
                { EventDateKey, expectedStartDate },
            };

            var expectedRecurrenceData = new RelativeMonthlyPattern()
            {
                StartDate = expectedStartDate,
                Interval = 2,
                EndDate = null,
                NumberOfOccurrences = 5,
                DayOfTheWeekIndex = DayOfTheWeekIndex.Last,
                DaysOfTheWeek = new List<DayOfTheWeek>() { DayOfTheWeek.WeekendDay }
            };

            var converter = new RecurrenceDataConverter();

            // act
            var result = converter.Convert(fildValuesDictionary);

            // assert
            var monthlyResult = result as RelativeMonthlyPattern;

            Assert.AreEqual(expectedRecurrenceData.StartDate, monthlyResult.StartDate);
            Assert.AreEqual(expectedRecurrenceData.Interval, monthlyResult.Interval);
            Assert.AreEqual(expectedRecurrenceData.EndDate, monthlyResult.EndDate);
            Assert.AreEqual(expectedRecurrenceData.NumberOfOccurrences, monthlyResult.NumberOfOccurrences);
            Assert.AreEqual(expectedRecurrenceData.DayOfTheWeekIndex, monthlyResult.DayOfTheWeekIndex);

            for (var i = 0; i < expectedRecurrenceData.DaysOfTheWeek.Count(); i++)
            {
                Assert.AreEqual(expectedRecurrenceData.DaysOfTheWeek.ToList()[i], monthlyResult.DaysOfTheWeek.ToList()[i]);
            }
        }

        [TestMethod]
        public void Convert_YearlyPatternXML_MonthlyByDayPattern()
        {
            // arrange
            const string xml = @"<recurrence>
                                   <rule>
                                     <firstDayOfWeek>su</firstDayOfWeek>
                                     <repeat><yearly yearFrequency='2' month='11' day='9' /></repeat>
                                     <repeatInstances>5</repeatInstances>
                                   </rule>
                                 </recurrence>";

            var expectedStartDate = DateTime.Now;

            var fildValuesDictionary = new Dictionary<string, object>()
            {
                { RecurrenceDataKey, xml },
                { EventDateKey, expectedStartDate },
            };

            var expectedRecurrenceData = new YearlyPattern()
            {
                StartDate = expectedStartDate,
                Interval = 2,
                EndDate = null,
                NumberOfOccurrences = 5,
                Month = Month.November,
                DayOfMonth = 9
            };

            var converter = new RecurrenceDataConverter();

            // act
            var result = converter.Convert(fildValuesDictionary);

            // assert
            var yearlyResult = result as YearlyPattern;

            Assert.AreEqual(expectedRecurrenceData.StartDate, yearlyResult.StartDate);
            Assert.AreEqual(expectedRecurrenceData.Interval, yearlyResult.Interval);
            Assert.AreEqual(expectedRecurrenceData.EndDate, yearlyResult.EndDate);
            Assert.AreEqual(expectedRecurrenceData.NumberOfOccurrences, yearlyResult.NumberOfOccurrences);
            Assert.AreEqual(expectedRecurrenceData.Month, yearlyResult.Month);
            Assert.AreEqual(expectedRecurrenceData.DayOfMonth, yearlyResult.DayOfMonth);
        }

        [TestMethod]
        public void Convert_YearlyByDayPatternXML_YearlyByDay()
        {
            // arrange
            const string xml = @"<recurrence>
                                   <rule>
                                     <firstDayOfWeek>su</firstDayOfWeek>
                                     <repeat><yearlyByDay yearFrequency='2' mo='TRUE' fr='TRUE' weekdayOfMonth='second' month='4' /></repeat>
                                     <repeatInstances>5</repeatInstances>
                                   </rule>
                                 </recurrence>";

            var expectedStartDate = DateTime.Now;

            var fildValuesDictionary = new Dictionary<string, object>()
            {
                { RecurrenceDataKey, xml },
                { EventDateKey, expectedStartDate },
            };

            var expectedRecurrenceData = new RelativeYearlyPattern()
            {
                StartDate = expectedStartDate,
                Interval = 2,
                EndDate = null,
                NumberOfOccurrences = 5,
                Month = Month.April,
                DaysOfTheWeek = new List<DayOfTheWeek>() { DayOfTheWeek.Monday, DayOfTheWeek.Friday },
                DayOfTheWeekIndex = DayOfTheWeekIndex.Second
            };

            var converter = new RecurrenceDataConverter();

            // act
            var result = converter.Convert(fildValuesDictionary);

            // assert
            var yearlyResult = result as RelativeYearlyPattern;

            Assert.AreEqual(expectedRecurrenceData.StartDate, yearlyResult.StartDate);
            Assert.AreEqual(expectedRecurrenceData.Interval, yearlyResult.Interval);
            Assert.AreEqual(expectedRecurrenceData.EndDate, yearlyResult.EndDate);
            Assert.AreEqual(expectedRecurrenceData.NumberOfOccurrences, yearlyResult.NumberOfOccurrences);
            Assert.AreEqual(expectedRecurrenceData.Month, yearlyResult.Month);
            Assert.AreEqual(expectedRecurrenceData.DayOfTheWeekIndex, yearlyResult.DayOfTheWeekIndex);

            for (var i = 0; i < expectedRecurrenceData.DaysOfTheWeek.Count(); i++)
            {
                Assert.AreEqual(expectedRecurrenceData.DaysOfTheWeek.ToList()[i], yearlyResult.DaysOfTheWeek.ToList()[i]);
            }
        }
    }
}
