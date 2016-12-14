namespace MinistryPlatform.Translation.Models.Rules
{
    public class MPRegistrationRule : MPRuleBase
    {
        public int MinimumRegistrants { get; set; }
        public int MaximumRegistrants { get; set; }
    }
}
