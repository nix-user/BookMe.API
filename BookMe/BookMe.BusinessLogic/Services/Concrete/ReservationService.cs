using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookMe.BusinessLogic.DTO;
using BookMe.BusinessLogic.Interfaces.SharePoint;
using BookMe.BusinessLogic.OperationResult;
using BookMe.BusinessLogic.Services.Abstract;

namespace BookMe.BusinessLogic.Services.Concrete
{
    public class ReservationService : IReservationService
    {
        private ISharePointReservationService reservationService;
        private ISharePointResourceService resourceService;

        public ReservationService(ISharePointReservationService reservationService, ISharePointResourceService resourceService)
        {
            this.reservationService = reservationService;
            this.resourceService = resourceService;
        }

        public OperationResult<UserReservationsDTO> GetUserReservations(string userName)
        {
            var allResourcesRetrieval = this.resourceService.GetAll();
            if (!allResourcesRetrieval.IsSuccessful)
            {
                return new OperationResult<UserReservationsDTO>() { IsSuccessful = false };
            }

            var todayInterval = new IntervalDTO(DateTime.Today, DateTime.Today.AddDays(1));
            var todayReservationsRetrieval = this.reservationService.GetPossibleReservationsInInterval(todayInterval,
                allResourcesRetrieval.Result,
                userName);
            if (!todayReservationsRetrieval.IsSuccessful)
            {
                return new OperationResult<UserReservationsDTO>() { IsSuccessful = false };
            }

            var allReservationsRetrieval = this.reservationService.GetUserActiveReservations(userName);
            if (!allReservationsRetrieval.IsSuccessful)
            {
                return new OperationResult<UserReservationsDTO>() { IsSuccessful = false };
            }

            return new OperationResult<UserReservationsDTO>()
            {
                IsSuccessful = true,
                Result = new UserReservationsDTO()
                {
                    AllReservations = allReservationsRetrieval.Result,
                    TodayReservations = todayReservationsRetrieval.Result
                }
            };
        }
    }
}