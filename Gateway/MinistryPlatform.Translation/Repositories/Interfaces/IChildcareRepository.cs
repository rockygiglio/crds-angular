using System;
using System.Collections.Generic;
using MinistryPlatform.Translation.Models.Childcare;

namespace MinistryPlatform.Translation.Repositories.Interfaces
{
    public interface IChildcareRepository
    {
        List<object> GetChildcareDashboard(int contactId);
    }
}
