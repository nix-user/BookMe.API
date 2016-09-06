using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookMe.Core.Models;
using BookMe.Core.Models.Recurrence;
using BookMe.ShareProint.Data.Converters.Concrete;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BookMe.UnitTests.SharePoint.Converters
{
    [TestClass]
    public class RecurrenceDataConverterTests
    {
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

            var reservation = new Reservation()
            {
                Description = xml,
                EventDate = expectedStartDate
            };

            var expectedRecurrenceDate = new DailyPattern()
            {
                StartDate = expectedStartDate,
                Interval = 1,
                EndDate = null,
                NumberOfOccurrences = null
            };

            var converter = new RecurrenceDataConverter();

            // act
            var result = converter.Convert(reservation);

            // assert
            var dailyResult = result as DailyPattern;

            Assert.AreEqual(expectedRecurrenceDate.StartDate, dailyResult.StartDate);
            Assert.AreEqual(expectedRecurrenceDate.Interval, dailyResult.Interval);
            Assert.AreEqual(expectedRecurrenceDate.EndDate, dailyResult.EndDate);
            Assert.AreEqual(expectedRecurrenceDate.NumberOfOccurrences, dailyResult.NumberOfOccurrences);
        }
    }
}
