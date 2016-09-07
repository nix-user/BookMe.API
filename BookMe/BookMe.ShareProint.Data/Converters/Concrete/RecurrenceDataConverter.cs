using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using BookMe.Core.Enums;
using BookMe.Core.Models;
using BookMe.Core.Models.Recurrence;
using BookMe.ShareProint.Data.Converters.Abstract;

namespace BookMe.ShareProint.Data.Converters.Concrete
{
    public class RecurrenceDataConverter : IConverter<Reservation, RecurrenceData>
    {
        private static readonly IDictionary<string, DayOfTheWeek> DayOfTheWeekByAbbreviation = new Dictionary<string, DayOfTheWeek>()
        {
            { "mo", DayOfTheWeek.Monday },
            { "tu", DayOfTheWeek.Tuesday },
            { "we", DayOfTheWeek.Wednesday },
            { "th", DayOfTheWeek.Thursday },
            { "fr", DayOfTheWeek.Friday },
            { "sa", DayOfTheWeek.Saturday },
            { "su", DayOfTheWeek.Sunday },
            { "day", DayOfTheWeek.Day },
            { "weekday", DayOfTheWeek.Weekday },
            { "weekend_day", DayOfTheWeek.WeekendDay },
        };

        private static readonly IDictionary<string, DayOfTheWeekIndex> DayOfTheWeekIndexByAbbreviation = new Dictionary<string, DayOfTheWeekIndex>()
        {
            { "first", DayOfTheWeekIndex.First },
            { "second", DayOfTheWeekIndex.Second },
            { "third", DayOfTheWeekIndex.Third },
            { "fourth", DayOfTheWeekIndex.Fourth },
            { "last", DayOfTheWeekIndex.Last },
        };

        private static readonly IDictionary<string, Func<Reservation, XElement, RecurrenceData>>
            InstatiationMethodByRecurenceDataName = new Dictionary<string, Func<Reservation, XElement, RecurrenceData>>()
            {
                { "daily", InstanceDailyPattern },
                { "weekly", InstanceWeeklyPattern },
                { "monthly", InstanceMonthlyPattern },
                { "monthlyByDay", InstanceRelativeMonthlyPattern },
                { "yearly", InstanceYearlyPattern },
                { "yearlyByDay", InstanceRelativeYearlyPattern },
            };

        public RecurrenceData Convert(Reservation value)
        {
            const string RecurrenceKey = "recurrence";
            const string RuleKey = "rule";

            var xdoc = XDocument.Parse(value.Description);

            var recurrenceElement = xdoc.Element(RecurrenceKey);
            var ruleElement = recurrenceElement.Element(RuleKey);

            var repeatElement = GetRepeatElement(ruleElement);
            var recurrenceDataTypeName = repeatElement.Name.ToString();

            return InstatiationMethodByRecurenceDataName[recurrenceDataTypeName](value, ruleElement);
        }

        public IEnumerable<RecurrenceData> Convert(IEnumerable<Reservation> values)
        {
            throw new NotImplementedException();
        }

        #region RecurrenceData instantiation methods

        private static RecurrenceData InstanceDailyPattern(Reservation reservation, XElement ruleElement)
        {
            const string FrequencyKey = "dayFrequency";

            var repeatElement = GetRepeatElement(ruleElement);
            var recurrenceData = new DailyPattern()
            {
                EndDate = GetEndDate(ruleElement),
                Interval = GetFrequency(repeatElement, FrequencyKey),
                StartDate = reservation.EventDate,
                NumberOfOccurrences = GetRepeatInstances(ruleElement)
            };

            return recurrenceData;
        }

        private static RecurrenceData InstanceWeeklyPattern(Reservation reservation, XElement ruleElement)
        {
            const string FrequencyKey = "weekFrequency";

            var repeatElement = GetRepeatElement(ruleElement);
            var recurrenceData = new WeeklyPattern()
            {
                Interval = GetFrequency(repeatElement, FrequencyKey),
                EndDate = GetEndDate(ruleElement),
                DaysOfTheWeek = GetDaysOfTheWeek(repeatElement),
                NumberOfOccurrences = GetRepeatInstances(ruleElement),
                StartDate = reservation.EventDate
            };

            return recurrenceData;
        }

        private static RecurrenceData InstanceMonthlyPattern(Reservation reservation, XElement ruleElement)
        {
            const string FrequencyKey = "monthFrequency";

            var repeatElement = GetRepeatElement(ruleElement);
            var recurrenceData = new MonthlyPattern()
            {
                Interval = GetFrequency(repeatElement, FrequencyKey),
                EndDate = GetEndDate(ruleElement),
                NumberOfOccurrences = GetRepeatInstances(ruleElement),
                StartDate = reservation.EventDate,
                DayOfMonth = GetDayOfMonth(repeatElement)
            };

            return recurrenceData;
        }

