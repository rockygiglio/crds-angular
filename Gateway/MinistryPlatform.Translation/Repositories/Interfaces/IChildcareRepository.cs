using System.Collections.Generic;
using MinistryPlatform.Translation.Models.Childcare;

namespace MinistryPlatform.Translation.Repositories.Interfaces
{
    public interface IChildcareRepository
    {
        List<ChildcareDashboard> GetChildcareDashboard(int contactId);
        bool IsChildRsvpd(int contactId, int groupId, string token);
        List<MPChildcareEmail> GetChildcareReminderEmails(string token);
    }
}
