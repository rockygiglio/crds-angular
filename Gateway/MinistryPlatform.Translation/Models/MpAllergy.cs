using MinistryPlatform.Translation.Models.Attributes;
using Newtonsoft.Json;

namespace MinistryPlatform.Translation.Models
{
    [MpRestApiTable(Name = "cr_Allergy")]
    public class MpAllergy
    {
        [JsonProperty(PropertyName = "Allergy_ID")]
        public int AllergyID { get; set; }

        [JsonProperty(PropertyName = "Allergy_Type_ID")]
        public int AllergyTypeID { get; set; }

        [JsonProperty(PropertyName = "Description")]
        public string AllergyDescription { get; set; }
    }
}