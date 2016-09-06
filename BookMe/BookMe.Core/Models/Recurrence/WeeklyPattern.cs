using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookMe.Core.Enums;

namespace BookMe.Core.Models.Recurrence
{
    public sealed class WeeklyPattern : IntervalPattern
    {
        public WeeklyPattern(DateTime startDate, int interval, IEnumerable<DayOfTheWeek> daysOfTheWeek)
        {
            this.StartDate = startDate;
            this.Interval = interval;
            this.DaysOfTheWeek = daysOfTheWeek;
        }

        public IEnumerable<DayOfTheWeek> DaysOfTheWeek { get; }

        public DayOfWeek FirstDayOfWeek { get; set; }

        public override Interval GetBusyInterval(DateTime date)
        {
            throw new NotImplementedException();
        }
    }
}
