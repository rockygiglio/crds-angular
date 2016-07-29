using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using crds_angular.Models.Crossroads.Stewardship;

namespace crds_angular.Models.Crossroads.Trip
{
    public class CreateTripParticipantDto
    {
        public int PledgeCampaignId { get; set; }
        public  int ContactId { get; set; }
    }
}
