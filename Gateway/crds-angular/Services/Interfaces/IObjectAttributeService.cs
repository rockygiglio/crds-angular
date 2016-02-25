using System.Collections.Generic;
using crds_angular.Models.Crossroads.Profile;
using MinistryPlatform.Models;

namespace crds_angular.Services.Interfaces
{
    public interface IObjectAttributeService
    {
        ObjectAllAttributesDTO GetObjectAttributes(string token, int objectId);
        void SaveObjectAttributes(int objectId, Dictionary<int, ObjectAttributeTypeDTO> objectAttributes, Dictionary<int, ObjectSingleAttributeDTO> objectSingleAttributes);
        void SaveObjectMultiAttribute(string token, int objectId, ObjectAttributeDTO objectAttribute);
    }
}