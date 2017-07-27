using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Crossroads.Web.Common.Configuration;
using Crossroads.Web.Common.MinistryPlatform;
using Crossroads.Web.Common.Security;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Repositories.Interfaces;

namespace MinistryPlatform.Translation.Repositories
{
    public class WaiverRepository : BaseRepository,IWaiverRepository
    {

        private readonly IMinistryPlatformRestRepository _ministryPlatformRestRepository;

        public WaiverRepository(IMinistryPlatformRestRepository ministryPlatformRestRepository, IAuthenticationRepository authenticationService, IConfigurationWrapper configurationWrapper) : base(authenticationService, configurationWrapper)
        {
            _ministryPlatformRestRepository = ministryPlatformRestRepository;
        }

        public IObservable<MpWaivers> GetWaiver(int waiverId)
        {
            return Observable.Create<MpWaivers>(observer =>
            {
                try
                {
                    var apiToken = ApiLogin();
                    var res = _ministryPlatformRestRepository.UsingAuthenticationToken(apiToken).Get<MpWaivers>(waiverId);
                    observer.OnNext(res);
                    observer.OnCompleted();
                }
                catch (Exception e)
                {
                    observer.OnError(e);
                }               
                return Disposable.Empty;
            });
        }

        public IObservable<MpEventWaivers> GetEventWaivers(int eventId)
        {          
            var apiToken = ApiLogin();
            const string columnList = "Waiver_ID_Table.[Waiver_ID], Waiver_ID_Table.[Waiver_Name], Waiver_ID_Table.[Waiver_Text], cr_Event_Waivers.[Required]";
            var result = _ministryPlatformRestRepository.UsingAuthenticationToken(apiToken).Search<MpEventWaivers>($"Event_ID = {eventId} AND Active=1", columnList).ToList();
            return result.ToObservable();
        }

        public IObservable<MpEventParticipantWaiver> CreateEventParticipantWaiver(int waiverId, int eventParticipantId, int contactId)
        {
            return Observable.Start<MpEventParticipantWaiver>(() =>
            {
                var apiToken = ApiLogin();
                var eventParticipantWaiver = new MpEventParticipantWaiver
                {
                    Accepted = false,
                    EventParticipantId = eventParticipantId,
                    WaiverId = waiverId,
                    SignerId = contactId
                };
                var exists = _ministryPlatformRestRepository.UsingAuthenticationToken(apiToken).Search<MpEventParticipantWaiver>(
                    $"Event_Participant_ID_Table.[Event_Participant_ID] = {eventParticipantId} AND Waiver_ID_Table.[Waiver_ID] = {waiverId}");
                if (exists.Count > 0)
                {
                    return exists.First();
                }
                return _ministryPlatformRestRepository.UsingAuthenticationToken(apiToken).Create(eventParticipantWaiver);

            });           
        }

        public IObservable<MpEventParticipantWaiver> GetEventParticipantWaiversByContact(int eventId, int contactId)
        {
            throw new NotImplementedException();
        }
    }
}