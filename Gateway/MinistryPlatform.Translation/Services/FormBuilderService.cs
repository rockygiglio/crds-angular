using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Crossroads.Utilities.Interfaces;
using log4net;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Extensions;
using MinistryPlatform.Translation.Services.Interfaces;

namespace MinistryPlatform.Translation.Services
{
    public class FormBuilderService : BaseService, IGroupService
    {
        private readonly IConfigurationWrapper _configurationWrapper;
        private readonly ICommunicationService _communicationService;
        private readonly IContactService _contactService;
        private readonly IContentBlockService _contentBlockService;
        private readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly int GroupsParticipantsPageId = Convert.ToInt32(AppSettings("GroupsParticipants"));
        private readonly int JourneyGroupId = Convert.ToInt32(AppSettings("JourneyGroupId"));
        private readonly int JourneyGroupSearchPageViewId = Convert.ToInt32(AppSettings("JourneyGroupSearchPageViewId"));

        private readonly int GroupParticipantQualifiedServerPageView =
            Convert.ToInt32(AppSettings("GroupsParticipantsQualifiedServerPageView"));

        private IMinistryPlatformService ministryPlatformService;

        public FormBuilderService(IMinistryPlatformService ministryPlatformService, IConfigurationWrapper configurationWrapper, IAuthenticationService authenticationService, ICommunicationService communicationService, IContactService contactService, IContentBlockService contentBlockService)
            : base(authenticationService, configurationWrapper)
        {
            this.ministryPlatformService = ministryPlatformService;
            this._configurationWrapper = configurationWrapper;
            this._communicationService = communicationService;            
        }


        public List<GroupSearchResult> GetSearchResults(int pageId)
        {
            return Authorized(token =>
            {//TODO need to handle security and need to refactor this code
                var pageViewRecords = _ministryPlatformService.GetPageViewRecords(pageView, token);

                if (pageViewRecords.Count == 0)
                {
                    return null;
                }

                return Ok(pageViewRecords);

            });
        }

        private int GetSearchPageViewId(int groupTypeId)
        {
            if (groupTypeId == JourneyGroupId)
            {
                return JourneyGroupSearchPageViewId;
            }

            var message = string.Format("Could not find matching search page for group type {0}", groupTypeId);
            throw new ArgumentException(message);
        }


        public bool checkIfUserInGroup(int participantId, IList<GroupParticipant> groupParticipants)
        {
            return groupParticipants.Select(p => p.ParticipantId).Contains(participantId);
        }



        //public bool ParticipantQualifiedServerGroupMember(int groupId, int participantId)
        //{
        //    var searchString = string.Format(",{0},,{1}", groupId, participantId);
        //    var teams = ministryPlatformService.GetPageViewRecords(GroupParticipantQualifiedServerPageView, ApiLogin(), searchString);
        //    return teams.Count != 0;
        //}







    }
}