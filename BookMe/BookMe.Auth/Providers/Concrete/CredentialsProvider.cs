using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;
using BookMe.Auth.Providers.Abstract;
using BookMe.Auth.Resources;
using BookMe.BusinessLogic.DTO;

namespace BookMe.Auth.Providers.Concrete
{
    public class CredentialsProvider : ICredentialsProvider
    {
        public CredentialsDTO Credentials
        {
            get
            {
                var claimsIdentity = HttpContext.Current.User?.Identity as ClaimsIdentity;

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
