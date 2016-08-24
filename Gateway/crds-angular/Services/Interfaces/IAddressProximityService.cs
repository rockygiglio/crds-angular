using System.Collections.Generic;
using crds_angular.Models.Crossroads;
using System.Device.Location;

namespace crds_angular.Services.Interfaces
{
    public interface IAddressProximityService
    {
        List<decimal?> GetProximity(string originAddress, List<AddressDTO> destinationAddresses);
        List<decimal?> GetProximity(string originAddress, List<string> destinationAddresses);
    }
}