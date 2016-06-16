using System.Collections.Generic;
using MinistryPlatform.Translation.Models;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IAddressService
    {
        int Create(MpAddress address);
        List<MpAddress> FindMatches(MpAddress address);
    }
}