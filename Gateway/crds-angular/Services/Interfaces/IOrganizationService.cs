using System.Collections.Generic;
using crds_angular.Models.Crossroads.GoVolunteer;

namespace crds_angular.Services.Interfaces
{
    public interface IOrganizationService
    {
        Organization GetOrganizationByName(string name);
        List<Organization> GetOrganizations();
    }
}
