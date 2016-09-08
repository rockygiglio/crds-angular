
using System.Collections.Generic;
using System.Text.RegularExpressions;
using crds_angular.Models.Crossroads;
using crds_angular.Services.Interfaces;
using crds_angular.Util;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Repositories.Interfaces;
using Quartz;

namespace crds_angular.Processors
{
    public class SendTextMessageJob : IJob
    {
        private readonly ITextCommunicationService _textCommunicationService;
        private readonly ICommunicationRepository _communicationService;

        public SendTextMessageJob(ITextCommunicationService textCommunicationService, ICommunicationRepository communicationService)
        {
            _textCommunicationService = textCommunicationService;
            _communicationService = communicationService;
        }

        public void Execute(IJobExecutionContext context)
        {
            JobDataMap dataMap = context.JobDetail.JobDataMap;
            MpMessageTemplate template = _communicationService.GetTemplate(dataMap.GetIntValue("TemplateId"));
            string htmlBody =_communicationService.ParseTemplateBody(template.Body, (Dictionary<string, object>) dataMap["MergeData"]);
            string textBody = HtmlHelper.StripHTML(htmlBody);
            _textCommunicationService.SendTextMessage(dataMap.GetString("ToPhoneNumber"), textBody);
        }
    }
}