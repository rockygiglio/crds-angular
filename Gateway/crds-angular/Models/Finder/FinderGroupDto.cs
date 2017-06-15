using System.Collections.Generic;
using crds_angular.Models.Crossroads.Groups;

namespace crds_angular.Models.Finder
{
    public class FinderGroupDto : GroupDTO
    {
        public List<string> GroupCategoriesList { get; set; }
        public List<string> GroupAgesRangeList { get; set; }
        public string GroupType { get; set; }
        public bool VirtualGroup { get; set; }
        public string PrimaryContactFirstName { get; set; }
        public string PrimaryContactLastName { get; set; }
        public string PrimaryContactCongregation { get; set; }
    }
}