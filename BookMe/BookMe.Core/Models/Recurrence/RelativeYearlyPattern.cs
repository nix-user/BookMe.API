using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookMe.Core.Enums;

namespace BookMe.Core.Models.Recurrence
{
    public sealed class RelativeYearlyPattern : IntervalPattern
    {
        public IEnumerable<DayOfTheWeek> DaysOfTheWeek { get; set; }

        public DayOfTheWeekIndex DayOfTheWeekIndex { get; set; }

        public Month Month { get; set; }

        public override Interval GetBusyInterval(DateTime date)
        {
            throw new NotImplementedException();
        }
    }
}
