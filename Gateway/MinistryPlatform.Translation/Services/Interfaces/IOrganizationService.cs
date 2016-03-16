using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinistryPlatform.Translation.Models;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IOrganizationService
    {
        MPOrganization GetOrganization(String name, string token);
        List<MPOrganization> GetOrganizations(string token);
    }
}
