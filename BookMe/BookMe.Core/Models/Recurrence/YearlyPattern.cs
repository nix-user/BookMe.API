using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookMe.Core.Enums;

namespace BookMe.Core.Models.Recurrence
{
    public sealed class YearlyPattern : RecurrenceData
    {
        public YearlyPattern(DateTime startDate, Month month, int dayOfMonth)
        {
            this.StartDate = startDate;
            this.Month = month;
            this.DayOfMonth = dayOfMonth;
        }

        public int DayOfMonth { get; set; }

        public Month Month { get; set; }
    }
}
