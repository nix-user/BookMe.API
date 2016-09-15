using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using AutoMapper;
using BookMe.Auth.Resources;
using BookMe.BusinessLogic.DTO;
using BookMe.BusinessLogic.Services.Abstract;
using BookMe.WebApi.Models;

namespace BookMe.WebApi.Controllers
{
    public class ProfileController : ApiController
    {
        private readonly IProfileService profileService;

        private string UserName
        {
            get
            {
                var claimsIdentity = User?.Identity as ClaimsIdentity;
                return claimsIdentity?.Claims.FirstOrDefault(x => x.Type == ExtendedClaimTypes.UserName)?.Value;
            }
        }

        public ProfileController(IProfileService profileService)
        {
            this.profileService = profileService;
        }

        // GET: api/Profile
        public ProfileModel Get()
        {
            var profileDto = this.profileService.GetProfile(this.UserName);
            return Mapper.Map<ProfileModel>(profileDto);
        }

        // PUT: api/Profile/5
        public void Put([FromBody]ProfileModel value)
        {
            var profileDto = Mapper.Map<ProfileDTO>(value);
            this.profileService.UpdateProfile(profileDto, this.UserName);
        }
    }
}