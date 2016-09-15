using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookMe.BusinessLogic.DTO;

namespace BookMe.BusinessLogic.Services.Abstract
{
    public interface IProfileService
    {
        void UpdateProfile(ProfileDTO profile, string userName);

        ProfileDTO GetProfile(string userName);
    }
}