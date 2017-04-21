using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
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
        private readonly IParticipantRepository _participantRepository;

        public GroupLeaderService(IPersonService personService, IUserRepository userRepository, IFormSubmissionRepository formSubmissionRepository, IParticipantRepository participantRepository, IConfigurationWrapper configWrapper)
        {
            _personService = personService;
            _userRepository = userRepository;
            _formSubmissionRepository = formSubmissionRepository;
            _participantRepository = participantRepository;
            _configWrapper = configWrapper;
        }

        public IObservable<int> SaveReferences(GroupLeaderProfileDTO leader)
        {
            var form = new MpFormResponse
            {
                ContactId = leader.ContactId,
                FormId = _configWrapper.GetConfigIntValue("GroupLeaderFormId"),
                FormAnswers = new List<MpFormAnswer>
                {
                    new MpFormAnswer
                    {
                        FieldId = _configWrapper.GetConfigIntValue("GroupLeaderReferenceFieldId"),
                        Response = leader.ReferenceContactId
                    },
                    new MpFormAnswer
                    {
                         FieldId = _configWrapper.GetConfigIntValue("GroupLeaderHuddleFieldId"),
                        Response = leader.HuddleResponse
                    },
                    new MpFormAnswer
                    {
                         FieldId = _configWrapper.GetConfigIntValue("GroupLeaderStudentFieldId"),
                        Response = leader.LeadStudents
                    }
                }                   
            };
            return Observable.Create<int>(observer =>
            {
                var responseId = _formSubmissionRepository.SubmitFormResponse(form);
                if (responseId == 0)
                {
                   observer.OnError(new ApplicationException("Unable to submit form response for Group Leader"));
                }
                observer.OnNext(responseId);
                observer.OnCompleted();
                return Disposable.Create(() => Console.WriteLine("Observable destroyed"));
            });         
        }

        public void SetInterested(string token)
        {
            var participant = _participantRepository.GetParticipantRecord(token);
            SetGroupLeaderStatus(participant, _configWrapper.GetConfigIntValue("GroupLeaderInterested"));
        }

        public IObservable<IList<Unit>> SaveProfile(string token, GroupLeaderProfileDTO leader)
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
            return Observable.Zip(
                Observable.Start(() => _personService.SetProfile(token, person)),
                Observable.Start(() => _userRepository.UpdateUser(userUpdates)));
        }

        private void SetGroupLeaderStatus(MpParticipant participant, int statusId)
        {
            participant.GroupLeaderStatus = statusId;
            _participantRepository.UpdateParticipant(participant);
        }

        public IObservable<int> SaveSpiritualGrowth(SpiritualGrowthDTO spiritualGrowth)
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

            return Observable.Create<int>(observer =>
            {
                var responseId = _formSubmissionRepository.SubmitFormResponse(form);
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
