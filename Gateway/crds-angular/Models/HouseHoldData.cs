using System.Collections.Generic;
using MinistryPlatform.Translation.Models;

namespace crds_angular.Models
{
    public class HouseHoldData
    {
        public List<MpHouseholdMember> AllMembers { get; set; }
        public List<MpHouseholdMember> HeadsOfHousehold { get; set; }

    }
}
