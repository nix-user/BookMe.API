using System;
using System.Collections.Generic;
using BookMe.BusinessLogic.DTO;
using BookMe.BusinessLogic.OperationResult;

namespace BookMe.BusinessLogic.Interfaces.SharePoint
{
    public interface ISharePointReservationService
    {
        OperationResult<IEnumerable<ReservationDTO>> GetPossibleReservationsInInterval(DateTime intervalStart, DateTime intervalEnd);

        OperationResult<IEnumerable<ReservationDTO>> GetUserActiveReservations(string userName);

        OperationResult.OperationResult AddReservation(ReservationDTO reservationDTO);
    }
}
