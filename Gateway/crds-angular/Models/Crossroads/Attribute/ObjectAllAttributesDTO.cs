using System.Collections.Generic;

namespace crds_angular.Models.Crossroads.Attribute
{
    public class ObjectAllAttributesDTO
    {
        public Dictionary<int, ObjectAttributeTypeDTO> MultiSelect { get; set; }
        public Dictionary<int, ObjectSingleAttributeDTO> SingleSelect { get; set; }
    }
}