using System.Collections.Generic;

namespace MinistryPlatform.Translation.Models
{
    public class MpFormResponse
    {
        public int FormId { get; set; }
        public int ContactId { get; set; }
        public int? OpportunityId { get; set; }
        public int? OpportunityResponseId { get; set; }
        public int? PledgeCampaignId { get; set; }
        public List<MpFormAnswer> FormAnswers { get; set; }

        public MpFormResponse()
        {
            FormAnswers = new List<MpFormAnswer>();
        }
    }

    public class MpFormAnswer
    {
        public int FieldId { get; set; }
        public int FormResponseId { get; set; }
        public string Response { get; set; }
        public int? OpportunityResponseId { get; set; }
    }
}