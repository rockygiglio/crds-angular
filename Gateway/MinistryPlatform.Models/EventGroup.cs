using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinistryPlatform.Models
{
    public class EventGroup
    {
        public int EventGroupId { get; set; }
        public int EventId { get; set; }
        public int GroupId { get; set; }
        public int? RoomId { get; set; }
        public int DomainId { get; set; }
        public bool Closed { get; set; }
        public int? EventRoomId { get; set; }
        public bool Created { get; set; }
    }
}
