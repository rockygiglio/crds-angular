using MinistryPlatform.Translation.Models;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface ICongregationRepository
    {
        MpCongregation GetCongregationById(int id);

    }
}