using System.Configuration;

namespace MinistryPlatform.Models
{
    public static class ObjectAttributeConfigurationFactory
    {
        public static ObjectAttributeConfiguration Contact()
        {
            return new ObjectAttributeConfiguration()
            {
                SubPage = int.Parse(ConfigurationManager.AppSettings["ContactAttributesSubPage"]),
                SelectedSubPage = int.Parse(ConfigurationManager.AppSettings["SelectedContactAttributes"]),
                TableName = "Contact"
            };
        }
        public static ObjectAttributeConfiguration MyContact()
        {
            return new ObjectAttributeConfiguration()
            {
                SubPage = int.Parse(ConfigurationManager.AppSettings["MyContactAttributesSubPage"]),
                SelectedSubPage = int.Parse(ConfigurationManager.AppSettings["MyContactCurrentAttributesSubPageView"]),
                TableName = "Contact"
            };
        }

        public static ObjectAttributeConfiguration Group()
        {
            return new ObjectAttributeConfiguration()
            {
                SubPage = int.Parse(ConfigurationManager.AppSettings["GroupAttributesSubPage"]),
                SelectedSubPage = int.Parse(ConfigurationManager.AppSettings["SelectedGroupAttributesAttributes"]),
                TableName = "Group"
            };
        }

        public static ObjectAttributeConfiguration GroupParticipant()
        {
            return new ObjectAttributeConfiguration()
            {
                SubPage = int.Parse(ConfigurationManager.AppSettings["GroupParticipantAttributesSubPage"]),
                SelectedSubPage = int.Parse(ConfigurationManager.AppSettings["SelectedGroupPartAttributes"]),
                TableName = "Group_Participant"
            };
        }
    }
}