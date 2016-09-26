using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using BookMe.BusinessLogic.DTO;
using BookMe.BusinessLogic.Interfaces.SharePoint;
using BookMe.BusinessLogic.OperationResult;
using BookMe.BusinessLogic.Services.Abstract;
using BookMe.Core.Enums;

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
                    AllReservations = this.CompleteAllReservations(allReservationsRetrieval.Result),
                    TodayReservations = this.CompleteTodayReservations(todayReservationsRetrieval.Result)
                }
            };
        }

        private IEnumerable<ReservationDTO> CompleteAllReservations(IEnumerable<ReservationDTO> allReservations)
        {
            var reservations = new List<ReservationDTO>(allReservations);

            reservations.RemoveAll(x => x.EventType == EventType.Deleted);

            var modifiedReservations = reservations.Where(x => x.EventType == EventType.Modified);
            foreach (var reservation in modifiedReservations)
            {
                reservation.IsRecurrence = false;
            }

            return reservations.OrderByDescending(x => x.EventDate);
        }

        private IEnumerable<ReservationDTO> CompleteTodayReservations(IEnumerable<ReservationDTO> todayReservations)
        {
            var reservations = new List<ReservationDTO>(todayReservations);

            var deletedReservationsIds = reservations.Where(x => x.EventType == EventType.Deleted).Select(x => x.Id);
            reservations.RemoveAll(x => x.ParentId != null && (x.EventType == EventType.Deleted || deletedReservationsIds.Contains(x.ParentId.Value)));

            var modifiedReservations = reservations.Where(x => x.EventType == EventType.Modified);
            var modifiedReservationsIds = modifiedReservations.Select(x => x.Id);
            reservations.RemoveAll(x => x.ParentId != null && modifiedReservationsIds.Contains(x.ParentId.Value));

            foreach (var reservation in modifiedReservations)
            {
                reservation.IsRecurrence = false;
            }

            return reservations.OrderByDescending(x => x.EventDate);
        }
    }
}