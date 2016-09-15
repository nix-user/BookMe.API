using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookMe.Core.Enums;
using BookMe.Core.Models;
using BookMe.ShareProint.Data.Converters.Abstract;
using BookMe.ShareProint.Data.Converters.Concrete;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RecurrenceDataType = BookMe.Core.Models.Recurrence.RecurrenceData;

namespace BookMe.UnitTests.SharePoint.Converters
{
    [TestClass]
    public class ReservationConverterTests
    {
        private const string IdKey = "ID";
        private const string TitleKey = "Title";
        private const string DescriptionKey = "Description";
        private const string FacilitiesKey = "Facilities";
        private const string EventDateKey = "EventDate";
        private const string EndDateKey = "EndDate";
        private const string DurationKey = "Duration";
        private const string IsRecurrenceKey = "fRecurrence";
        private const string AuthorKey = "Author";
        private const string ParrentIdKey = "MasterSeriesItemID";
        private const string EventTypeKey = "EventType";
        private const string IsAllDayEventKey = "fAllDayEvent";
        private const string RecurrenceDateKey = "RecurrenceID";

        private IConverter<IDictionary<string, object>, RecurrenceDataType> ArrangeRecurrenceDataConverter()
        {
            var recurrenceDataConverterMock = new Mock<IConverter<IDictionary<string, object>, RecurrenceDataType>>();
            recurrenceDataConverterMock
                .Setup(x => x.Convert(It.IsAny<IDictionary<string, object>>()))
                .Returns((RecurrenceDataType)null);

            return recurrenceDataConverterMock.Object;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Convert_ArgumentNull_ShouldThrowArgumentNullException()
        {
            // arrange
            var recurrenceDataConverter = this.ArrangeRecurrenceDataConverter();
            var reservationConverter = new ReservationConverter(recurrenceDataConverter);

            // act
            reservationConverter.Convert((IDictionary<string, object>)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Convert_IEnumerableArgumentNull_ShouldThrowArgumentNullException()
        {
            // arrange
            var recurrenceDataConverter = this.ArrangeRecurrenceDataConverter();
            var reservationConverter = new ReservationConverter(recurrenceDataConverter);

            // act
            reservationConverter.Convert((IEnumerable<IDictionary<string, object>>)null);
        }

        [TestMethod]
        public void Convert_DictionaryWithNeededProperties_PropertiesMappedRightOnModel()
        {
            // arrange
            var recurrenceDataConverter = this.ArrangeRecurrenceDataConverter();
            var reservationConverter = new ReservationConverter(recurrenceDataConverter);

            var expectedReservation = new Reservation()
            {
                Id = 1,
                EndDate = new DateTime(2016, 1, 1),
                RecurrenceData = null,
                IsRecurrence = false,
                EventDate = new DateTime(2016, 1, 1),
                Description = "description",
                ParentId = null,
                EventType = (EventType)1,
                IsAllDayEvent = false,
                Title = "title",
                Duration = new TimeSpan(0, 1, 0),
                OwnerName = "Jon Snow",
                ResourceId = 42,
                RecurrenceDate = null
            };

            var value = new Dictionary<string, object>()
            {
                {IdKey, expectedReservation.Id},
                {TitleKey, expectedReservation.Title},
                {DescriptionKey, expectedReservation.Description},
                {FacilitiesKey, expectedReservation.ResourceId},
                {EventDateKey, expectedReservation.EventDate},
                {EndDateKey, expectedReservation.EndDate},
                {DurationKey, expectedReservation.Duration.TotalSeconds},
                {IsRecurrenceKey, expectedReservation.IsRecurrence},
                {AuthorKey, expectedReservation.OwnerName},
                {ParrentIdKey, expectedReservation.ParentId},
                {EventTypeKey, (int)expectedReservation.EventType},
                {IsAllDayEventKey, expectedReservation.IsAllDayEvent},
                { RecurrenceDateKey,expectedReservation.RecurrenceDate}
            };

            // act
            var actualReservation = reservationConverter.Convert(value);

            // assert
            Assert.AreEqual(expectedReservation.Id, actualReservation.Id);
            Assert.AreEqual(expectedReservation.Title, actualReservation.Title);
            Assert.AreEqual(expectedReservation.Description, actualReservation.Description);
            Assert.AreEqual(expectedReservation.EventDate, actualReservation.EventDate);
            Assert.AreEqual(expectedReservation.EndDate, actualReservation.EndDate);
            Assert.AreEqual(expectedReservation.Duration, actualReservation.Duration);
            Assert.AreEqual(expectedReservation.IsRecurrence, actualReservation.IsRecurrence);
            Assert.AreEqual(expectedReservation.ParentId, actualReservation.ParentId);
            Assert.AreEqual(expectedReservation.EventType, actualReservation.EventType);
            Assert.AreEqual(expectedReservation.IsAllDayEvent, actualReservation.IsAllDayEvent);
        }

        [TestMethod]
        public void Convert_DictionaryCollectionWithNeededProperties_PropertiesMappedRightOnModel()
        {
            // arrange
            var recurrenceDataConverter = this.ArrangeRecurrenceDataConverter();
            var reservationConverter = new ReservationConverter(recurrenceDataConverter);

            var expectedReservation1 = new Reservation()
            {
                Id = 1,
                EndDate = new DateTime(2016, 1, 1),
                RecurrenceData = null,
                IsRecurrence = false,
                EventDate = new DateTime(2016, 1, 1),
                Description = "description",
                ParentId = null,
                EventType = EventType.Recurrent,
                IsAllDayEvent = false,
                Title = "title",
                Duration = new TimeSpan(0, 1, 0),
                OwnerName = "Jon Snow",
                ResourceId = 42,
                RecurrenceDate = null
            };

            var expectedReservation2 = new Reservation()
            {
                Id = 2,
                EndDate = new DateTime(2015, 1, 1),
                RecurrenceData = null,
                IsRecurrence = false,
                EventDate = new DateTime(2015, 1, 1),
                Description = "description2",
                ParentId = null,
                EventType = EventType.Recurrent,
                IsAllDayEvent = false,
                Title = "title2",
                Duration = new TimeSpan(0, 2, 0),
                OwnerName = "Tyrion Lannister",
                ResourceId = 99,
                RecurrenceDate = null
            };

            var expectedReservations = new List<Reservation>() { expectedReservation1, expectedReservation2 };
            var value = new List<Dictionary<string, object>>();

            for (var i = 0; i < expectedReservations.Count; i++)
            {
                value.Add(new Dictionary<string, object>()
                {
                    {IdKey, expectedReservations[i].Id},
                    {TitleKey, expectedReservations[i].Title},
                    {DescriptionKey, expectedReservations[i].Description},
                    {FacilitiesKey, expectedReservations[i].ResourceId},
                    {EventDateKey, expectedReservations[i].EventDate},
                    {EndDateKey, expectedReservations[i].EndDate},
                    {DurationKey, expectedReservations[i].Duration.TotalSeconds},
                    {IsRecurrenceKey, expectedReservations[i].IsRecurrence},
                    {AuthorKey, expectedReservations[i].OwnerName},
                    {ParrentIdKey, expectedReservations[i].ParentId},
                    {EventTypeKey, (int)expectedReservations[i].EventType},
                    {IsAllDayEventKey, expectedReservations[i].IsAllDayEvent},
                    { RecurrenceDateKey,expectedReservations[i].RecurrenceDate}
                });
            }

            // act
            var actualReservations = reservationConverter.Convert(value).ToList();

            // assert
            for (var i = 0; i < actualReservations.Count; i++)
            {
                Assert.AreEqual(expectedReservations[i].Id, actualReservations[i].Id);
                Assert.AreEqual(expectedReservations[i].Title, actualReservations[i].Title);
                Assert.AreEqual(expectedReservations[i].Description, actualReservations[i].Description);
                Assert.AreEqual(expectedReservations[i].EventDate, actualReservations[i].EventDate);
                Assert.AreEqual(expectedReservations[i].EndDate, actualReservations[i].EndDate);
                Assert.AreEqual(expectedReservations[i].Duration, actualReservations[i].Duration);
                Assert.AreEqual(expectedReservations[i].IsRecurrence, actualReservations[i].IsRecurrence);
                Assert.AreEqual(expectedReservations[i].ParentId, actualReservations[i].ParentId);
                Assert.AreEqual(expectedReservations[i].EventType, actualReservations[i].EventType);
                Assert.AreEqual(expectedReservations[i].IsAllDayEvent, actualReservations[i].IsAllDayEvent);
            }
        }
    }
}
