using System;
using System.Collections.Generic;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Models;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IOrganizationService
    {
        MPOrganization GetOrganization(String name, string token);
        List<MPOrganization> GetOrganizations(string token);
        List<Location> GetLocationsForOrganization(int organizationId, string token);
    }
}
