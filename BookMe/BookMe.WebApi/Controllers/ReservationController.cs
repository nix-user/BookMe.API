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
    public class ReservationController : ApiController
    {
        private readonly ISharePointReservationService reservationService;

        public ReservationController(ISharePointReservationService reservationService)
        {
            this.reservationService = reservationService;
        }

        [HttpPost]
        public ResponseModel Post([FromBody]ReservationModel value)
        {
            var operationResult = this.reservationService.AddReservation(Mapper.Map<ReservationModel, ReservationDTO>(value));

            return new ResponseModel()
            {
                IsOperationSuccessful = operationResult.IsSuccessful
            };
        }

        [Route("api/reservation/{userName}")]
        public ResponseModel<IEnumerable<ReservationModel>> GetUserReservations(string userName)
        {
            var operationResult = this.reservationService.GetUserActiveReservations(userName);
            return new ResponseModel<IEnumerable<ReservationModel>>()
            {
                IsOperationSuccessful = operationResult.IsSuccessful,
                Result = operationResult.Result?.Select(Mapper.Map<ReservationDTO, ReservationModel>)
            };
        }
    }
}