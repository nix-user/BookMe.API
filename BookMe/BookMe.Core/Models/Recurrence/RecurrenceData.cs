using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMe.Core.Models.Recurrence
{
    public abstract class RecurrenceData
    {
        public DateTime? EndDate { get; set; }

        public int? NumberOfOccurrences { get; set; }

        public DateTime StartDate { get; set; }
    }
}
