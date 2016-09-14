using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using BookMe.Auth.Resources;
using BookMe.BusinessLogic.DTO;

namespace BookMe.WebApi.Controllers
{
    public abstract class IdentityController : ApiController
    {
        protected CredentialsDTO Credentials
        {
            get
            {
                var claimsIdentity = User.Identity as ClaimsIdentity;

                if (claimsIdentity == null)
                {
                    return null;
                }

                var fullName = claimsIdentity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
                var userName = claimsIdentity.Claims.FirstOrDefault(x => x.Type == ExtendedClaimTypes.UserName)?.Value;
                var password = claimsIdentity.Claims.FirstOrDefault(x => x.Type == ExtendedClaimTypes.Password)?.Value;

                return new CredentialsDTO()
                {
                    FullName = fullName,
                    UserName = userName,
                    Password = password
                };
            }
        }
    }
}