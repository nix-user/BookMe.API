using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMe.BusinessLogic.DTO
{
    public class IntervalDTO
    {
        public IntervalDTO(DateTime intervalStart, DateTime intervalEnd)
        {
            this.Start = intervalStart;
            this.End = intervalEnd;
        }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }
    }
}