using System.Collections.Generic;
using crds_angular.Models.Crossroads;
using crds_angular.Models.Crossroads.Attribute;
using MinistryPlatform.Translation.Models;

namespace crds_angular.Services.Interfaces
{
    public interface IAttributeService
    {
        List<AttributeTypeDTO> GetAttributeTypes(int? attributeTypeId);
        AttributeDTO ConvertAttributeToAttributeDto(MpAttribute attribute);
        List<MpAttribute> CreateMissingAttributes(List<MpAttribute> attributes, int attributeType);
        List<AttributeCategoryDTO> GetAttributeCategory(int attributeTypeId);
        AttributeDTO GetOneAttributeByCategoryId(int categoryId);
    }
}