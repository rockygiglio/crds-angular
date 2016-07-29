using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinistryPlatform.Translation.Models
{
    public class MpTripRecord
    {
        public int CampaignDestinationId { get; set; }
        public decimal CampaignFundRaisingGoal { get; set; }
        public int GroupId { get; set; }
    }
}
