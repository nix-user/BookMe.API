using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BookMe.BusinessLogic.DTO;
using BookMe.BusinessLogic.Repository;
using BookMe.BusinessLogic.Services.Abstract;
using Profile = BookMe.Core.Models.Profile;

namespace BookMe.BusinessLogic.Services.Concrete
{
    public class ProfileService : IProfileService
    {
        private readonly IRepository<Profile> profileRepository;

        public ProfileService(IRepository<Profile> profileRepository)
        {
            this.profileRepository = profileRepository;
        }

        public void UpdateProfile(ProfileDTO profile, string userName)
        {
            var profileEntity = this.profileRepository.Find(x => x.UserName == userName).FirstOrDefault();

            if (profileEntity != null)
            {
                profileEntity = Mapper.Map<Profile>(profile);
                this.profileRepository.Save();
            }
        }

        public ProfileDTO GetProfile(string userName)
        {
            var profileEntity = this.profileRepository.Find(x => x.UserName == userName).FirstOrDefault();
            return Mapper.Map<ProfileDTO>(profileEntity);
        }
    }
}