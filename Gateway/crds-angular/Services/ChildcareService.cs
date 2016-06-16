using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reactive.Linq;
using crds_angular.Exceptions;
using crds_angular.Models.Crossroads.Childcare;
using crds_angular.Models.Crossroads.Serve;
using crds_angular.Services.Interfaces;
using crds_angular.Util.Interfaces;
using Crossroads.Utilities.Interfaces;
using Crossroads.Utilities.Services;
using log4net;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Models.Childcare;
using MinistryPlatform.Translation.Services.Interfaces;

namespace crds_angular.Services
{
    public class ChildcareService : IChildcareService
    {
        private readonly IChildcareRequestService _childcareRequestService;
        private readonly ICommunicationService _communicationService;
        private readonly IConfigurationWrapper _configurationWrapper;
        private readonly IContactService _contactService;
        private readonly IEventParticipantService _eventParticipantService;
        private readonly MinistryPlatform.Translation.Services.Interfaces.IEventService _eventService;
        private readonly crds_angular.Services.Interfaces.IEventService _crdsEventService;
        private readonly IParticipantService _participantService;
        private readonly IServeService _serveService;
        private readonly IDateTime _dateTimeWrapper;
        private readonly IApiUserService _apiUserService;

        private readonly ILog _logger = LogManager.GetLogger(typeof (ChildcareService));

        public ChildcareService(IEventParticipantService eventParticipantService,
                                ICommunicationService communicationService,
                                IConfigurationWrapper configurationWrapper,
                                IContactService contactService,
                                MinistryPlatform.Translation.Services.Interfaces.IEventService eventService,
                                IParticipantService participantService,
                                IServeService serveService,
                                IDateTime dateTimeWrapper,
                                IApiUserService apiUserService, 
                                Interfaces.IEventService crdsEventService, 
                                IChildcareRequestService childcareRequestService)
        {
            _childcareRequestService = childcareRequestService;
            _eventParticipantService = eventParticipantService;
            _communicationService = communicationService;
            _configurationWrapper = configurationWrapper;
            _contactService = contactService;
            _crdsEventService = crdsEventService;
            _eventService = eventService;
            _participantService = participantService;
            _serveService = serveService;
            _dateTimeWrapper = dateTimeWrapper;
            _apiUserService = apiUserService;
        }

        public List<FamilyMember> MyChildren(string token)
        {
            var family = _serveService.GetImmediateFamilyParticipants(token);
            var myChildren = new List<FamilyMember>();

            foreach (var member in family)
            {
                var schoolGrade = SchoolGrade(member.HighSchoolGraduationYear);
                var maxAgeWithoutGrade = _configurationWrapper.GetConfigIntValue("MaxAgeWithoutGrade");
                var maxGradeForChildcare = _configurationWrapper.GetConfigIntValue("MaxGradeForChildcare");
                if (member.Age == 0)
                {
                    continue;
                }
                if (schoolGrade == 0 && member.Age <= maxAgeWithoutGrade)
                {
                    myChildren.Add(member);
                }
                else if (schoolGrade > 0 && schoolGrade <= maxGradeForChildcare)
                {
                    myChildren.Add(member);
                }
            }
            return myChildren;
        }

        public void SaveRsvp(ChildcareRsvpDto saveRsvp, string token)
        {
            var participant = _participantService.GetParticipantRecord(token);
            var participantId = 0;

            try
            {
                foreach (var p in saveRsvp.ChildParticipants)
                {
                    participantId = p;
                    _eventService.SafeRegisterParticipant(saveRsvp.EventId, participantId);
                }

                //send email to parent
                SendConfirmation(saveRsvp.EventId,participant,saveRsvp.ChildParticipants);
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("Save RSVP failed for event ({0}), participant ({1})", saveRsvp.EventId, participantId), ex);
            }
        }

