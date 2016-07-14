using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Crossroads.Utilities.Interfaces;
using log4net;
using MinistryPlatform.Translation;
using MinistryPlatform.Translation.Extensions;
using MinistryPlatform.Translation.Helpers;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Repositories.Interfaces;

namespace MinistryPlatform.Translation.Repositories
{
    public class GroupToolRepository : BaseRepository, IGroupToolRepository
    {
        private readonly IConfigurationWrapper _configurationWrapper;
        private readonly ICommunicationRepository _communicationService;
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

        public List<MpInvitation> GetInvitations(int sourceId, int invitationTypeId, string token)
        {
            var mpInvitations = new List<MpInvitation>();
            try
            {
                var searchString = string.Format(",,,{0},{1},,false", sourceId, invitationTypeId);
                var mpResults = _ministryPlatformService.GetRecords(_invitationPageId, token, searchString, string.Empty);
                var invitations = MPFormatConversion.MPFormatToList(mpResults);

                // Translate object format from MP to an MpInvitaion object
                if (invitations != null && invitations.Count > 0)
                {
                    foreach (Dictionary<string, object> p in invitations)
                    {
                        mpInvitations.Add(new MpInvitation
                        {
                            SourceId = p.ToInt("Source_ID"),
                            EmailAddress = p.ToString("Email_address"),
                            GroupRoleId = p.ToInt("Group_Role_ID"),
                            InvitationType = p.ToInt("Invitation_Type_ID"),
                            RecipientName = p.ToString("Recipient_Name"),
                            RequestDate = p.ToDate("Invitation_Date")
                        });

                    }
                }
                else
                {
                    logger.Debug(string.Format("No pending invitations found for SourceId = {0}, InvitationTypeId = {1} ", sourceId, invitationTypeId));
                }
            }
            catch (Exception exception)
            {
                logger.Debug(string.Format("Exception thrown while retrieving invitations for SourceId = {0}, InvitationTypeId = {1} ", sourceId, invitationTypeId));
                logger.Debug(string.Format("Exception message:  {0} ", exception.Message));
            }
            return mpInvitations;
        }

    }
}