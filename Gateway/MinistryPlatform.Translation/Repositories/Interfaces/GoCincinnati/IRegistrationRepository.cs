using MinistryPlatform.Translation.Models.GoCincinnati;

namespace MinistryPlatform.Translation.Repositories.Interfaces.GoCincinnati
{
    public interface IRegistrationRepository
    {
        int CreateRegistration(MpRegistration registration);
        int AddAgeGroup(int registrationId, int attributeId, int count);
        int AddPrepWork(int registrationId, int attributeId, bool spouse);
        int AddEquipment(int registrationId, int equipmentId, string notes);
        int AddProjectPreferences(int registrationId, int projectType, int priority);
    }
}