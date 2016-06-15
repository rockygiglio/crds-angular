using System;

namespace MinistryPlatform.Translation.Models
{
    public class MpRelationship
    {
        public int RelationshipID { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime StartDate { get; set; }
        public int RelatedContactID{ get; set; }
    }
}