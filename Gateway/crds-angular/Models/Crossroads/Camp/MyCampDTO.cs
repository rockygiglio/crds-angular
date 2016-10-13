using System;

namespace crds_angular.Models.Crossroads.Camp
{
    public class MyCampDTO
    {
        public int CamperContactId { get; set; }
        public string CamperNickName { get; set; }
        public string CamperLastName { get; set; }
        public string CampName { get; set; }
        public DateTime CampStartDate { get; set; }
        public DateTime CampEndDate { get; set; }
    }
}