        public void CreateChildcareRequest(ChildcareRequestDto request, String token)
        {
            var mpRequest = request.ToMPChildcareRequest();
            var childcareRequestId = _childcareRequestService.CreateChildcareRequest(mpRequest);
            _childcareRequestService.CreateChildcareRequestDates(childcareRequestId, mpRequest, token);
            try
            {
                var childcareRequest = _childcareRequestService.GetChildcareRequest(childcareRequestId, token);
                SendChildcareRequestNotification(childcareRequest);
            }
           catch (Exception ex)
            {
                _logger.Error(string.Format("Save Request failed"), ex);
            }

        }

        // TODO: Should we merge childcareRequestId into the childcareRequestDto?
        public void ApproveChildcareRequest(int childcareRequestId, ChildcareRequestDto childcareRequest, string token)
        {
            try
            {
                var request = GetChildcareRequestForReview(childcareRequestId, token);
                var datesFromRequest = _childcareRequestService.GetChildcareRequestDates(childcareRequestId);
                var requestedDates = childcareRequest.DatesList.Select(date => GetChildcareDateFromList(datesFromRequest, date)).ToList();
                if (requestedDates.Count == 0)
                {
                    throw new ChildcareDatesMissingException(childcareRequestId);
                }

                var childcareEvents = _childcareRequestService.FindChildcareEvents(childcareRequestId, requestedDates);
                var missingDates = requestedDates.Where(childcareRequestDate => !childcareEvents.ContainsKey(childcareRequestDate.ChildcareRequestDateId)).ToList();
                if (missingDates.Count > 0)
                {
                    var dateList = missingDates.Aggregate("", (current, date) => current + ", " + date.RequestDate.ToShortDateString());
                    _logger.Debug("Missing events for dates: ${dateList}");
                    var dateMap = missingDates.Select(c => c.RequestDate).ToList();
                    throw new EventMissingException(dateMap);
                }

                //set the approved column for dates to true
                foreach (var ccareDates in requestedDates)
                {
                    _childcareRequestService.DecisionChildcareRequestDate(ccareDates.ChildcareRequestDateId, true);
                    var eventId = childcareEvents.Where((ev) => ev.Key == ccareDates.ChildcareRequestDateId).Select( (ev) => ev.Value).SingleOrDefault();
                    var eventGroup = new MpEventGroup() {Closed = false, Created = true, EventId = eventId, GroupId = request.GroupId};
                    var currentGroups = _eventService.GetGroupsForEvent(eventId).Select((g) => g.GroupId).ToList();
                    if (!currentGroups.Contains(request.GroupId))
                    {
                        _eventService.CreateEventGroup(eventGroup, token);
                    }
                }

                var requestStatusId = GetApprovalStatus(datesFromRequest, requestedDates);
                _childcareRequestService.DecisionChildcareRequest(childcareRequestId, requestStatusId, childcareRequest.ToMPChildcareRequest());
                var templateId = GetApprovalEmailTemplate(requestStatusId);
                SendChildcareRequestDecisionNotification(childcareRequestId, requestedDates, childcareRequest, templateId, token);
            }
            catch (EventMissingException ex)
            {
                throw;
            }
            catch (ChildcareDatesMissingException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("Update Request failed"), ex);
                throw new Exception("Approve Childcare failed", ex);
            }
        }

        private int GetApprovalEmailTemplate(int requestStatusId)
        {
            if (requestStatusId == 1)
            {
                return _configurationWrapper.GetConfigIntValue("ChildcareRequestApprovalNotificationTemplate");
            }
            else
            {
                return _configurationWrapper.GetConfigIntValue("ChildcareRequestConditionalApprovalNotificationTemplate");
            }
        }

        private MpChildcareRequestDate GetChildcareDateFromList(List<MpChildcareRequestDate> allDates, DateTime date)
        {
            var requestedDate = new MpChildcareRequestDate();
            return allDates.SingleOrDefault(d => date.Date == d.RequestDate.Date); 
        }

        private int GetApprovalStatus(List<MpChildcareRequestDate> datesFromMP, List<MpChildcareRequestDate> datesApproving)
        {
            if (datesFromMP.Count > datesApproving.Count)
            {
                return _configurationWrapper.GetConfigIntValue("ChildcareRequestConditionallyApproved");
            }
            return _configurationWrapper.GetConfigIntValue("ChildcareRequestApproved");
        }

        public void RejectChildcareRequest(int childcareRequestId, ChildcareRequestDto childcareRequest, string token)
        {
            try
            {
                //set the approved column for dates to false
                var childcareDates = _childcareRequestService.GetChildcareRequestDates(childcareRequestId);
                foreach (var ccareDates in childcareDates)
                {
                    _childcareRequestService.DecisionChildcareRequestDate(ccareDates.ChildcareRequestDateId, false);
                }

                _childcareRequestService.DecisionChildcareRequest(childcareRequestId, _configurationWrapper.GetConfigIntValue("ChildcareRequestRejected"), childcareRequest.ToMPChildcareRequest());
                var templateId = _configurationWrapper.GetConfigIntValue("ChildcareRequestRejectionNotificationTemplate");
                SendChildcareRequestDecisionNotification(childcareRequestId, childcareDates, childcareRequest, templateId, token);
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("Update Request failed"), ex);
                throw new Exception("Reject Childcare failed", ex);
            }
        }

        public MpChildcareRequest GetChildcareRequestForReview(int childcareRequestId, string token)
        {
            try
            {
                return _childcareRequestService.GetChildcareRequestForReview(childcareRequestId);
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("GetChildcareRequestForReview failed"), ex);
            }
            return null;
        }

        private void SendChildcareRequestDecisionNotification(int requestId, List<MpChildcareRequestDate> childcareRequestDates, ChildcareRequestDto childcareRequest, int templateId, String token)
        {
            var childcareRequestEmail = _childcareRequestService.GetChildcareRequest(requestId, token);;
            var template = _communicationService.GetTemplate(templateId);

            var decisionNotes = childcareRequest.DecisionNotes ?? "N/A";

           
            var authorUserId = _configurationWrapper.GetConfigIntValue("DefaultUserAuthorId");
            var datesList = childcareRequestDates.Select(dateRec => dateRec.RequestDate).Select(requestDate => BuildParagraph("", requestDate.ToShortDateString())).ToList();
            var styles = Styles();
            var htmlCell = new HtmlElement("td", styles).Append(datesList);
            var htmlRow = new HtmlElement("tr", styles).Append(htmlCell);
            var htmlTBody = new HtmlElement("tbody", styles).Append(htmlRow);
            var htmlTable = new HtmlElement("table", styles).Append(htmlTBody);

            var mergeData = new Dictionary<string, object>
            {
                {"Group", childcareRequestEmail.GroupName},
                {"ChildcareSession", childcareRequestEmail.ChildcareSession},
                {"DecisionNotes", decisionNotes },
                {"Frequency", childcareRequest.Frequency},
                {"Dates", htmlTable.Build() },
                {"RequestId", childcareRequestEmail.RequestId },
                {"Base_Url", _configurationWrapper.GetConfigValue("BaseMPUrl")},
                {"Congregation", childcareRequestEmail.CongregationName }
            };
            var toContactsList = new List<MpContact> {new MpContact {ContactId = childcareRequestEmail.RequesterId, EmailAddress = childcareRequestEmail.RequesterEmail}};


            var communication = new MpCommunication
            {
                AuthorUserId = authorUserId,
                EmailBody = template.Body,
                EmailSubject = template.Subject,
                FromContact = new MpContact { ContactId = childcareRequestEmail.ChildcareContactId, EmailAddress = childcareRequestEmail.ChildcareContactEmail},
                ReplyToContact = new MpContact { ContactId = childcareRequestEmail.ChildcareContactId, EmailAddress = childcareRequestEmail.ChildcareContactEmail },
                ToContacts = toContactsList,
                MergeData = mergeData
            };

            try
            {
                _communicationService.SendMessage(communication);
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("Send Childcare request approval notification email failed"), ex);
            }

        }

        public void SendChildcareRequestNotification( MpChildcareRequestEmail request)
        {
            var templateId = _configurationWrapper.GetConfigIntValue("ChildcareRequestNotificationTemplate");
            var authorUserId = _configurationWrapper.GetConfigIntValue("DefaultUserAuthorId");          
            var template = _communicationService.GetTemplate(templateId);           

            var mergeData = new Dictionary<string, object>
            {
                {"Requester", request.Requester},
                {"Nickname", request.RequesterNickname },
                {"LastName", request.RequesterLastName },
                {"Group", request.GroupName },
                {"Site", request.CongregationName },
                {"StartDate", (request.StartDate).ToShortDateString() },
                {"EndDate", (request.EndDate).ToShortDateString() },
                {"ChildcareSession", request.ChildcareSession },
                {"RequestId", request.RequestId },
                {"Base_Url", _configurationWrapper.GetConfigValue("BaseMPUrl")}
            };

            var communication = new MpCommunication
             {
                AuthorUserId = authorUserId,
                EmailBody = template.Body,
                EmailSubject = template.Subject,
                FromContact = new MpContact {ContactId = request.RequesterId, EmailAddress = request.RequesterEmail},
                ReplyToContact = new MpContact { ContactId = request.RequesterId, EmailAddress = request.RequesterEmail},
                ToContacts = new List<MpContact> {new MpContact {ContactId = request.ChildcareContactId, EmailAddress = request.ChildcareContactEmail } },
                MergeData = mergeData
             };

            try
            {
                _communicationService.SendMessage(communication);
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("Send Childcare request notification email failed"), ex);
            }


        }

        private void SendConfirmation(int childcareEventId, Participant participant, IEnumerable<int> kids )
        {
            var templateId = _configurationWrapper.GetConfigIntValue("ChildcareConfirmationTemplate");
            var authorUserId = _configurationWrapper.GetConfigIntValue("DefaultUserAuthorId");
            var template = _communicationService.GetTemplate(templateId);
            var fromContact = _contactService.GetContactById(_configurationWrapper.GetConfigIntValue("DefaultContactEmailId"));
            const int domainId = 1;

            var childEvent = _eventService.GetEvent(childcareEventId);

            if (childEvent.ParentEventId == null)
            {
                throw new ApplicationException("SendConfirmation: Parent Event Not Found.");
            }
            var parentEventId = (int) childEvent.ParentEventId;

            var mergeData = SetConfirmationMergeData(parentEventId, kids);
            var replyToContact = ReplyToContact(childEvent);

            var communication = FormatCommunication(authorUserId, domainId, template, fromContact, replyToContact, participant.ContactId, participant.EmailAddress, mergeData);
            try
            {
                _communicationService.SendMessage(communication);
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("Send Childcare Confirmation email failed. Participant: {0}, Event: {1}", participant.ParticipantId, childcareEventId), ex);
            }
        }

        private Dictionary<string, object> SetConfirmationMergeData(int parentEventId, IEnumerable<int> kids)
        {
            var parentEvent = _eventService.GetEvent(parentEventId);
            var kidList = kids.Select(kid => _contactService.GetContactByParticipantId(kid)).Select(contact => contact.First_Name + " " + contact.Last_Name).ToList();

            var html = new HtmlElement("ul");
            var elements = kidList.Select(kid => new HtmlElement("li", kid));
            foreach (var htmlElement in elements)
            {
                html.Append(htmlElement);
            }
            var mergeData = new Dictionary<string, object>
            {
                {"EventTitle", parentEvent.EventTitle},
                {"EventStartDate", parentEvent.EventStartDate.ToString("g")},
                {"ChildNames", html.Build()}
            };
            return mergeData;
        }

        public int SchoolGrade(int graduationYear)
        {
            if (graduationYear == 0)
            {
                return 0;
            }
            var today = _dateTimeWrapper.Today;
            var todayMonth = today.Month;
            var yearForCalc = today.Year;
            if (todayMonth > 7)
            {
                yearForCalc = today.Year + 1;
            }

            var grade = 12 - (graduationYear - yearForCalc);
            if (grade <= 12 && grade >= 0)
            {
                return grade;
            }
            return 0;
        }

        public void SendRequestForRsvp()
        {
            var daysBeforeEvent = _configurationWrapper.GetConfigIntValue("NumberOfDaysBeforeEventToSend");
            var templateId = _configurationWrapper.GetConfigIntValue("ChildcareRequestTemplate");
            var authorUserId = _configurationWrapper.GetConfigIntValue("DefaultUserAuthorId"); ;
            var template = _communicationService.GetTemplate(templateId);
            var fromContact = _contactService.GetContactById(_configurationWrapper.GetConfigIntValue("DefaultContactEmailId"));
            const int domainId = 1;
            var token = _apiUserService.GetToken();

            var participants = _eventParticipantService.GetChildCareParticipants(daysBeforeEvent);
            foreach (var participant in participants)
            {
                var childEvent = _crdsEventService.GetChildcareEvent(participant.EventId);
                var childcareParticipants = _crdsEventService.EventParticpants(childEvent.EventId, token);
                var mine = _crdsEventService.MyChildrenParticipants(participant.ContactId, childcareParticipants, token);

                if (mine!=null && mine.Any())
                {
                    // i have kids already signed up for childcare!
                    continue;
                }
                var mergeData = SetMergeData(participant.GroupName, participant.EventStartDateTime, participant.EventId);
                var replyToContact = ReplyToContact(childEvent);
                var communication = FormatCommunication(authorUserId,
                                                        domainId,
                                                        template,
                                                        fromContact,
                                                        replyToContact,
                                                        participant.ContactId,
                                                        participant.ParticipantEmail,
                                                        mergeData);
                try
                {
                    _communicationService.SendMessage(communication);
                }
                catch (Exception ex)
                {
                    LogError(participant, ex);
                }
            }
        }

        private static MyContact ReplyToContact(MpEvent childEvent)
        {
            var contact = childEvent.PrimaryContact;
            var replyToContact = new MyContact
            {
                Contact_ID = contact.ContactId,
                Email_Address = contact.EmailAddress
            };
            return replyToContact;
        }

        private static MpCommunication FormatCommunication(int authorUserId,
                                                         int domainId,
                                                         MpMessageTemplate template,
                                                         MyContact fromContact,
                                                         MyContact replyToContact,
                                                         int participantContactId,
                                                         string participantEmail,
                                                         Dictionary<string, object> mergeData)
        {
            var communication = new MpCommunication
            {
                AuthorUserId = authorUserId,
                DomainId = domainId,
                EmailBody = template.Body,
                EmailSubject = template.Subject,
                FromContact = new MpContact {ContactId = fromContact.Contact_ID, EmailAddress = fromContact.Email_Address},
                ReplyToContact = new MpContact {ContactId = replyToContact.Contact_ID, EmailAddress = replyToContact.Email_Address},
                ToContacts = new List<MpContact> {new MpContact {ContactId = participantContactId, EmailAddress = participantEmail}},
                MergeData = mergeData
            };
            return communication;
        }

        private void LogError(MpEventParticipant participant, Exception ex)
        {
            var participantId = participant.ParticipantId;
            var groupId = participant.GroupId;
            var eventId = participant.EventId;
            _logger.Error(string.Format("Send Childcare RSVP email failed. Participant: {0}, Group: {1}, Event: {2}", participantId, groupId, eventId), ex);
        }

        private Dictionary<string, object> SetMergeData(string groupName, DateTime eventStartDateTime, int eventId)
        {
            var mergeData = new Dictionary<string, object>
            {
                {"GroupName", groupName},
                {"EventStartDate", eventStartDateTime.ToString("g")},
                {"EventId", eventId},
                {"BaseUrl", _configurationWrapper.GetConfigValue("BaseUrl")}
            };
            return mergeData;
        }
        private static HtmlElement BuildParagraph(string label, string value)
        {
            var els = new List<HtmlElement>()
            {
                new HtmlElement("strong", label),
                new HtmlElement("span", value)
            }
                ;
            return new HtmlElement("p", els);
        }
        private Dictionary<string, string> Styles()
        {
            return new Dictionary<string, string>()
            {
                {"style", "border-spacing: 0; border-collapse: collapse; vertical-align: top; text-align: left; width: 100%; padding: 0; border:none; border-color:#ffffff;font-size: small; font-weight: normal;" }
            };
        }

    } 
}
