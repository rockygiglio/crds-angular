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
    public class GroupService : BaseService, IGroupService
    {
        private readonly IConfigurationWrapper _configurationWrapper;
        private readonly ICommunicationService _communicationService;
        private readonly IContactService _contactService;
        private readonly IContentBlockService _contentBlockService;
        private readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly int GroupsParticipantsPageId = Convert.ToInt32(AppSettings("GroupsParticipants"));
        private readonly int GroupsParticipantsSubPageId = Convert.ToInt32(AppSettings("GroupsParticipantsSubPage"));
        private readonly int GroupsPageId = Convert.ToInt32(AppSettings("Groups"));
        private readonly int GroupsSubgroupsPageId = Convert.ToInt32(AppSettings("GroupsSubgroups"));
        private readonly int DefaultEmailContactId = Convert.ToInt32(AppSettings("DefaultContactEmailId"));
        private readonly int GroupSignupRelationsPageId = Convert.ToInt32((AppSettings("GroupSignUpRelations")));
        private readonly int CommunityGroupConfirmationTemplateId = Convert.ToInt32(AppSettings("CommunityGroupConfirmationTemplateId"));
        private readonly int CommunityGroupWaitListConfirmationTemplateId = Convert.ToInt32(AppSettings("CommunityGroupWaitListConfirmationTemplateId"));
        private readonly int MyCurrentGroupsPageView = Convert.ToInt32(AppSettings("MyCurrentGroupsPageView"));
        private readonly int JourneyGroupId = Convert.ToInt32(AppSettings("JourneyGroupId"));
        private readonly int JourneyGroupSearchPageViewId = Convert.ToInt32(AppSettings("JourneyGroupSearchPageViewId"));

        private readonly int GroupParticipantQualifiedServerPageView =
            Convert.ToInt32(AppSettings("GroupsParticipantsQualifiedServerPageView"));

        private IMinistryPlatformService ministryPlatformService;

        public GroupService(IMinistryPlatformService ministryPlatformService, IConfigurationWrapper configurationWrapper, IAuthenticationService authenticationService, ICommunicationService communicationService, IContactService contactService, IContentBlockService contentBlockService)
            : base(authenticationService, configurationWrapper)
        {
            this.ministryPlatformService = ministryPlatformService;
            this._configurationWrapper = configurationWrapper;
            this._communicationService = communicationService;
            this._contactService = contactService;
            this._contentBlockService = contentBlockService;
        }

        public int CreateGroup(Group group)
        {
            logger.Debug("Adding group");

            var addressId = (group.Address != null) ? group.Address.Address_ID : null;
            var endDate = group.EndDate.HasValue ? (object) group.EndDate.Value : null;

            var values = new Dictionary<string, object>
            {
                {"Group_Name", group.Name},
                {"Group_Type_ID", group.GroupType },
                {"Ministry_ID", group.MinistryId },
                {"Congregation_ID", group.CongregationId },
                {"Primary_Contact", group.ContactId },
                {"Description", group.GroupDescription },
                {"Start_Date", group.StartDate },
                {"End_Date", endDate},
                {"Target_Size", group.TargetSize },
                {"Offsite_Meeting_Address", addressId },
                {"Group_Is_Full", group.Full },
                {"Available_Online", group.AvailableOnline },
                {"Meeting_Time", group.MeetingTime },
                {"Meeting_Day_Id", group.MeetingDayId },
                {"Domain_ID", 1 },
                {"Child_Care_Available", group.ChildCareAvailable },
                {"Remaining_Capacity", group.RemainingCapacity },
                {"Enable_Waiting_List", group.WaitList },
                {"Online_RSVP_Minimum_Age", group.MinimumAge },
            };

            var groupId =
                WithApiLogin<int>(
                    apiToken =>
                    {
                        return (ministryPlatformService.CreateRecord(GroupsPageId, values, apiToken, true));
                    });

            logger.Debug("Added group " + groupId);
            return (groupId);
        }

        public int addParticipantToGroup(int participantId,
                                         int groupId,
                                         int groupRoleId,
                                         Boolean childCareNeeded,
                                         DateTime startDate,
                                         DateTime? endDate = null,
                                         Boolean? employeeRole = false)
        {
            logger.Debug("Adding participant " + participantId + " to group " + groupId);

            var values = new Dictionary<string, object>
            {
                {"Participant_ID", participantId},
                {"Group_Role_ID", groupRoleId},
                {"Start_Date", startDate},
                {"End_Date", endDate},
                {"Employee_Role", employeeRole},
                {"Child_Care_Requested", childCareNeeded}
            };

            var groupParticipantId =
                WithApiLogin<int>(
                    apiToken =>
                    {
                        return
                            (ministryPlatformService.CreateSubRecord(GroupsParticipantsPageId,
                                                                     groupId,
                                                                     values,
                                                                     apiToken,
                                                                     true));
                    });

            logger.Debug("Added participant " + participantId + " to group " + groupId + ": record id: " +
                         groupParticipantId);
            return (groupParticipantId);
        }

        public Group getGroupDetails(int groupId)
        {
            return (WithApiLogin<Group>(apiToken =>
            {
                logger.Debug("Getting group details for group " + groupId);
                var groupDetails = ministryPlatformService.GetRecordDict(GroupsPageId, groupId, apiToken, false);
                if (groupDetails == null)
                {
                    logger.Debug("No group found for group id " + groupId);
                    return (null);
                }
                var g = new Group();

                object gid = null;
                groupDetails.TryGetValue("Group_ID", out gid);
                if (gid != null)
                {
                    g.GroupId = (int) gid;
                }

                object gn = null;
                groupDetails.TryGetValue("Group_Name", out gn);
                if (gn != null)
                {
                    g.Name = (string) gn;
                }

                object gt = null;
                groupDetails.TryGetValue("Group_Type_ID", out gt);
                if (gt != null)
                {
                    g.GroupType = (int) gt;
                }

                object gsz = null;
                groupDetails.TryGetValue("Target_Size", out gsz);
                if (gsz != null)
                {
                    g.TargetSize = (short) gsz;
                }

                object gf = null;
                groupDetails.TryGetValue("Group_Is_Full", out gf);
                if (gf != null)
                {
                    g.Full = (Boolean) gf;
                }

                object gwl = null;
                groupDetails.TryGetValue("Enable_Waiting_List", out gwl);
                if (gwl != null)
                {
                    g.WaitList = (Boolean) gwl;
                }

                object gcc = null;
                groupDetails.TryGetValue("Child_Care_Available", out gcc);
                if (gcc != null)
                {
                    g.ChildCareAvailable = (Boolean) gcc;
                }

                object gc = null;
                groupDetails.TryGetValue("Congregation_ID_Text", out gc);
                if (gc != null)
                {
                    g.Congregation = (string)gc;
                }

                object ma = null;
                groupDetails.TryGetValue("Online_RSVP_Minimum_Age", out ma);
                if (ma != null)
                {
                    g.MinimumAge = (int) ma;
                }

                object rc = null;
                groupDetails.TryGetValue("Remaining_Capacity", out rc);
                if (rc != null)
                {
                    g.RemainingCapacity = (short)rc;
                }

                if (g.WaitList)
                {
                    var subGroups = ministryPlatformService.GetSubPageRecords(GroupsSubgroupsPageId,
                                                                              groupId,
                                                                              apiToken);
                    if (subGroups != null)
                    {
                        foreach (var i in subGroups)
                        {
                            if (i.ContainsValue("Wait List"))
                            {
                                object gd = null;
                                i.TryGetValue("dp_RecordID", out gd);
                                g.WaitListGroupId = (int) gd;
                                break;
                            }
                        }
                    }
                    else
                    {
                        logger.Debug("No wait list found for group id " + groupId);
                    }
                }

                g.Participants = LoadGroupParticipants(groupId, apiToken);

                logger.Debug("Group details: " + g);
                return (g);
            }));
        }

        public List<GroupParticipant> GetGroupParticipants(int groupId)
        {
            return (WithApiLogin(apiToken =>
            {
                return LoadGroupParticipants(groupId, apiToken);
            }));
        }

        public List<GroupSearchResult> GetSearchResults(int groupTypeId)
        {
            var apiToken = ApiLogin();
            var pageId = GetSearchPageViewId(groupTypeId);
            var records = ministryPlatformService.GetPageViewRecords(pageId, apiToken);

            return records.Select(record => new GroupSearchResult()
            {
                GroupId = record.ToInt("Group_ID"),
                ContactId = record.ToInt("Contact_Id"),
                Name = record.ToString("Group_Name"),
                GroupDescription = record.ToString("Description"),
                StartDate = record.ToDate("Start_Date"),
                EndDate = record.ToNullableDate("End_Date"),
                MeetingTime = record.ToString("Meeting_Time"),
                MeetingDayId = record.ToInt("Meeting_Day_ID"),

                Address = new Address()
                {
                    Address_ID = record.ToInt("Address_ID"),
                    Address_Line_1 = record.ToString("Address_Line_1"),
                    Address_Line_2 = record.ToString("Address_Line_2"),
                    City = record.ToString("City"),
                    State = record.ToString("State/Region"),
                    Postal_Code = record.ToString("Postal_Code")
                },
                RemainingCapacity = record.ToInt("Remaining_Capacity"),
                PrimaryContactName = record.ToString("Last_Name") + ", " + record.ToString("Nickname"),
                // TODO: Do we need email address?
                //PrimaryContactEmail = record.ToString("PrimaryEmail")

                SearchAttributes = new GroupSearchAttributes()
                {
                    TypeId = record.ToNullableInt("Group_Type"),
                    GoalId = record.ToNullableInt("Group_Goal"),
                    KidsId = record.ToNullableInt("Kids"),
                    DogId = record.ToNullableInt("Has_Dog"),
                    CatId = record.ToNullableInt("Has_Cat"),
                    MeetingRangeId = record.ToNullableInt("Meeting_Range")
                }
            }).ToList();
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

        private List<GroupParticipant> LoadGroupParticipants(int groupId, string token)
        {
                var groupParticipants = new List<GroupParticipant>();
                logger.Debug("Getting participants for group " + groupId);
                var participants = ministryPlatformService.GetSubpageViewRecords(GroupsParticipantsSubPageId, groupId, token);
                if (participants != null && participants.Count > 0)
                {
                    foreach (Dictionary<string, object> p in participants)
                    {
                        object pid = null;
                        p.TryGetValue("Participant_ID", out pid);
                        if (pid != null)
                        {
                            groupParticipants.Add(new GroupParticipant
                            {
                                ContactId = p.ToInt("Contact_ID"),
                                ParticipantId = p.ToInt("Participant_ID"),
                                GroupParticipantId = p.ToInt("dp_RecordID"),
                                GroupRoleId = p.ToInt("Group_Role_ID"),
                                GroupRoleTitle = p.ToString("Role_Title"),
                                LastName = p.ToString("Last_Name"),
                                NickName = p.ToString("Nickname"),
                                Email = p.ToString("Email")
                            });
                        }
                    }
                }
                else
                {
                    logger.Debug("No participants found for group id " + groupId);
                }
                return groupParticipants;
        }

        public IList<Event> getAllEventsForGroup(int groupId)
        {
            var apiToken = ApiLogin();
            var groupEvents = ministryPlatformService.GetSubpageViewRecords("GroupEventsSubPageView", groupId, apiToken);
            if (groupEvents == null || groupEvents.Count == 0)
            {
                return null;
            }
            return groupEvents.Select(tmpEvent => new Event
            {
                EventId = tmpEvent.ToInt("Event_ID"),
                Congregation = tmpEvent.ToString("Congregation_Name"),
                EventStartDate = tmpEvent.ToDate("Event_Start_Date"),
                EventEndDate = tmpEvent.ToDate("Event_End_Date"),
                EventTitle = tmpEvent.ToString("Event_Title")
            }).ToList();
        }

        public IList<string> GetEventTypesForGroup(int groupId, string token)
        {
            var loginToken = token ?? ApiLogin();
            var records = ministryPlatformService.GetSubpageViewRecords("GroupOpportunitiesEvents", groupId, loginToken);
            return records.Select(e =>
            {
                try
                {
                    return e.ToString("Event Type");
                }
                catch (Exception exception)
                {
                    logger.Debug("tried to parse a Event_Type_ID for a record and failed");
                    return String.Empty;
                }
            }).ToList();
        }

        public bool ParticipantQualifiedServerGroupMember(int groupId, int participantId)
        {
            var searchString = string.Format(",{0},,{1}", groupId, participantId);
            var teams = ministryPlatformService.GetPageViewRecords(GroupParticipantQualifiedServerPageView, ApiLogin(), searchString);
            return teams.Count != 0;
        }

        public bool ParticipantGroupMember(int groupId, int participantId)
        {
            var searchString = string.Format(",{0},{1}", groupId, participantId);
            var teams = ministryPlatformService.GetPageViewRecords("GroupParticipantsById", ApiLogin(), searchString);
            return teams.Count != 0;
        }

        public bool checkIfUserInGroup(int participantId, IList<GroupParticipant> groupParticipants)
        {
            return groupParticipants.Select(p => p.ParticipantId).Contains(participantId);
        }

        public bool checkIfRelationshipInGroup(int relationshipId, IList<int> currRelationshipsList)
        {
            return currRelationshipsList.Contains(relationshipId);
        }

        public List<GroupSignupRelationships> GetGroupSignupRelations(int groupType)
        {
            var response = WithApiLogin<List<GroupSignupRelationships>>(
                apiToken =>
                {
                    var relationRecords = ministryPlatformService.GetSubPageRecords(GroupSignupRelationsPageId,
                                                                                    groupType,
                                                                                    apiToken);

                    return relationRecords.Select(relationRecord => new GroupSignupRelationships
                    {
                        RelationshipId = relationRecord.ToInt("Relationship_ID"),
                        RelationshipMinAge = relationRecord.ToNullableInt("Min_Age"),
                        RelationshipMaxAge = relationRecord.ToNullableInt("Max_Age")
                    }).ToList();
                });
            return response;
        }

        public List<Group> GetGroupsForEvent(int eventId)
        {
            var searchString = eventId + ",";
            var pageViewId = _configurationWrapper.GetConfigIntValue("GroupsByEventId");
            var token = ApiLogin();
            var records = ministryPlatformService.GetPageViewRecords(pageViewId, token, searchString);
            if (records == null)
            {
                return null;
            }
            return records.Select(record => new Group
            {
                GroupId = record.ToInt("Group_ID"),
                Name = record.ToString("Group_Name")
            }).ToList();
        }

        public void SendCommunityGroupConfirmationEmail(int participantId, int groupId, bool waitlist, bool childcareNeeded)
        {
            var emailTemplate = _communicationService.GetTemplate(waitlist ? CommunityGroupWaitListConfirmationTemplateId : CommunityGroupConfirmationTemplateId);
            var toContact = _contactService.GetContactIdByParticipantId(participantId);
            var toContactInfo = _contactService.GetContactById(toContact);
            var groupInfo = getGroupDetails(groupId);

            var mergeData = new Dictionary<string, object>
            {
                {"Nickname", toContactInfo.Nickname},
                {"Group_Name", groupInfo.Name},
                {"Congregation_Name", groupInfo.Congregation},
                {"Childcare_Needed", (childcareNeeded) ? _contentBlockService["communityGroupChildcare"].Content : ""}
            };

            var domainId = Convert.ToInt32(AppSettings("DomainId"));
            var from = new Contact()
            {
                ContactId = DefaultEmailContactId,
                EmailAddress = _communicationService.GetEmailFromContactId(DefaultEmailContactId)
            };

            var to = new List<Contact>
            {
                new Contact {
                    ContactId = toContact,
                    EmailAddress = toContactInfo.Email_Address
                }
            };

            var confirmation = new Communication 
            { 
                EmailBody = emailTemplate.Body, 
                EmailSubject = emailTemplate.Subject,
                AuthorUserId = 5,
                DomainId = domainId,
                FromContact = from,
                MergeData = mergeData,
                ReplyToContact = from,
                TemplateId = CommunityGroupConfirmationTemplateId,
                ToContacts = to
            };
            _communicationService.SendMessage(confirmation);
        }

        public List<GroupParticipant> getEventParticipantsForGroup(int groupId, int eventId)
        {
            var records = ministryPlatformService.GetPageViewRecords("ParticipantsByGroupAndEvent", ApiLogin(), String.Format("{0},{1}", groupId, eventId));
            return records.Select(rec => new GroupParticipant
            {
                ContactId = rec.ToInt("Contact_ID"), NickName = rec.ToString("Nickname"), LastName = rec.ToString("Last_Name")
            }).ToList();
        }

        public List<Group> GetGroupsByTypeForParticipant(string token, int participantId, int groupTypeId)
        {
            var groupDetails = ministryPlatformService.GetPageViewRecords(MyCurrentGroupsPageView, token, String.Format(",,{0}", groupTypeId));
            if (groupDetails == null || groupDetails.Count == 0)
            {
                return new List<Group>();
            }
            return groupDetails.Select(details => new Group()
            {
                GroupId = details.ToInt("Group_ID"),
                CongregationId = details.ToInt("Congregation_ID"),
                Name = details.ToString("Group_Name"),
                GroupRoleId = details.ToInt("Group_Role_ID"),
                GroupDescription = details.ToString("Description"),
                MinistryId = details.ToInt("Ministry_ID"),
                ContactId = details.ToInt("Primary_Contact"),
                PrimaryContactName = details.ToString("Primary_Contact_Name"),
                PrimaryContactEmail = details.ToString("Primary_Contact_Email"),
                GroupType = details.ToInt("Group_Type_ID"),
                StartDate = details.ToDate("Start_Date"),
                EndDate = details.ToNullableDate("End_Date"),
                MeetingDayId = details.ToInt("Meeting_Day_ID"),
                MeetingTime = details.ToString("Meeting_Time"),
                AvailableOnline = details.ToBool("Available_Online"),
                Address = new Address()
                {
                    Address_ID = details.ToInt("Address_ID"),
                    Address_Line_1 = details.ToString("Address_Line_1"),
                    Address_Line_2 = details.ToString("Address_Line_2"),
                    City = details.ToString("City"),
                    State = details.ToString("State"),
                    Postal_Code = details.ToString("Zip_Code"),
                    Foreign_Country = details.ToString("Foreign_Country")
                }
            }).ToList();
        }

        public void UpdateGroupRemainingCapacity(Group group)
        {
            logger.Debug("Updating group: " + group.GroupId + " : " + group.Name);

            var values = new Dictionary<string, object>
            {
                {"Group_ID", group.GroupId},
                {"Remaining_Capacity", group.RemainingCapacity },
            };

            var retValue = WithApiLogin<int>(token =>
            {
                try
                {
                    ministryPlatformService.UpdateRecord(GroupsPageId, values, token);
                    return 1;
                }
                catch (Exception e)
                {
                    throw new ApplicationException("Error updating group: " + e.Message);
                }
            });

            logger.Debug("updated group: " + group.GroupId);
        }
    }
}