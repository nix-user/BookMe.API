using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookMe.Core.Enums;

namespace BookMe.Core.Models.Recurrence
{
    public sealed class RelativeYearlyPattern : RelativePattern
    {
        public Month Month { get; set; }

        protected override int CalculatePeriodsCount(DateTime to)
        {
            return to.Year - this.StartDate.Year;
        }

        protected override bool DoesMatchDateCondition(DateTime date)
        {
            if (date.Month != (int)this.Month)
            {
                return false;
            }

            return base.DoesMatchDateCondition(date);
        }

        protected override bool IsNextInterval(IList<DateTime> days, int index)
        {
            return days[index].Day == 1 && days[index].Month == 1 && index != 0;
        }
    }
}
