using System;

namespace MinistryPlatform.Translation.Models.Finder
{
    public class MpConnectAws
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SiteName { get; set; }
        public string EmailAddress { get; set; }
        public int? ContactId { get; set; }
        public int? ParticipantId { get; set; }
        public int AddressId { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int? GroupId { get; set; }
        public string GroupName { get; set; }
        public string GroupDescription { get; set; }
        public string GroupTypeName { get; set; }
        public int MinistryId { get; set; }
        public int CongregationId { get; set; }
        public string CongregationName { get; set; }
        public int PrimaryContactId { get; set; }
        public string PrimaryContactEmail { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int AvailableOnline { get; set; }
        public int GroupFullIndicator { get; set; }
        public int ChildcareIndicator { get; set; }
        public int MeetingDayId { get; set; }
        public string MeetingTime { get; set; }
        public int MeetingFrequencyId { get; set; }
        public int TargetSize { get; set; }
        public int KidsWelcome { get; set; }
        public int[] ParticipantList { get; set; }
        public int? GroupTypeId { get; set; }
        public int? HouseholdId { get; set; }
        public int PinType { get; set; }
    }
}
