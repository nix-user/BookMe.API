using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BookMe.WebApi.Models;

namespace BookMe.WebApi.Controllers
{
    public class ReservationController : ApiController
    {
        private static List<ReservationModel> reservations = new List<ReservationModel>();

        public IEnumerable<ReservationModel> Get()
        {
            return reservations.Where(x => true);
        }

        public ReservationModel Get(int id)
        {
            return reservations.FirstOrDefault(x => x.Id == id);
        }

        [HttpPost]
        public void Post([FromBody]ReservationModel value)
        {
            reservations.Add(value);
        }

        public void Put(int id, [FromBody]string value)
        {
        }

        public void Delete(int id)
        {
            ReservationModel removeReservation = reservations.FirstOrDefault(x => x.Id == id);
            reservations.Remove(removeReservation);
        }
    }
}