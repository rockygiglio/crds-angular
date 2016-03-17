using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinistryPlatform.Translation.Models.Lookups
{
    public class MPOtherOrganization
    {
        public MPOtherOrganization(int id, string name)
        {
            OtherOrganizationID = id;
            OtherOrganization = name;
        }

        public int OtherOrganizationID { get; set; }
        public string OtherOrganization { get; set;  }
    }
}
