using Participant = MinistryPlatform.Translation.Models.MpParticipant;

namespace crds_angular.Models.Crossroads.Groups
{
    public class MyGroup
    {
        public GroupDTO Group { get; set; }
        public Participant Me { get; set; }
    }
}