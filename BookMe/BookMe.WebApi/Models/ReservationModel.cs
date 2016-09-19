using System;

namespace BookMe.WebApi.Models
{
    public class ReservationModel : IEquatable<ReservationModel>
    {
        public string Title { get; set; }

        public int Id { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public string Author { get; set; }

        public bool IsRecursive { get; set; }

        public int? ResourceId { get; set; }

        public TimeSpan Duration { get; set; }

        public Room Room { get; set; }

        public bool Equals(ReservationModel other)
        {
            return this.Id == other.Id;
        }
    }
}