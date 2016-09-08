using System;

namespace BookMe.Core
{
    public static class SystemTime
    {
        private static Func<DateTime> now = () => DateTime.Now;

        public static Func<DateTime> Now
        {
            get { return now; }
            set { now = value; }
        }
    }
}
