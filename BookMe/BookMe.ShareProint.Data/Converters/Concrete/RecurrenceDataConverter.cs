using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using BookMe.Core.Models;
using BookMe.Core.Models.Recurrence;
using BookMe.ShareProint.Data.Converters.Abstract;

namespace BookMe.ShareProint.Data.Converters.Concrete
{
    public class RecurrenceDataConverter : IConverter<Reservation, RecurrenceData>
    {
        public RecurrenceData Convert(Reservation value)
        {
            RecurrenceData recurenceData = null;

            var xdoc = XDocument.Parse(value.Description);

            var recurrenceElement = xdoc.Element("recurrence");
            var ruleElement = recurrenceElement.Element("rule");

            var windowEndElement = ruleElement.Element("windowEnd");
            DateTime? endDate = null;
            if (windowEndElement != null)
            {
                endDate = DateTime.Parse(windowEndElement.Value);
            }

            var repeatInstancesElement = ruleElement.Element("ruleElement");
            int? numberOfOccurrences = null;
            if (repeatInstancesElement != null)
            {
                numberOfOccurrences = int.Parse(repeatInstancesElement.Value);
            }

            var repeatElement = ruleElement.Element("repeat").Elements().FirstOrDefault();
            var repeatForeverElement = ruleElement.Element("repeatForever");
            var recurrenceDataTypeName = repeatElement.Name.ToString();

            switch (recurrenceDataTypeName)
            {
                case "daily":
                    var frequency = int.Parse(repeatElement.Attribute("dayFrequency").Value);
                    recurenceData = new DailyPattern()
                    {
                        EndDate = endDate,
                        Interval = frequency,
                        StartDate = value.EventDate,
                        NumberOfOccurrences = numberOfOccurrences
                    };
                    break;
            }

            return recurenceData;
        }

        public IEnumerable<RecurrenceData> Convert(IEnumerable<Reservation> values)
        {
            throw new NotImplementedException();
        }
    }
}
