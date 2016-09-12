using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMe.Core.Models.Recurrence
{
    public class Interval
    {
        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public bool IsAllDay { get; set; }
    }
}
