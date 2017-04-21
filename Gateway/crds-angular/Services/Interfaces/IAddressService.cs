using System.Device.Location;
using crds_angular.Models.Crossroads;

namespace crds_angular.Services.Interfaces
{
    public interface IAddressService
    {
        void FindOrCreateAddress(AddressDTO address, bool updateGeoCoordinates = false);
        int CreateAddress(AddressDTO address);
        void SetGeoCoordinates(AddressDTO address);

        GeoCoordinate GetGeoLocationCascading(AddressDTO address);
    }
}