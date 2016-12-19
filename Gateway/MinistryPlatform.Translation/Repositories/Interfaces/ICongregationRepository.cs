using Crossroads.Utilities.FunctionalHelpers;
using MinistryPlatform.Translation.Models;

namespace MinistryPlatform.Translation.Repositories.Interfaces
{
    public interface ICongregationRepository
    {
        MpCongregation GetCongregationById(int id);
        Result<MpCongregation> GetCongregationByName(string congregationName , string token );
    }
}