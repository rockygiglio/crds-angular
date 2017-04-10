using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Web;
using crds_angular.Models.Crossroads.GroupLeader;
using crds_angular.Models.Crossroads.Profile;
using crds_angular.Services.Interfaces;
using Microsoft.Ajax.Utilities;
using MinistryPlatform.Translation.Repositories.Interfaces;

namespace crds_angular.Services
{
    public class GroupLeaderService : IGroupLeaderService
    {
        private readonly IPersonService _personService;
        private readonly IUserRepository _userRepository;

        public GroupLeaderService(IPersonService personService, IUserRepository userRepository)
        {
            _personService = personService;
            _userRepository = userRepository;
        }

        public async void SaveProfile(string token, GroupLeaderProfileDTO leader)
        {
            var person = new Person
            {
                ContactId = leader.ContactId,
                CongregationId = leader.Site,
                NickName = leader.NickName,
                LastName = leader.LastName,
                EmailAddress = leader.Email,
                DateOfBirth = leader.BirthDate.ToShortDateString(),
                HouseholdId = leader.HouseholdId,
                MobilePhone = leader.MobilePhone,                
            };
            var userUpdates = person.GetUserUpdateValues();
            try
            {
                userUpdates["User_ID"] = _userRepository.GetUserIdByUsername(leader.OldEmail);                
            }
            catch (Exception e)
            {
                throw new ApplicationException($"Unable to find the user account for {leader.OldEmail}", e);
            }
            await Observable.CombineLatest(
                    Observable.Start(() => _personService.SetProfile(token, person)),
                    Observable.Start(() => _userRepository.UpdateUser(userUpdates)));
        }      
    }
}