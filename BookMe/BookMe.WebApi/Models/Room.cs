using System.Collections.Generic;

namespace BookMe.WebApi.Models
{
    public class Room
    {
        public int Id { get; set; }

        public string Number { get; set; }

        public bool IsBig { get; set; }

        public bool IsHasPolykom { get; set; }
    }
}