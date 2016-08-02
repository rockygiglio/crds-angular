using System.Collections.Generic;
using System.Linq;
using crds_angular.Models.Crossroads.Attribute;
using crds_angular.Services.Interfaces;
using MinistryPlatform.Translation.Models;
using MPInterfaces = MinistryPlatform.Translation.Repositories.Interfaces;

namespace crds_angular.Services
{
    public class AttributeService : IAttributeService
    {
        private readonly MPInterfaces.IAttributeRepository _attributeService;

        public AttributeService(MPInterfaces.IAttributeRepository attributeService)
        {
            _attributeService = attributeService;
        }

        public List<AttributeTypeDTO> GetAttributeTypes(int? attributeTypeId)
        {
            var attributes = _attributeService.GetAttributes(attributeTypeId);

            var attributeTypes = new Dictionary<int, AttributeTypeDTO>();

            foreach (var attribute in attributes)
            {
                var attributeDto = ConvertAttributeToAttributeDto(attribute);

                var key = GetOrCreateAttributeTypeDto(attribute, attributeTypes);

                attributeTypes[key].Attributes.Add(attributeDto);
            }

            return attributeTypes.Values.ToList();
        }

        public AttributeDTO ConvertAttributeToAttributeDto(MpAttribute attribute)
        {
            var attributeDto = new AttributeDTO
            {
                AttributeId = attribute.AttributeId,
                Name = attribute.Name,
                Description = attribute.Description,
                SortOrder = attribute.SortOrder,
                CategoryId = attribute.CategoryId,
                Category = attribute.Category,
                CategoryDescription = attribute.CategoryDescription
            };
            return attributeDto;
        }

        public int CreateOrUpdateAttributes(List<AttributeDTO> attributes, int attributeType)
        {
            var attributesList = attributes.Select(attribute => new MpAttribute
            {
                AttributeId = attribute.AttributeId,
                Name = attribute.Name,
                Description = attribute.Description,
                SortOrder = attribute.SortOrder,
                CategoryId = attribute.CategoryId,
                Category = attribute.Category,
                CategoryDescription = attribute.CategoryDescription,
                AttributeTypeId = attribute.AttributeTypeId ?? default(int)
            }).ToList();

            this._attributeService.CreateMissingAttributes(attributesList, attributeType);
     
            return 1;
        }

        private static int GetOrCreateAttributeTypeDto(MpAttribute attribute, Dictionary<int, AttributeTypeDTO> attributeTypes)
        {
            var attributeTypeDto = new AttributeTypeDTO()
            {
                AttributeTypeId = attribute.AttributeTypeId,
                Name = attribute.AttributeTypeName,
                AllowMultipleSelections = !attribute.PreventMultipleSelection
            };

            var key = attributeTypeDto.AttributeTypeId;

            if (!attributeTypes.ContainsKey(key))
            {
                attributeTypes[key] = attributeTypeDto;
            }
            return key;
        }
    }
}