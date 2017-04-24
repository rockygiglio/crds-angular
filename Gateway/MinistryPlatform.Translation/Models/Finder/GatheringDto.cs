using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Crossroads.Web.Common.MinistryPlatform;
using MinistryPlatform.Translation.Extensions;
using MinistryPlatform.Translation.Models.DTO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MinistryPlatform.Translation.Models.Finder
{
    [MpRestApiTable(Name = "Groups")]
    public class GatheringDto : MpBaseDto
    {
        [JsonProperty("Group_ID")]
        public int GroupId { get; set; }

        [JsonProperty("Group_Name")]
        public string Name { get; set; }

        [JsonProperty("Group_Type_ID")]
        public int GroupType { get; set; }

        [JsonProperty("Primary_Contact")]
        public int PrimaryContact { get; set; }

        [JsonProperty("Description")]
        public string GroupDescription { get; set; }

        [JsonProperty("Group_Role_ID")]
        public int GroupRoleId { get; set; }

        [JsonProperty("Congregation_Name")]
        public string Congregation { get; set; }

        [JsonProperty("Ministry_ID")]
        public int MinistryId { get; set; }

        [JsonProperty("Congregation_ID")]
        public int CongregationId { get; set; }

        [JsonProperty("Start_Date")]
        public DateTime StartDate { get; set; }

        [JsonProperty("End_Date")]
        public DateTime? EndDate { get; set; }

        [JsonProperty("Available_Online")]
        public bool? AvailableOnline { get; set; }

        [JsonProperty("Meeting_Time")]
        public string MeetingTime { get; set; }

        [JsonProperty("Meeting_Day_ID")]
        public int? MeetingDayId { get; set; }

        [JsonProperty("Meeting_Day")]
        public string MeetingDay { get; set; }

        [JsonProperty("Meeting_Frequency")]
        public string MeetingFrequency { get; set; }

        [JsonProperty("Meeting_Frequency_ID")]
        public int? MeetingFrequencyID { get; set; }

        [JsonProperty("Kids_Welcome")]
        public bool? KidsWelcome { get; set; }

        [JsonProperty("Target_Size")]
        public int? TargetSize { get; set; }

        [JsonProperty("Group_Is_Full")]
        public bool Full { get; set; }

        [JsonProperty("Enable_Waiting_List")]
        public bool? WaitList { get; set; }

        [JsonProperty("Child_Care_Available")]
        public bool ChildCareAvailable { get; set; }

        [JsonProperty("Minimum_Age")]
        public int MinimumAge { get; set; }

        [JsonProperty("Maximum_Age")]
        public int? MaximumAge { get; set; }

        [JsonProperty("Remaining_Capacity")]
        public int? RemainingCapacity { get; set; }


        [JsonProperty("Minimum_Participants")]
        public int? MinimumParticipants { get; set; }

        public int WaitListGroupId { get; set; }

        [JsonProperty("Offsite_Meeting_Address")]
        public MpAddress Address { get; set; }

        protected override void ProcessUnmappedData(IDictionary<string, JToken> unmappedData, StreamingContext context)
        {
            if (Address == null) Address = new MpAddress();
            Address.Address_ID = unmappedData.GetUnmappedDataField<int>("Address_ID");
            Address.Address_Line_1 = unmappedData.GetUnmappedDataField<string>("Address_Line_1");
            Address.Address_Line_2 = unmappedData.GetUnmappedDataField<string>("Address_Line_2");
            Address.City = unmappedData.GetUnmappedDataField<string>("City");
            Address.State = unmappedData.GetUnmappedDataField<string>("State/Region");
            Address.Postal_Code = unmappedData.GetUnmappedDataField<string>("Postal_Code");
            Address.Foreign_Country = unmappedData.GetUnmappedDataField<string>("Foreign_Country");
            Address.Longitude = Convert.ToDouble(unmappedData.GetUnmappedDataField<string>("Longitude"));
            Address.Latitude = Convert.ToDouble(unmappedData.GetUnmappedDataField<string>("Latitude"));
        }
    }
}
