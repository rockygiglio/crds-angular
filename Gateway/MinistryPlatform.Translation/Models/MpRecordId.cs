using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinistryPlatform.Translation.Models.Attributes;
using Newtonsoft.Json;

namespace MinistryPlatform.Translation.Models
{
    namespace MinistryPlatform.Translation.Models
    {
        [MpRestApiTable(Name = "MpRecordID")]
        public class MpRecordID
        {
            [JsonProperty(PropertyName = "RecordID")]
            public int RecordId { get; set; }

        }
    }
}
