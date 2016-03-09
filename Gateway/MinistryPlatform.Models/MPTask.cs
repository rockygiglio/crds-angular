using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinistryPlatform.Models
{
    public class MPTask
    {
        public int Task_ID { get; set; }
        public string Title { get; set; }
        public int Author_User_ID { get; set; }
        public int Assigned_User_ID { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool Completed { get; set; }
        public string Description { get; set; }
        public int Domain_ID { get; set; }
        public int? Record_ID { get; set; } // MP _
        public int? Page_ID { get; set; } // MP _
        public int? Process_Submission_ID { get; set; } // MP _
        public int? Process_Step_ID { get; set; } // MP _
        public bool Rejected { get; set; } // MP _
        public bool Escalated { get; set; } // MP _
        public string Record_Description { get; set; } // MP _
    }
}
