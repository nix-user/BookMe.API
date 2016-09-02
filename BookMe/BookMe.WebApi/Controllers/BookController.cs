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
        private static List<ReservationModel> bookings = new List<ReservationModel>();

        public IEnumerable<ReservationModel> Get()
        {
            return bookings.Where(x => true);
        }

        public ReservationModel Get(int id)
        {
            return bookings.FirstOrDefault(x => x.Id == id);
        }

        [HttpPost]
        public void Post([FromBody]ReservationModel value)
        {
            bookings.Add(value);
            RoomController.Rooms.FirstOrDefault(x => x.Id == value.Room.Id).Bookings.Add(value);
        }

        public void Put(int id, [FromBody]string value)
        {
        }

        public void Delete(int id)
        {
            ReservationModel removeBook = bookings.FirstOrDefault(x => x.Id == id);
            bookings.Remove(removeBook);
            RoomController.Rooms.FirstOrDefault(x => x.Id == removeBook.Room.Id).Bookings.Remove(removeBook);
        }
    }
}