using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinistryPlatform.Models
{
    public class GroupSearchAttributes
    {
        public int TypeId { get; set; }
        public int GoalId { get; set; }
        public int KidsId { get; set; }
        public int MeetingRangeId { get; set; }
        public bool HasDog { get; set; }
        public bool HasCat { get; set; }

    }
}
