using System;
using Newtonsoft.Json;

namespace crds_angular.Models.AwsCloudsearch
{
    public class AwsConnectDto
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
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public int? HostStatus { get; set; }
        public int? GroupId { get; set; }
        public string GroupName { get; set; }
        public string GroupDescription { get; set; }
        public int? PrimaryContactId { get; set; }
        public string PrimaryContactEmail { get; set; }
        public int? ParticipantCount { get; set; }
        public int? GroupTypeId { get; set; }
        public int? HouseholdId { get; set; }
        public int PinType { get; set; }
    }
}