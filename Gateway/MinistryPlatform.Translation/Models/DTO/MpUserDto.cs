using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinistryPlatform.Translation.Models.DTO
{
    public class MpUserDto
    {
        public bool? Can_Impersonate { get; set; }
        public string User_GUID { get; set; }
        public string User_Name { get; set; }
        public string User_Email { get; set; }
        public int User_ID { get; set; }
        public string Display_Name { get; set; }
    }
}
