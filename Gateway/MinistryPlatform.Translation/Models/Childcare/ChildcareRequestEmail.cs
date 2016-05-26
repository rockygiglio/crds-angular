using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinistryPlatform.Translation.Models.Childcare
{
    public class ChildcareRequestEmail
    {

        public int RequesterId { get; set; }
        public String RequesterEmail { get; set; }
        public String MinistryName { get; set; }
        public String GroupName{ get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Frequency { get; set; }
        public string ChildcareSession { get; set; }
        public int EstimatedChildren { get; set; }
        public int CongregationId { get; set; }
        public string Requester { get; set; }
       

    }
}
    
