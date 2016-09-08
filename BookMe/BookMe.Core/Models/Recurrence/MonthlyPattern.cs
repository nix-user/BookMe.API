using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMe.Core.Models.Recurrence
{
    public sealed class MonthlyPattern : RecurrenceData
    {
        public int DayOfMonth { get; set; }

        public override bool IsBusyInDate(DateTime date)
        {
            throw new NotImplementedException();
        }
    }
}
