using System.Diagnostics.Eventing.Reader;

namespace MinistryPlatform.Models
{
    public class EventRoom
    {
        public int Event_Room_ID { get; set; }
        public int Event_ID { get; set; }
        public int Room_ID { get; set; }
        public int Room_Layout_ID { get; set; }
        public int Chairs { get; set; }
        public int Tables { get; set; }
        public string Notes { get; set; }
        public int Domain_ID { get; set; }
        public bool _Approved { get; set; }
        public bool Cancelled { get; set; }
        public bool Hidden { get; set; }
        public int Capacity { get; set; }
        public string Label { get; set; }
        public EventBookmark Allow_Checkin { get; set; }
    }
}
