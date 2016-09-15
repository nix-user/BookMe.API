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

        public ResponseModel<IEnumerable<ReservationModel>> Get()
        {
            var operationResult = this.reservationService.GetUserActiveReservations(User.Identity.Name);
            return new ResponseModel<IEnumerable<ReservationModel>>()
            {
                IsOperationSuccessful = operationResult.IsSuccessful,
                Result = operationResult.Result?.Select(Mapper.Map<ReservationDTO, ReservationModel>)
            };
        }

        public ResponseModel Delete(int id)
        {
            var operationResult = this.reservationService.RemoveReservation(id);

            return new ResponseModel()
            {
                IsOperationSuccessful = operationResult.IsSuccessful
            };
        }
    }
}