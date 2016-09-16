using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookMe.BusinessLogic.DTO;
using BookMe.BusinessLogic.OperationResult;

namespace BookMe.BusinessLogic.Services.Abstract
{
    public interface IProfileService
    {
        OperationResult.OperationResult UpdateProfile(UserProfileDTO profile, string userName);

        OperationResult<UserProfileDTO> GetProfile(string userName);
    }
}