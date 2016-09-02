using System;

namespace BookMe.WebApi.Models
{
    public class ReservationModel : IEquatable<ReservationModel>
    {
        public int Id { get; set; }

        public TimeSpan From { get; set; }

        public TimeSpan To { get; set; }

        public DateTime Date { get; set; }

        public Room Room { get; set; }

        public User Author { get; set; }

        public bool IsRecursive { get; set; }

        public TimeSpan Duration { get; set; }

        public bool Equals(ReservationModel other)
        {
            return this.Id == other.Id;
        }
    }
}