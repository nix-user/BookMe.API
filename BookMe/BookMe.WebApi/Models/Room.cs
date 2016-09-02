﻿using System.Collections.Generic;

namespace BookMe.WebApi.Models
{
    public class Room
    {
        public Room()
        {
            this.Bookings = new List<Booking>();
        }

        public int Id { get; set; }

        public string Number { get; set; }

        public bool IsBig { get; set; }

        public bool IsHasPolykom { get; set; }

        public List<Booking> Bookings { get; set; }
    }
}