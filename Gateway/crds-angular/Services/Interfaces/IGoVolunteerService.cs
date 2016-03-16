using crds_angular.Models.Crossroads.GoVolunteer;

namespace crds_angular.Services.Interfaces
{
    public interface IGoVolunteerService
    {
        bool CreateRegistration(Registration registration, string token);
    }
}