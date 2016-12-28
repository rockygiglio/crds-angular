using System.Collections.Generic;
using Newtonsoft.Json;
using Swashbuckle.Swagger;

namespace crds_angular.Models.Crossroads.Camp
{
    public class MedicalInfoDTO
    {
        [JsonProperty(PropertyName = "contactId")]
        public int ContactId { get; set; }

        [JsonProperty(PropertyName = "medicalInformationId")]
        public int MedicalInformationId { get; set; }

        [JsonProperty(PropertyName = "insuranceCompany")]
        public string InsuranceCompany { get; set; }

        [JsonProperty(PropertyName = "policyHolder")]
        public string PolicyHolder { get; set; }

        [JsonProperty(PropertyName = "physicianName")]
        public string PhysicianName { get; set; }

        [JsonProperty(PropertyName = "physicianPhone")]
        public string PhysicianPhone { get; set; }

        [JsonProperty(PropertyName = "showAllergies")]
        public bool ShowAllergies { get; set; }

        [JsonProperty(PropertyName = "allergies")]
        public List<Allergy> Allergies  { get; set; }

        [JsonProperty(PropertyName = "showMedications")]
        public bool ShowMedications { get; set; }

        [JsonProperty(PropertyName = "medications")]
        public List<Medication> Medications { get; set; }
    }

    public class Allergy
    {
        [JsonProperty(PropertyName = "allergyType")]
        public string AllergyType { get; set; }

        [JsonProperty(PropertyName = "allergyTypeId")]
        public int AllergyTypeId { get; set; }

        [JsonProperty(PropertyName = "allergyDescription")]
        public string AllergyDescription { get; set; }

        [JsonProperty(PropertyName = "allergyId")]
        public int AllergyId { get; set; }

        [JsonProperty(PropertyName = "medicalInformationAllergyId")]
        public int MedicalInformationAllergyId { get; set; }
    }

    public class Medication
    {
        [JsonProperty(PropertyName = "medicationName")]
        public string MedicationName { get; set; }

        [JsonProperty(PropertyName = "medicationType")]
        public int MedicationTypeId { get; set; }

        [JsonProperty(PropertyName = "timeOfDay")]
        public string TimesOfDay { get; set; }

        [JsonProperty(PropertyName = "dosage")]
        public string Dosage { get; set; }


    }
   
}