﻿using System;
using System.Collections.Generic;
using System.Linq;
using BookMe.BusinessLogic.DTO;
using BookMe.BusinessLogic.OperationResult;

namespace BookMe.BusinessLogic.Interfaces.SharePoint
{
    public interface ISharePointReservationService
    {
        OperationResult<IEnumerable<ReservationDTO>> GetPossibleReservationsInInterval(IntervalDTO interval, IEnumerable<ResourceDTO> resources);

        OperationResult<IEnumerable<ReservationDTO>> GetUserActiveReservations(string userName);

        OperationResult<IEnumerable<ReservationDTO>> GetUserReservations(IntervalDTO interval, string userName);

        OperationResult.OperationResult AddReservation(ReservationDTO reservationDTO);

        OperationResult.OperationResult RemoveReservation(int reservationId);
    }
}