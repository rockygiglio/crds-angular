using System.Collections.Generic;
using MinistryPlatform.Translation.Models;

namespace MinistryPlatform.Translation.Repositories.Interfaces
{
    public interface IDestinationRepository
    {
        List<MpTripDocuments> DocumentsForDestination(int destinationId);
    }
}