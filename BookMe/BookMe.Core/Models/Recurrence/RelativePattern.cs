using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookMe.Core.Enums;

namespace BookMe.Core.Models.Recurrence
{
    public abstract class RelativePattern : RecurrenceData
    {
        public IEnumerable<DayOfTheWeek> DaysOfTheWeek { get; set; }

        public DayOfTheWeekIndex DayOfTheWeekIndex { get; set; }

        protected WeekRange GetNeededWeek(int firstWeekHasWeekdayIndex, int firstDayHasNotWeekdayIndex, IList<WeekRange> weeks, DateTime date)
        {
            var firstWeek = weeks[0];
            for (var d = firstWeek.Start; d.Date <= firstWeek.End; d = d.AddDays(1))
            {
                if (d.Month == date.Month && d.DayOfWeek == date.DayOfWeek && this.IsDateInDaysOfTheWeek(d, this.DaysOfTheWeek))
                {
                    return weeks[firstWeekHasWeekdayIndex];
                }
            }

            return weeks[firstDayHasNotWeekdayIndex];
        }
    }
}