        private static RecurrenceData InstanceRelativeMonthlyPattern(Reservation reservation, XElement ruleElement)
        {
            const string FrequencyKey = "monthFrequency";

            var repeatElement = GetRepeatElement(ruleElement);
            var recurrenceData = new RelativeMonthlyPattern()
            {
                Interval = GetFrequency(repeatElement, FrequencyKey),
                EndDate = GetEndDate(ruleElement),
                NumberOfOccurrences = GetRepeatInstances(ruleElement),
                StartDate = reservation.EventDate,
                DaysOfTheWeek = GetDaysOfTheWeek(repeatElement),
                DayOfTheWeekIndex = GetDaysOfTheWeekIndex(repeatElement)
            };

            return recurrenceData;
        }

        private static RecurrenceData InstanceYearlyPattern(Reservation reservation, XElement ruleElement)
        {
            const string FrequencyKey = "yearFrequency";

            var repeatElement = GetRepeatElement(ruleElement);
            var recurrenceData = new YearlyPattern()
            {
                Interval = GetFrequency(repeatElement, FrequencyKey),
                EndDate = GetEndDate(ruleElement),
                NumberOfOccurrences = GetRepeatInstances(ruleElement),
                StartDate = reservation.EventDate,
                DayOfMonth = GetDayOfMonth(repeatElement),
                Month = GetMonth(repeatElement)
            };

            return recurrenceData;
        }

        private static RecurrenceData InstanceRelativeYearlyPattern(Reservation reservation, XElement ruleElement)
        {
            const string FrequencyKey = "yearFrequency";

            var repeatElement = GetRepeatElement(ruleElement);
            var recurrenceData = new RelativeYearlyPattern()
            {
                Interval = GetFrequency(repeatElement, FrequencyKey),
                EndDate = GetEndDate(ruleElement),
                NumberOfOccurrences = GetRepeatInstances(ruleElement),
                StartDate = reservation.EventDate,
                Month = GetMonth(repeatElement),
                DayOfTheWeekIndex = GetDaysOfTheWeekIndex(repeatElement),
                DaysOfTheWeek = GetDaysOfTheWeek(repeatElement)
            };

            return recurrenceData;
        }

        #endregion

        #region XML parsing helpers

        private static DateTime? GetEndDate(XElement element)
        {
            const string EndDateXmlKey = "windowEnd";

            var windowEndElement = element.Element(EndDateXmlKey);
            DateTime? endDate = null;
            if (windowEndElement != null)
            {
                endDate = DateTime.Parse(windowEndElement.Value);
            }

            return endDate?.ToUniversalTime();
        }

        private static int? GetRepeatInstances(XElement element)
        {
            const string RepeatInstancesXmlKey = "repeatInstances";

            var repeatInstancesElement = element.Element(RepeatInstancesXmlKey);
            int? repeatInstances = null;
            if (repeatInstancesElement != null)
            {
                repeatInstances = int.Parse(repeatInstancesElement.Value);
            }

            return repeatInstances;
        }

        private static IEnumerable<DayOfTheWeek> GetDaysOfTheWeek(XElement element)
        {
            var daysOfTheWeek = new List<DayOfTheWeek>();
            foreach (var item in DayOfTheWeekByAbbreviation)
            {
                var dayOfTheWeekAttribute = element.Attribute(item.Key);
                if (dayOfTheWeekAttribute != null)
                {
                    daysOfTheWeek.Add(item.Value);
                }
            }

            return daysOfTheWeek;
        }

        private static DayOfTheWeekIndex GetDaysOfTheWeekIndex(XElement element)
        {
            const string WeekDayOfMonthKey = "weekdayOfMonth";

            var weekdayOfMonthAtribute = element.Attribute(WeekDayOfMonthKey);
            return DayOfTheWeekIndexByAbbreviation[weekdayOfMonthAtribute.Value];
        }

        private static XElement GetRepeatElement(XElement ruleElement)
        {
            const string RepeatKey = "repeat";

            return ruleElement
                .Element(RepeatKey)
                .Elements()
                .FirstOrDefault();
        }

        private static int GetFrequency(XElement repeatElement, string frequencyKey)
        {
            return int.Parse(repeatElement.Attribute(frequencyKey).Value);
        }

        private static int GetDayOfMonth(XElement repeatElement)
        {
            const string DayOfMonthKey = "day";

            return int.Parse(repeatElement.Attribute(DayOfMonthKey).Value);
        }

        private static Month GetMonth(XElement repeatElement)
        {
            const string MonthKey = "month";

            return (Month)int.Parse(repeatElement.Attribute(MonthKey).Value);
        }

        #endregion
    }
}
