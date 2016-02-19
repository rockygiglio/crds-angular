using crds_angular.Models.Crossroads;

namespace crds_angular.Services.Interfaces
{
    public interface IAddressService
    {
        void FindOrCreateAddress(AddressDTO address);
    }
}