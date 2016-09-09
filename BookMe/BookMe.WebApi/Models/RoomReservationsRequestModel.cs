using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookMe.WebApi.Models
{
    public class RoomReservationsRequestModel
    {
        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public int RoomId { get; set; }
    }
}