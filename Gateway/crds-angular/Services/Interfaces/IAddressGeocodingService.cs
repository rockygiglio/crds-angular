using System.Device.Location;
using crds_angular.Models.Crossroads;

namespace crds_angular.Services.Interfaces
{
    public interface IAddressGeocodingService
    {
        GeoCoordinate GetGeoCoordinates(string address);
        GeoCoordinate GetGeoCoordinates(AddressDTO address);
        AddressDTO ValidateAddress(string address);
        AddressDTO ValidateAddress(AddressDTO address);
    }
}
