using System.Collections.Generic;
using crds_angular.Models.Crossroads.GoVolunteer;

namespace crds_angular.Services.Interfaces
{
    public interface IGoVolunteerService
    {
        CincinnatiRegistration CreateRegistration(CincinnatiRegistration registration, string token);
        AnywhereRegistration CreateAnywhereRegistration(AnywhereRegistration registration, int projectId, string token);

        List<ProjectType> GetProjectTypes();
        List<ChildrenOptions> ChildrenOptions();
        bool SendMail(CincinnatiRegistration registration);

        List<ProjectCity> GetParticipatingCities(int initiativeId);
        Project GetProject(int projectId);
        List<DashboardDatum> GetRegistrationsForProject(int projectId);
    }
}
