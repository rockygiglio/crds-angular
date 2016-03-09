using System.Collections.Generic;
using crds_angular.Models.Crossroads.Attribute;
using MinistryPlatform.Models;

namespace crds_angular.Services.Interfaces
{
    public interface IObjectAttributeService
    {
        ObjectAllAttributesDTO GetObjectAttributes(string token, int objectId, ObjectAttributeConfiguration configuration);
        ObjectAllAttributesDTO GetObjectAttributes(string token, int objectId, ObjectAttributeConfiguration configuration, List<Attribute> mpAttributes);

        void SaveObjectAttributes(int objectId,
                                  Dictionary<int, ObjectAttributeTypeDTO> objectAttributes,
                                  Dictionary<int, ObjectSingleAttributeDTO> objectSingleAttributes,
                                  ObjectAttributeConfiguration configuration);

        void SaveObjectMultiAttribute(string token, int objectId, ObjectAttributeDTO objectAttribute, ObjectAttributeConfiguration configuration);
    }
}