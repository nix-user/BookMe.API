using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMe.Core.Models.Recurrence
{
    public class Interval
    {
        public Interval()
        {
        }

        public Interval(DateTime intervalStart, DateTime intervalEnd)
        {
            this.Start = intervalStart;
            this.End = intervalEnd;
        }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public bool IsIntersecting(Interval interval)
        {
            return this.Start < interval.End && this.End > interval.Start;
        }
    }
}