using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using BookMe.BusinessLogic.DTO;
using BookMe.BusinessLogic.Interfaces.SharePoint;
using BookMe.BusinessLogic.Services.Abstract;
using BookMe.WebApi.Models;

namespace BookMe.WebApi.Controllers
{
    [Authorize]
    public class ReservationController : ApiController
    {
        private readonly ISharePointReservationService sharePointReservationService;
        private readonly IReservationService reservationService;

        public ReservationController(ISharePointReservationService sharePointReservationService, IReservationService reservationService)
        {
            this.sharePointReservationService = sharePointReservationService;
            this.reservationService = reservationService;
        }

        [HttpPost]
        public ResponseModel Post([FromBody]ReservationModel value)
        {
            var operationResult = this.sharePointReservationService.AddReservation(Mapper.Map<ReservationModel, ReservationDTO>(value));

            return new ResponseModel()
            {
                IsOperationSuccessful = operationResult.IsSuccessful
            };
        }

        public ResponseModel<IEnumerable<ReservationModel>> Get()
        {
            var operationResult = this.sharePointReservationService.GetUserActiveReservations(User.Identity.Name);
            return new ResponseModel<IEnumerable<ReservationModel>>()
            {
                IsOperationSuccessful = operationResult.IsSuccessful,
                Result = operationResult.Result?.Select(Mapper.Map<ReservationDTO, ReservationModel>)
            };
        }

        public ResponseModel Delete(int id)
        {
            var operationResult = this.sharePointReservationService.RemoveReservation(id);

            return new ResponseModel()
            {
                IsOperationSuccessful = operationResult.IsSuccessful
            };
        }

        [Route("api/reservations/currentUser")]
        [HttpGet]
        public ResponseModel<UserReservationsModel> GetCurrentUserReservations()
        {
            var operationResult = this.reservationService.GetUserReservations(User.Identity.Name);
            return new ResponseModel<UserReservationsModel>()
            {
                IsOperationSuccessful = operationResult.IsSuccessful,
                Result = Mapper.Map<UserReservationsDTO, UserReservationsModel>(operationResult.Result)
            };
        }
    }
}