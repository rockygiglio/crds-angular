using System.Collections.Generic;
using MinistryPlatform.Translation.Models;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IDestinationService
    {
        List<MpTripDocuments> DocumentsForDestination(int destinationId);
    }
}