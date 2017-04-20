using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using crds_angular.Models.Crossroads.GroupLeader;
using crds_angular.Models.Crossroads.Profile;
using crds_angular.Services.Interfaces;
using Crossroads.Web.Common.Configuration;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Repositories.Interfaces;

namespace crds_angular.Services
{
    public class GroupLeaderService : IGroupLeaderService
    {
        private readonly IPersonService _personService;
        private readonly IUserRepository _userRepository;
        private readonly IConfigurationWrapper _configWrapper;
        private readonly IFormSubmissionRepository _formSubmissionRepository;

        public GroupLeaderService(IPersonService personService, IUserRepository userRepository, IFormSubmissionRepository formSubmissionRepository, IConfigurationWrapper configWrapper)
        {
            _personService = personService;
            _userRepository = userRepository;
            _formSubmissionRepository = formSubmissionRepository;
            _configWrapper = configWrapper;
        }

        public async Task SaveProfile(string token, GroupLeaderProfileDTO leader)
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
                throw new Exception($"Unable to find the user account for {leader.OldEmail}", e);
            }
            await Observable.Zip(
                Observable.Start(() => _personService.SetProfile(token, person)),
                Observable.Start(() => _userRepository.UpdateUser(userUpdates))
            );
        }

        public IObservable<int> SaveSpiritualGrowth(string token, SpiritualGrowthDTO spiritualGrowth)
        {
            var form = new MpFormResponse()
            {
                ContactId = spiritualGrowth.ContactId,
                FormId = _configWrapper.GetConfigIntValue("GroupLeaderFormId"),
                FormAnswers = new List<MpFormAnswer>
                {
                    new MpFormAnswer
                    {
                        FieldId = _configWrapper.GetConfigIntValue("GroupLeaderFormStoryFieldId"),
                        Response = spiritualGrowth.Story
                    },
                    new MpFormAnswer
                    {
                        FieldId = _configWrapper.GetConfigIntValue("GroupLeaderFormTaughtFieldId"),
                        Response = spiritualGrowth.Taught
                    }
                }
            };
            Console.WriteLine(form.FormId);
            return Observable.Create<int>(observer =>
            {
                var responseId = _formSubmissionRepository.SubmitFormResponse(form);
                Console.WriteLine(responseId);
                if (responseId == 0)
                {
                    observer.OnError(new ApplicationException("Unable to submit Spiritual Growth form for Group Leader"));
                }
                observer.OnNext(responseId);
                observer.OnCompleted();
                return Disposable.Create(() => Console.WriteLine("Observable Destroyed"));
            });
        }
    }
}