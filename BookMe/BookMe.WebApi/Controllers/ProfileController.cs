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
    [Authorize]
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
        public ResponseModel<UserProfileModel> Get()
        {
            var operationResult = this.profileService.GetProfile(this.UserName);
            return new ResponseModel<UserProfileModel>()
            {
                IsOperationSuccessful = operationResult.IsSuccessful,
                Result = Mapper.Map<UserProfileModel>(operationResult.Result)
            };
        }

        // PUT: api/Profile/5
        public ResponseModel Put([FromBody]UserProfileModel value)
        {
            var profileDto = Mapper.Map<UserProfileDTO>(value);
            var operationResult = this.profileService.UpdateProfile(profileDto, this.UserName);
            return new ResponseModel()
            {
                IsOperationSuccessful = operationResult.IsSuccessful
            };
        }
    }
}