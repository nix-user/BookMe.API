using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BookMe.WebApi.Models;

namespace BookMe.WebApi.Controllers
{
    public class RoomController : ApiController
    {
        public static List<Room> Rooms = new List<Room>()
        {
              new Room()
            {
                IsBig = false,
                IsHasPolykom = false,
                Number = "304D",
                Id = 6,
                Bookings = new List<Booking>()
            },
            new Room()
            {
                IsBig = false,
                IsHasPolykom = false,
                Number = "505E",
                Id = 5,
                Bookings = new List<Booking>()
            },
            new Room()
            {
                IsBig = false,
                IsHasPolykom = false,
                Number = "403D",
                Id = 1,
                Bookings = new List<Booking>()
            },
            new Room()
            {
                IsBig = true,
                IsHasPolykom = false,
                Number = "202K",
                Id = 2,
                Bookings = new List<Booking>()
            },
            new Room()
            {
                IsBig = true,
                IsHasPolykom = false,
                Number = "323P",
                Id = 3,
                Bookings = new List<Booking>()
            },
            new Room()
            {
                IsBig = false,
                IsHasPolykom = true,
                Number = "678T",
                Id = 4,
                Bookings = new List<Booking>()
            }
        };

        public IEnumerable<Room> Get()
        {
            return Rooms.Where(x => true);
        }

        public Room Get(int id)
        {
            return Rooms.FirstOrDefault(x => x.Id == id);
        }
        
        public void Post([FromBody]string value)
        {
        }

        public void Put(int id, [FromBody]string value)
        {
        }

        
        public void Delete(int id)
        {
        }
    }
}