namespace MinistryPlatform.Translation.Models.GoCincinnati
{
    public class MpGroupConnector
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PrimaryRegistrationID { get; set; }
        public string OrganizationName { get; set; }
        public string PreferredLaunchSite { get; set; }
        public int ProjectMaximumVolunteers { get; set; }
        public int ProjectMinimumAge { get; set; }
        public string ProjectName { get; set; }
        public string ProjectType { get; set; }
        public int VolunteerCount { get; set; }
    }
}