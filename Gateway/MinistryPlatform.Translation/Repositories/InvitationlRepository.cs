using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Crossroads.Utilities.Interfaces;
using log4net;
using MinistryPlatform.Translation.Extensions;
using MinistryPlatform.Translation.Helpers;
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

        public MpInvitation CreateInvitation(MpInvitation dto)
        {
            // override the user login to avoid granting rights to all users
            string token = ApiLogin();

            var invitationType = dto.InvitationType;

            var values = new Dictionary<string, object>
            {
                {"Source_ID", dto.SourceId},
                {"Email_Address", dto.EmailAddress},
                {"Recipient_Name", dto.RecipientName},
                {"Group_Role_ID", dto.GroupRoleId },
                {"Invitation_Type_ID", invitationType },
                {"Invitation_Date", dto.RequestDate }
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

        public MpInvitation GetOpenInvitation(string invitationGuid)
        {
            // override the user login to avoid granting rights to all users
            string token = ApiLogin();
            var mpInvitation = new MpInvitation();

            try
            {
                var searchString = string.Format(",,,,,{0},{1}", invitationGuid, false);
                var mpResults = _ministryPlatformService.GetRecordsDict(_invitationPageId, token, searchString, string.Empty);
                var invitation = (mpResults != null && mpResults.Count > 0) ? mpResults.FirstOrDefault() : null;

                if (invitation != null)
                {
                    mpInvitation = new MpInvitation
                    {
                        InvitationId = invitation.ToInt("dp_RecordID"),
                        SourceId = invitation.ToInt("Source_ID"),
                        EmailAddress = invitation.ToString("Email_address"),
                        GroupRoleId = invitation.ToInt("Group_Role_ID"),
                        InvitationType = invitation.ToInt("Invitation_Type_ID"),
                        RecipientName = invitation.ToString("Recipient_Name"),
                        RequestDate = invitation.ToDate("Invitation_Date")
                    };
                }
            }
            catch (Exception e)
            {
                throw new ApplicationException(string.Format("Get Invitation failed.  Invitation GUID: {0}, Invitation Used: {1}", invitationGuid, false), e);
            }

            return mpInvitation;
        }

        public void MarkInvitationAsUsed(string invitationGuid)
        {
            try
            {
                string token = ApiLogin();
                var invitation = GetOpenInvitation(invitationGuid);
            
                var dictionary = new Dictionary<string, object>();
                dictionary.Add("Invitation_ID", invitation.InvitationId);
                dictionary.Add("Invitation_GUID", invitationGuid);
                dictionary.Add("Invitation_Used", true);

                _ministryPlatformService.UpdateRecord(_invitationPageId, dictionary, token);
            }
            catch (Exception e)
            {
                throw new ApplicationException(string.Format("Update Invitation failed.  Invitation GUID: {0}", invitationGuid), e);
            }
        }

    }
}