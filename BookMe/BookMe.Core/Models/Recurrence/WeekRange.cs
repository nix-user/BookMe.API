using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMe.Core.Models.Recurrence
{
    public class WeekRange
    {
        public DateTime Start { get; private set; }

        public DateTime End => this.Start.AddDays(6);

        public WeekRange(DateTime start)
        {
            this.Start = start;
        }
    }
}
