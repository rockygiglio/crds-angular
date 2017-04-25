using System;
using System.Collections.Generic;
using System.Linq;
using Crossroads.Utilities.Interfaces;
using Crossroads.Web.Common;
using Crossroads.Web.Common.Configuration;
using Crossroads.Web.Common.Security;
using MinistryPlatform.Translation.Exceptions;
using MinistryPlatform.Translation.Extensions;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Repositories.Interfaces;

namespace MinistryPlatform.Translation.Repositories
{
    public class ParticipantRepository : BaseRepository, IParticipantRepository
    {
        private IMinistryPlatformService _ministryPlatformService;

        public ParticipantRepository(IMinistryPlatformService ministryPlatformService, IAuthenticationRepository authenticationService , IConfigurationWrapper configurationWrapper)
            : base(authenticationService, configurationWrapper)
        {
            this._ministryPlatformService = ministryPlatformService;
        }

        public int CreateParticipantRecord(int contactId)
        {
            var token = ApiLogin();
            var pageId = _configurationWrapper.GetConfigIntValue("Participants");

            var participantDictionary = new Dictionary<string, object>();
            participantDictionary["Participant_Type_ID"] = _configurationWrapper.GetConfigIntValue("Participant_Type_Default_ID");
            participantDictionary["Participant_Start_Date"] = DateTime.Now;
            participantDictionary["Contact_Id"] = contactId;

            return _ministryPlatformService.CreateRecord(pageId, participantDictionary, token);
        }

        //Get Participant IDs of a contact
        public MpParticipant GetParticipantRecord(string token)
        {
            var results = _ministryPlatformService.GetRecordsDict("MyParticipantRecords", token);
            Dictionary<string, object> result = null;
            try
            {
                result = results.SingleOrDefault();
            }
            catch (InvalidOperationException ex)
            {
                if (ex.Message == "Sequence contains more than one element")
                {
                    throw new MultipleRecordsException("Multiple Participant records found! Only one participant allowed per Contact.");
                }
            }

            if (result == null)
            {
                return null;
            }
            var participant = new MpParticipant
            {
                ContactId = result.ToInt("Contact_ID"),
                ParticipantId = result.ToInt("dp_RecordID"),
                EmailAddress = result.ToString("Email_Address"),
                PreferredName = result.ToString("Nickname"),
                DisplayName = result.ToString("Display_Name"),
                ApprovedSmallGroupLeader = result.ToBool("Approved_Small_Group_Leader")
            };

            return participant;
        }

        public MpParticipant GetParticipant(int contactId)
        {
            MpParticipant participant;
            try
            {
                var searchStr = contactId.ToString() + ",";
                var records =
                    WithApiLogin<List<Dictionary<string, object>>>(
                        apiToken =>
                            (_ministryPlatformService.GetPageViewRecords("ParticipantByContactId", apiToken, searchStr,
                                "")));
                var record = records.Single();
                participant = new MpParticipant
                {
                    ContactId = record.ToInt("Contact_ID"),
                    ParticipantId = record.ToInt("Participant_ID"),
                    EmailAddress = record.ToString("Email_Address"),
                    PreferredName = record.ToString("Nickname"),
                    DisplayName = record.ToString("Display_Name"),
                    Age = record.ToInt("__Age"),
                    ApprovedSmallGroupLeader = record.ToBool("Approved_Small_Group_Leader"),
                    AttendanceStart = record.ToDate("Attendance_Start_Date")
                };
            }
            catch (Exception ex)
            {
                throw new ApplicationException(
                    string.Format("GetParticipant failed.  Contact Id: {0}", contactId), ex);
            }
            return participant;       
        }

        public void UpdateParticipant(MpParticipant participant)
        {
            var participantDict = new Dictionary<string, object>()
            {
                {"Participant_ID", participant.ParticipantId },
                {"Attendance_Start_Date", participant.AttendanceStart },
                {"Approved_Small_Group", participant.ApprovedSmallGroupLeader },
                {"Group_Leader_Status_ID", participant.GroupLeaderStatus }
            };
            UpdateParticipant(participantDict);
        }
            

        public void UpdateParticipant(Dictionary<string, object> participant)
        {
            var apiToken = ApiLogin();
            try
            {
                _ministryPlatformService.UpdateRecord(_configurationWrapper.GetConfigIntValue("Participants"), participant, apiToken);
            }
            catch (Exception e)
            {
                throw new ApplicationException(
                   string.Format("Unable to update the participant.  Participant Id: {0}", participant["Participant_ID"]), e);
            }

        }

        public List<MpResponse> GetParticipantResponses(int participantId)
        {
            try
            {
                var records =
                    WithApiLogin<List<Dictionary<string, object>>>(
                        apiToken =>
                            (_ministryPlatformService.GetSubpageViewRecords("ParticipantResponsesWithEventId",
                                participantId, apiToken, "", "")));
                return records.Select(viewRecord => new MpResponse
                {
                    Opportunity_ID = viewRecord.ToInt("Opportunity ID"),
                    Participant_ID = viewRecord.ToInt("Participant ID"),
                    Response_Result_ID = viewRecord.ToInt("Response Result ID"),
                    Event_ID = viewRecord.ToInt("Event ID")
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(
                    string.Format("GetParticipantResponses failed.  Participant Id: {0}", participantId), ex);
            }
        }
    }
}