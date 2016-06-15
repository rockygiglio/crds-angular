using System.Collections.Generic;
using MinistryPlatform.Translation.Models;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IAddressService
    {
        int Create(Address address);
        List<Address> FindMatches(Address address);
    }
}