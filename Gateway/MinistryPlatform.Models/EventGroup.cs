using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinistryPlatform.Models
{
    public class EventGroup
    {
        public int Event_Group_ID { get; set; }
        public int Event_ID { get; set; }
        public int Group_ID { get; set; }
        public int Room_ID { get; set; }
        public int Domain_ID { get; set; }
        public bool Closed { get; set; }
        public int Event_Room_ID { get; set; }
    }
}
