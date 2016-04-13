using MinistryPlatform.Translation.Models.GoCincinnati;

namespace MinistryPlatform.Translation.Services.Interfaces.GoCincinnati
{
    public interface IRegistrationService
    {
        int CreateRegistration(Registration registration);
        int AddAgeGroup(int registrationId, int attributeId, int count);
        int AddPrepWork(int registrationId, int attributeId, bool spouse);
        int AddEquipment(int registrationId, int equipmentId, string notes);
        int AddProjectPreferences(int registrationId, int projectType, int priority);
    }
}