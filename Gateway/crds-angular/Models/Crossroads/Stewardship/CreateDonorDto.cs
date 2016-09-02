namespace crds_angular.Models.Crossroads.Stewardship
{
    public class CreateDonorDTO
    {
        public string stripe_token_id  { get; set; }
        public string email_address { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
    }
}