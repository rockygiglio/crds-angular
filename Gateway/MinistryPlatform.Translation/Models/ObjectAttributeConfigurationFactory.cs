using System.Configuration;

namespace MinistryPlatform.Translation.Models
{
    public static class ObjectAttributeConfigurationFactory
    {
        public static MpObjectAttributeConfiguration Contact()
        {
            return new MpObjectAttributeConfiguration()
            {
                SubPage = int.Parse(ConfigurationManager.AppSettings["ContactAttributesSubPage"]),
                SelectedSubPage = int.Parse(ConfigurationManager.AppSettings["SelectedContactAttributes"]),
                TableName = "Contact"
            };
        }
        public static MpObjectAttributeConfiguration MyContact()
        {
            return new MpObjectAttributeConfiguration()
            {
                SubPage = int.Parse(ConfigurationManager.AppSettings["MyContactAttributesSubPage"]),
                SelectedSubPage = int.Parse(ConfigurationManager.AppSettings["MyContactCurrentAttributesSubPageView"]),
                TableName = "Contact"
            };
        }

        public static MpObjectAttributeConfiguration Group()
        {
            return new MpObjectAttributeConfiguration()
            {
                SubPage = int.Parse(ConfigurationManager.AppSettings["GroupAttributesSubPage"]),
                SelectedSubPage = int.Parse(ConfigurationManager.AppSettings["SelectedGroupAttributesAttributes"]),
                TableName = "Group"
            };
        }

        public static MpObjectAttributeConfiguration GroupParticipant()
        {
            return new MpObjectAttributeConfiguration()
            {
                SubPage = int.Parse(ConfigurationManager.AppSettings["GroupParticipantAttributesSubPage"]),
                SelectedSubPage = int.Parse(ConfigurationManager.AppSettings["SelectedGroupPartAttributes"]),
                TableName = "Group_Participant"
            };
        }
    }
}