using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace BookMe.Infrastructure.MapperConfiguration
{
    public static class AutoMapperConfiguration
    {
        public static void Configure()
        {
            string webApiProjectName = "BookMe.WebApi";
            string bllProjectName = "BookMe.BusinessLogic";

            Mapper.Initialize(cfg =>
            cfg.AddProfiles(webApiProjectName, bllProjectName));
        }
    }
}