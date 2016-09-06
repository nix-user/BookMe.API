using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMe.Core.Models.Recurrence
{
    public sealed class MonthlyPattern : IntervalPattern
    {
        public MonthlyPattern(DateTime startDate, int interval, int dayOfMonth)
        {
            this.StartDate = startDate;
            this.Interval = interval;
            this.DayOfMonth = dayOfMonth;
        }

        public int DayOfMonth { get; set; }

        public override Interval GetBusyInterval(DateTime date)
        {
            throw new NotImplementedException();
        }
    }
}
