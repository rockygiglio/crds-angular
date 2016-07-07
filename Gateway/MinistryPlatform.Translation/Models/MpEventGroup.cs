namespace MinistryPlatform.Translation.Models
{
    public class MpEventGroup
    {
        public int EventGroupId { get; set; }
        public int EventId { get; set; }
        public int GroupId { get; set; }
        public int? RoomId { get; set; }
        public int DomainId { get; set; }
        public bool Closed { get; set; }
        public int? EventRoomId { get; set; }
        public bool Created { get; set; }
        public int GroupTypeId { get; set; }
    }
}
