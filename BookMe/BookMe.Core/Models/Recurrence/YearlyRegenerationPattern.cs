using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMe.Core.Models.Recurrence
{
    public sealed class YearlyRegenerationPattern : IntervalPattern
    {
        public YearlyRegenerationPattern(DateTime startDate, int interval)
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
