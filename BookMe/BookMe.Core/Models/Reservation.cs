using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMe.Core.Models
{
    public class Reservation : BaseEntity
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime EventDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool IsRecurrence { get; set; }

        public long? ResourceId { get; set; }

        public TimeSpan Duration { get; set; }

        public string OwnerName { get; set; }
    }
}