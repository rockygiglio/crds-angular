using MinistryPlatform.Translation.Models.Attributes;
using Newtonsoft.Json;

namespace MinistryPlatform.Translation.Models
{
    [MpRestApiTable(Name = "MpRecordID")]
    public class MpRecordID
    {
        [JsonProperty(PropertyName = "Record_ID")]
        public int RecordId { get; set; }

    }
}
