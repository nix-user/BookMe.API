using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BookMe.BusinessLogic.DTO;
using BookMe.BusinessLogic.OperationResult;
using BookMe.BusinessLogic.Repository;
using BookMe.BusinessLogic.Services.Abstract;
using BookMe.Core.Models;

namespace BookMe.BusinessLogic.Services.Concrete
{
    public class ProfileService : IProfileService
    {
        private readonly IRepository<UserProfile> profileRepository;

        public ProfileService(IRepository<UserProfile> profileRepository)
        {
            this.profileRepository = profileRepository;
        }

        public OperationResult.OperationResult UpdateProfile(UserProfileDTO profile, string userName)
        {
            try
            {
                var profileEntity = this.profileRepository.Entities.FirstOrDefault(x => x.UserName == userName);

                if (profileEntity == null)
                {
                    profileEntity = new UserProfile()
                    {
                        UserName = userName
                    };

                    this.profileRepository.Insert(profileEntity);
                }

                profileEntity.FavouriteRoom = profile.FavouriteRoom;
                profileEntity.Floor = profile.Floor;

                this.profileRepository.Save();

                return new OperationResult.OperationResult() { IsSuccessful = true };
            }
            catch (Exception)
            {
                return new OperationResult.OperationResult() { IsSuccessful = false };
            }
        }

        public OperationResult<UserProfileDTO> GetProfile(string userName)
        {
            try
            {
                var profileEntity = this.profileRepository.Find(x => x.UserName == userName).FirstOrDefault();
                var profileDto = Mapper.Map<UserProfileDTO>(profileEntity);

                return new OperationResult<UserProfileDTO>()
                {
                    IsSuccessful = true,
                    Result = profileDto
                };
            }
            catch (Exception)
            {
                return new OperationResult<UserProfileDTO>()
                {
                    IsSuccessful = false
                };
            }
        }
    }
}