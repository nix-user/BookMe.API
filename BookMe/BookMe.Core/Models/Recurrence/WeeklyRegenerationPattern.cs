using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMe.Core.Models.Recurrence
{
    public sealed class WeeklyRegenerationPattern : IntervalPattern
    {
        public WeeklyRegenerationPattern(DateTime startDate, int interval)
        {
            this.StartDate = startDate;
            this.Interval = interval;
        }

        public override Interval GetBusyInterval(DateTime date)
        {
            throw new NotImplementedException();
        }
    }
}
