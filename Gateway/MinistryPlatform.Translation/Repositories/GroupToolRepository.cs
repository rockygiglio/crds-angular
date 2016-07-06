using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Crossroads.Utilities.Interfaces;
using log4net;
using MinistryPlatform.Translation.Extensions;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Repositories.Interfaces;

namespace MinistryPlatform.Translation.Repositories
{
    public class GroupToolRepository : BaseRepository, IGroupToolRepository
    {
        private readonly IConfigurationWrapper _configurationWrapper;
        private readonly ICommunicationRepository _communicationService;
        private readonly IContactRepository _contactService;
        private readonly IContentBlockService _contentBlockService;
        private readonly int _invitationPageId;
        private readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        private IMinistryPlatformService _ministryPlatformService;

        public GroupToolRepository(IMinistryPlatformService ministryPlatformService,
                               IConfigurationWrapper configurationWrapper,
                               IAuthenticationRepository authenticationService,
                               ICommunicationRepository communicationService)
            : base(authenticationService, configurationWrapper)
        {
            this._ministryPlatformService = ministryPlatformService;
            this._configurationWrapper = configurationWrapper;
            this._communicationService = communicationService;
            this._invitationPageId = _configurationWrapper.GetConfigIntValue("InvitationPageID");
        }

        public bool CreateInvitation(MpInvitation dto, string token)
        {
            
            var values = new Dictionary<string, object>
            {
                {"Source_ID", dto.SourceId},
                {"Email_Address", dto.EmailAddress},
                {"Recipient_Name", dto.RecipientName},
                {"Group_Role_ID", dto.GroupRoleId },
                {"Invitation_Type_ID", 1 }
            };

            try
            {
                var privateInviteId = _ministryPlatformService.CreateRecord(_invitationPageId, values, token, true);
                var record = _ministryPlatformService.GetRecordDict(_invitationPageId, privateInviteId, token, false);
                return true;
            }
            catch (Exception e)
            {
                throw new ApplicationException(string.Format("Create Private Invite failed.  Group Id: {0}", dto.SourceId), e);
            }
        }

        public List<MpInvitation> GetInvitees(int GroupId)
        {
            return new List<MpInvitation>();
        }

        //
        //        int addParticipantToGroup(int participantId,
        //                                  int groupId,
        //                                  int groupRoleId,
        //                                  Boolean childCareNeeded,
        //                                  DateTime startDate,
        //                                  DateTime? endDate = null,
        //                                  Boolean? employeeRole = false);
        //
        //        MpGroup getGroupDetails(int groupId);
        //
        //        bool checkIfUserInGroup(int participantId, IList<MpGroupParticipant> participants);
        //
        //
        //        void UpdateGroupRemainingCapacity(MpGroup group);
        //
        //        List<MpGroupParticipant> GetGroupParticipants(int groupId);
    }
}