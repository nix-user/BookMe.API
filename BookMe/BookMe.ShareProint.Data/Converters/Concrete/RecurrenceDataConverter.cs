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
        private IDictionary<string, DayOfTheWeek> dayOfTheWeekByAbbreviation = new Dictionary<string, DayOfTheWeek>()
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

        public RecurrenceData Convert(Reservation value)
        {
            RecurrenceData recurenceData = null;

            var xdoc = XDocument.Parse(value.Description);

            var recurrenceElement = xdoc.Element("recurrence");
            var ruleElement = recurrenceElement.Element("rule");

            var endDate = this.GetEndDate(ruleElement);
            var numberOfOccurrences = this.GetRepeatInstances(ruleElement);

            var repeatElement = ruleElement.Element("repeat").Elements().FirstOrDefault();
            var recurrenceDataTypeName = repeatElement.Name.ToString();

            int frequency;

            switch (recurrenceDataTypeName)
            {
                case "daily":
                    frequency = int.Parse(repeatElement.Attribute("dayFrequency").Value);
                    recurenceData = new DailyPattern()
                    {
                        EndDate = endDate,
                        Interval = frequency,
                        StartDate = value.EventDate,
                        NumberOfOccurrences = numberOfOccurrences
                    };
                    break;
                case "weekly":
                    frequency = int.Parse(repeatElement.Attribute("weekFrequency").Value);
                    recurenceData = new WeeklyPattern()
                    {
                        Interval = frequency,
                        EndDate = endDate,
                        DaysOfTheWeek = this.GetDaysOfTheWeek(repeatElement),
                        NumberOfOccurrences = numberOfOccurrences,
                        StartDate = value.EventDate
                    };
                    break;
            }

            return recurenceData;
        }

        public IEnumerable<RecurrenceData> Convert(IEnumerable<Reservation> values)
        {
            throw new NotImplementedException();
        }

        private DateTime? GetEndDate(XElement element)
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

        private int? GetRepeatInstances(XElement element)
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

        private IEnumerable<DayOfTheWeek> GetDaysOfTheWeek(XElement element)
        {
            var daysOfTheWeek = new List<DayOfTheWeek>();
            foreach (var item in this.dayOfTheWeekByAbbreviation)
            {
                var dayOfTheWeekAttribute = element.Attribute(item.Key);
                if (dayOfTheWeekAttribute != null)
                {
                    daysOfTheWeek.Add(item.Value);
                }
            }

            return daysOfTheWeek;
        }
    }
}
