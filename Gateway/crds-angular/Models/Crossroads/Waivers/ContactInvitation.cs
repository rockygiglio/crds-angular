using MinistryPlatform.Translation.Models;

namespace crds_angular.Models.Crossroads.Waivers
{
    public class ContactInvitation
    {
        public MpInvitation Invitation { get; set; }
        public MpMyContact Contact { get; set; }
    }
}