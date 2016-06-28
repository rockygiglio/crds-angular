using MinistryPlatform.Translation.Models;

namespace MinistryPlatform.Translation.Repositories.Interfaces
{
    public interface ICongregationRepository
    {
        MpCongregation GetCongregationById(int id);

    }
}