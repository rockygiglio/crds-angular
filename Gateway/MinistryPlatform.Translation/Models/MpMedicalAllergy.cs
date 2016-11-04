using System.Collections.Generic;
using MinistryPlatform.Translation.Models.Attributes;
using Newtonsoft.Json;

namespace MinistryPlatform.Translation.Models
{
    [MpRestApiTable(Name = "cr_Medical_Information_Allergies")]
    public class MpMedicalAllergy
    {
        [JsonProperty(PropertyName = "Allergy_ID")]
        public List<MpAllergy> Allergy { get; set; }

        [JsonProperty(PropertyName = "Medical_Information_ID")]
        public MpMedicalInformation MedicalInfo { get; set; }

        [JsonProperty(PropertyName = "Medical_Information_Allergy_ID")]
        public int MedicalInfoAllergyID { get; set; }
    }
}