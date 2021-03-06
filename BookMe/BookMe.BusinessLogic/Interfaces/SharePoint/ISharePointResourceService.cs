﻿using System;
using System.Collections.Generic;
using BookMe.BusinessLogic.DTO;
using BookMe.BusinessLogic.OperationResult;

namespace BookMe.BusinessLogic.Interfaces.SharePoint
{
    public interface ISharePointResourceService
    {
        OperationResult<IEnumerable<ResourceDTO>> GetAll();

        OperationResult<IEnumerable<ResourceDTO>> GetAvailableResources(ResourceFilterParameters resourceFilterParameters, IEnumerable<ResourceDTO> resources);

        OperationResult<IEnumerable<ReservationDTO>> GetRoomsReservations(IntervalDTO interval, IEnumerable<ResourceDTO> resources);
    }
}