using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Crossroads.Utilities.Interfaces;
using log4net;
using MinistryPlatform.Translation.Extensions;
using MinistryPlatform.Translation.Helpers;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Models.DTO;
using MinistryPlatform.Translation.Repositories.Interfaces;

namespace MinistryPlatform.Translation.Repositories
{
    public class GroupToolRepository : BaseRepository, IGroupToolRepository
    {
        public const string SearchGroupsProcName = "api_crds_SearchGroups";
        private readonly int _invitationPageId;
        private readonly int _groupInquiresSubPageId;
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IMinistryPlatformService _ministryPlatformService;
        private readonly IMinistryPlatformRestRepository _mpRestRepository;
        private readonly IApiUserRepository _apiUserRepository;

        public GroupToolRepository(IMinistryPlatformService ministryPlatformService,
                               IConfigurationWrapper configurationWrapper,
                               IAuthenticationRepository authenticationService,
                               IMinistryPlatformRestRepository mpRestRepository,
                               IApiUserRepository apiUserRepository)
            : base(authenticationService, configurationWrapper)
        {
            _ministryPlatformService = ministryPlatformService;
            _invitationPageId = _configurationWrapper.GetConfigIntValue("InvitationPageID");
            _groupInquiresSubPageId = _configurationWrapper.GetConfigIntValue("GroupInquiresSubPage");
            _mpRestRepository = mpRestRepository;
            _apiUserRepository = apiUserRepository;
        }

        public List<MpInvitation> GetInvitations(int sourceId, int invitationTypeId)
        {
            var mpInvitations = new List<MpInvitation>();
            try
            {
                var searchString = string.Format(",,,{0},{1},,false", invitationTypeId, sourceId);
                var mpResults = _ministryPlatformService.GetRecords(_invitationPageId, ApiLogin(), searchString, string.Empty);
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
                    _logger.Debug(string.Format("No pending invitations found for SourceId = {0}, InvitationTypeId = {1} ", sourceId, invitationTypeId));
                }
            }
            catch (Exception exception)
            {
                _logger.Debug(string.Format("Exception thrown while retrieving invitations for SourceId = {0}, InvitationTypeId = {1} ", sourceId, invitationTypeId));
                _logger.Debug(string.Format("Exception message:  {0} ", exception.Message));
            }
            return mpInvitations;
        }

        public List<MpGroupSearchResultDto> SearchGroups(int groupTypeId, string[] keywords = null)
        {
            var token = _apiUserRepository.GetToken();

            var parms = new Dictionary<string, object>
            {
                {"@GroupTypeId", groupTypeId}
            };
            if (keywords != null && keywords.Any())
            {
                parms.Add("@SearchString", string.Join(",", keywords));
            }

            var results = _mpRestRepository.UsingAuthenticationToken(token).GetFromStoredProc<MpGroupSearchResultDto>(SearchGroupsProcName, parms);
            return results?.FirstOrDefault();
        }

        public List<MpInquiry> GetInquiries(int groupId)
        {
            var mpInquiries = new List<MpInquiry>();
            try
            {
                var inquiries = _ministryPlatformService.GetSubPageRecords(_groupInquiresSubPageId, groupId, ApiLogin());
               
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
                    _logger.Debug(string.Format("No pending inquires found for GroupId = {0} ", groupId));
                }
            }
            catch (Exception exception)
            {
                _logger.Debug(string.Format("Exception thrown while retrieving inquiries for GroupId = {0}", groupId));
                _logger.Debug(string.Format("Exception message:  {0} ", exception.Message));
            }
            return mpInquiries;
        }
    }
}