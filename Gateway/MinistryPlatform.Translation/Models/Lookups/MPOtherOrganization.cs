namespace MinistryPlatform.Translation.Models.Lookups
{
    public class MpOtherOrganization
    {
        public MpOtherOrganization(int id, string name)
        {
            OtherOrganizationID = id;
            OtherOrganization = name;
        }

        public int OtherOrganizationID { get; set; }
        public string OtherOrganization { get; set;  }
    }
}
