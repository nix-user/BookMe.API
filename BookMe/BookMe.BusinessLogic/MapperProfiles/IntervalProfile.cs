using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BookMe.BusinessLogic.DTO;
using BookMe.Core.Models.Recurrence;

namespace BookMe.BusinessLogic.MapperProfiles
{
    public class IntervalProfile : Profile
    {
        public IntervalProfile()
        {
            this.CreateMap<IntervalDTO, Interval>();
        }
    }
}