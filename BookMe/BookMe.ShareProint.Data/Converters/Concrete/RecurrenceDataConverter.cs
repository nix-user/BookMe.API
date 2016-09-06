using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookMe.Core.Models.Recurrence;
using BookMe.ShareProint.Data.Converters.Abstract;

namespace BookMe.ShareProint.Data.Converters.Concrete
{
    public class RecurrenceDataConverter : IConverter<string, RecurrenceData>
    {
        public RecurrenceData Convert(string value)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<RecurrenceData> Convert(IEnumerable<string> values)
        {
            throw new NotImplementedException();
        }
    }
}
