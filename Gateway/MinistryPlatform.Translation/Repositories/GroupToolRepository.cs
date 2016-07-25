using System;
using System.Collections.Generic;
using System.Reflection;
using Crossroads.Utilities.Interfaces;
using log4net;
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
        private readonly int _groupInquiresSubPageId;
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
            this._groupInquiresSubPageId = _configurationWrapper.GetConfigIntValue("GroupInquiresSubPage");
        }

        public List<MpInvitation> GetInvitations(int sourceId, int invitationTypeId, string token)
        {
            var mpInvitations = new List<MpInvitation>();
            try
            {
                var searchString = string.Format(",,,{0},{1},,false", invitationTypeId, sourceId);
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


        public List<MpInquiry> GetInquiries(int groupId, string token)
        {
            var mpInquiries = new List<MpInquiry>();
            try
            {
                var inquiries = _ministryPlatformService.GetSubPageRecords(this._groupInquiresSubPageId, groupId, token);
               
                // Translate object format from MP to an MpInquiry object
                if (inquiries != null && inquiries.Count > 0)
                {
                    foreach (Dictionary<string, object> p in inquiries)
                    {
                        mpInquiries.Add(new MpInquiry
                        {
                            InquiryId = p.ToInt("dp_RecordID"),
                            GroupId = groupId,
                            EmailAddress = p.ToString("Email"),
                            PhoneNumber = p.ToString("Phone"),
                            FirstName = p.ToString("First_Name"),
                            LastName = p.ToString("Last_Name"),
                            RequestDate = p.ToDate("Inquiry_Date"),
                            Placed = p.ToNullableBool("Placed"),
                            ContactId = p.ToInt("Contact_ID"),
                        });
                    }
                }
                else
                {
                    logger.Debug(string.Format("No pending inquires found for GroupId = {0} ", groupId));
                }
            }
            catch (Exception exception)
            {
                logger.Debug(string.Format("Exception thrown while retrieving inquiries for GroupId = {0}, InvitationTypeId = {1} ", groupId));
                logger.Debug(string.Format("Exception message:  {0} ", exception.Message));
            }
            return mpInquiries;
        }
    }
}