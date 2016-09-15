using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using BookMe.BusinessLogic.DTO;
using BookMe.BusinessLogic.Interfaces.SharePoint;
using BookMe.WebApi.Models;

namespace BookMe.WebApi.Controllers
{
    [Authorize]
    public class ReservationController : ApiController
    {
        private static List<ReservationModel> reservations = new List<ReservationModel>();
        private ISharePointReservationService reservationService;

        public ReservationController(ISharePointReservationService reservationService)
        {
            this.reservationService = reservationService;
        }

        [Route("api/reservation/all")]
        public IEnumerable<ReservationModel> GetAll()
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
            this.reservationService.AddReservation(Mapper.Map<ReservationModel, ReservationDTO>(value));
        }

        public void Put(int id, [FromBody]string value)
        {
        }

        public void Delete(int id)
        {
            ReservationModel removeReservation = reservations.FirstOrDefault(x => x.Id == id);
            reservations.Remove(removeReservation);
        }

        public ResponseModel<IEnumerable<ReservationModel>> Get()
        {
            var operationResult = this.reservationService.GetUserActiveReservations(User.Identity.Name);
            return new ResponseModel<IEnumerable<ReservationModel>>()
            {
                IsOperationSuccessful = operationResult.IsSuccessful,
                Result = operationResult.Result?.Select(Mapper.Map<ReservationDTO, ReservationModel>)
            };
        }
    }
}