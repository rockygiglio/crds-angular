using System;
using System.Collections.Generic;
using System.Reflection;
using Crossroads.Utilities.Interfaces;
using log4net;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Repositories.Interfaces;

namespace MinistryPlatform.Translation.Repositories
{
    public class InvitationRepository : BaseRepository, IInvitationRepository
    {
        private readonly int _invitationPageId;
        private readonly ILog _logger = LogManager.GetLogger(typeof(InvitationRepository));


        private readonly IMinistryPlatformService _ministryPlatformService;

        public InvitationRepository(IMinistryPlatformService ministryPlatformService,
                               IConfigurationWrapper configurationWrapper,
                               IAuthenticationRepository authenticationService)
            : base(authenticationService, configurationWrapper)
        {
            _ministryPlatformService = ministryPlatformService;
            _invitationPageId = _configurationWrapper.GetConfigIntValue("InvitationPageID");
        }

        public MpInvitation CreateInvitation(MpInvitation dto, string token)
        {
            var invitationType = dto.InvitationType;

            var values = new Dictionary<string, object>
            {
                {"Source_ID", dto.SourceId},
                {"Email_Address", dto.EmailAddress},
                {"Recipient_Name", dto.RecipientName},
                {"Group_Role_ID", dto.GroupRoleId },
                {"Invitation_Type_ID", invitationType }
            };

            try
            {
                var invitationId = _ministryPlatformService.CreateRecord(_invitationPageId, values, token, true);
                var invitation = _ministryPlatformService.GetRecordDict(_invitationPageId, invitationId, token);

                dto.InvitationId = invitationId;
                dto.InvitationGuid = invitation["Invitation_GUID"].ToString();
                return dto;
            }
            catch (Exception e)
            {
                throw new ApplicationException(string.Format("Create Invitation failed.  Invitation Type: {0}, Source Id: {1}", dto.InvitationType, dto.SourceId), e);
            }
        }


    }
}