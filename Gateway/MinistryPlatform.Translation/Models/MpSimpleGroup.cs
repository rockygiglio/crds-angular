using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinistryPlatform.Translation.Models.Attributes;

namespace MinistryPlatform.Translation.Models
{
    [MpRestApiTable(Name = "Groups")]
    public class MpSimpleGroup
    {
        public int GroupId { get; set; }
        public string Name { get; set; }

    }
}
