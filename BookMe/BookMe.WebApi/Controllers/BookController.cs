using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BookMe.WebApi.Models;

namespace BookMe.WebApi.Controllers
{
    public class BookController : ApiController
    {
        private static List<Booking> bookings = new List<Booking>();

        public IEnumerable<Booking> Get()
        {
            return bookings.Where(x => true);
        }

        public Booking Get(int id)
        {
            return bookings.FirstOrDefault(x => x.Id == id);
        }

        [HttpPost]
        public void Post([FromBody]Booking value)
        {
            bookings.Add(value);
            RoomController.Rooms.FirstOrDefault(x => x.Id == value.Id).Bookings.Add(value);
        }

        public void Put(int id, [FromBody]string value)
        {
        }

        public void Delete(int id)
        {
            Booking removeBook = bookings.FirstOrDefault(x => x.Id == id);
            bookings.Remove(removeBook);
            RoomController.Rooms.FirstOrDefault(x => x.Id == removeBook.Room.Id).Bookings.Remove(removeBook);
        }
    }
}