using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace crds_angular.Models.Crossroads.Camp
{
    public class CampFamilyMember
    {
        public int ContactId { get; set; }

        public string PreferredName { get; set; }

        public string LastName { get; set; }

        public bool IsEligible { get; set; }
    }
}