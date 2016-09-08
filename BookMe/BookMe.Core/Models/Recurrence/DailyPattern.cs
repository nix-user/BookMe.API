using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookMe.Core.Enums;

namespace BookMe.Core.Models.Recurrence
{
    public sealed class DailyPattern : RecurrenceData
    {
        public IEnumerable<DayOfTheWeek> DaysOfTheWeek { get; set; }

        public override bool IsBusyInDate(DateTime date)
        {
            throw new NotImplementedException();
        }
    }
}
