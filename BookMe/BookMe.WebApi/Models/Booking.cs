
using System;

namespace BookMe.WebApi.Models
{
    public class Booking : IEquatable<Booking>
    {
        public int Id { get; set; }

        public TimeSpan From { get; set; }

        public TimeSpan To { get; set; }

        public DateTime Date { get; set; }

        public Room Room { get; set; }

        public User WhoBook { get; set; }

        public bool IsRecursive { get; set; }

        public TimeSpan Duration { get; set; }

        public bool Equals(Booking other)
        {
            return this.Id == other.Id;
        }
    }
}