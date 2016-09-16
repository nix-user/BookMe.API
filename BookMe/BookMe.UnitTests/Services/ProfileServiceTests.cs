using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookMe.BusinessLogic.DTO;
using BookMe.BusinessLogic.Services.Concrete;
using BookMe.Core.Models;
using BookMe.Infrastructure.MapperConfiguration;
using BookMe.UnitTests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BookMe.UnitTests.Services
{
    [TestClass]
    public class ProfileServiceTests
    {
        [TestInitialize]
        public void Init()
        {
            AutoMapperConfiguration.Configure();
        }

        [TestMethod]
        public void GetProfile_ThereIsNoUsersProfile_ShouldReturnNull()
        {
            // arrange
            const string userName = "adorable_user_name";
            UserProfile expectedUserProfile = null;
            var expectedIsSuccess = true;

            var fakeProfileRepository = new FakeRepository<UserProfile>();
            var profileService = new ProfileService(fakeProfileRepository);

            // act
            var result = profileService.GetProfile(userName);

            // assert 
            Assert.AreEqual(expectedUserProfile, result.Result);
            Assert.AreEqual(expectedIsSuccess, result.IsSuccessful);
        }

        [TestMethod]
        public void GetProfile_ThereIsUsersProfile_ShouldReturnNull()
        {
            // arrange
            const string userName = "adorable_user_name";
            var expectedProfile = new UserProfile()
            {
                UserName = userName,
                Id = 1,
                FavouriteRoom = "403a",
                Floor = 4
            };
            var expectedIsSuccess = true;

            var fakeProfileRepository = new FakeRepository<UserProfile>(new List<UserProfile>() { expectedProfile });
            var profileService = new ProfileService(fakeProfileRepository);

            // act
            var result = profileService.GetProfile(userName);

            // assert 
            Assert.AreEqual(expectedProfile.FavouriteRoom, result.Result.FavouriteRoom);
            Assert.AreEqual(expectedProfile.Floor, result.Result.Floor);
            Assert.AreEqual(expectedIsSuccess, result.IsSuccessful);
        }

        [TestMethod]
        public void UpdateProfile_ThereWasUsersProfile_ProfileIsUpdated()
        {
            // arrange
            const string userName = "adorable_user_name";
            var oldProfile = new UserProfile()
            {
                UserName = userName,
                Id = 1,
                FavouriteRoom = "403a",
                Floor = 4
            };

            var newProfile = new UserProfileDTO()
            {
                FavouriteRoom = "503b",
                Floor = 5
            };

            var expectedIsSuccess = true;

            var fakeProfileRepository = new FakeRepository<UserProfile>(new List<UserProfile>() { oldProfile });
            var profileService = new ProfileService(fakeProfileRepository);

            // act
            var updateResult = profileService.UpdateProfile(newProfile, userName);

            // assert 
            var receivedNewProfile = profileService.GetProfile(userName);

            Assert.AreEqual(newProfile.FavouriteRoom, receivedNewProfile.Result.FavouriteRoom);
            Assert.AreEqual(newProfile.Floor, receivedNewProfile.Result.Floor);
            Assert.AreEqual(expectedIsSuccess, updateResult.IsSuccessful);
        }

        [TestMethod]
        public void UpdateProfile_ThereWasNoUsersProfile_ProfileIsCreated()
        {
            // arrange
            const string userName = "adorable_user_name";

            var newProfile = new UserProfileDTO()
            {
                FavouriteRoom = "503b",
                Floor = 5
            };

            var expectedIsSuccess = true;

            var fakeProfileRepository = new FakeRepository<UserProfile>();
            var profileService = new ProfileService(fakeProfileRepository);

            // act
            var updateResult = profileService.UpdateProfile(newProfile, userName);

            // assert 
            var receivedNewProfile = profileService.GetProfile(userName);

            Assert.AreEqual(newProfile.FavouriteRoom, receivedNewProfile.Result.FavouriteRoom);
            Assert.AreEqual(newProfile.Floor, receivedNewProfile.Result.Floor);
            Assert.AreEqual(expectedIsSuccess, updateResult.IsSuccessful);
        }
    }
}
