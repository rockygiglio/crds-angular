using System;
using System.Collections.Generic;
using MinistryPlatform.Translation.Models;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface ICommunicationService
    {
        MpCommunicationPreferences GetPreferences(String token, int userId);
        bool SetEmailSMSPreferences(String token, Dictionary<string, object> prefs);
        bool SetMailPreferences(string token, Dictionary<string, object> prefs);
        int SendMessage(MpCommunication communication, bool isDraft = false);
        MpMessageTemplate GetTemplate(int templateId);

        MpCommunication GetTemplateAsCommunication(int templateId,
                                                 int fromContactId,
                                                 string fromEmailAddress,
                                                 int replyContactId,
                                                 string replyEmailAddress,
                                                 int toContactId,
                                                 string toEmailAddress,
                                                 Dictionary<string, object> mergeData);

        string ParseTemplateBody(string templateBody, Dictionary<string, object> record);
        int GetUserIdFromContactId(string token, int contactId);
        int GetUserIdFromContactId(int contactId);
        string GetEmailFromContactId(int contactId);
    }
}