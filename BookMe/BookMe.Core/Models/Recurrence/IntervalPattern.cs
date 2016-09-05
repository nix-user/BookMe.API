using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMe.Core.Models.Recurrence
{
    public abstract class IntervalPattern : RecurrenceData
    {
        public int Interval { get; set; }
    }
}
