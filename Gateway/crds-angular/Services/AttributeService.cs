using System;
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
        private readonly MPInterfaces.IAttributeRepository _attributeRepository;

        public AttributeService(MPInterfaces.IAttributeRepository attributeRepository)
        {
            _attributeRepository = attributeRepository;
        }

        public List<AttributeTypeDTO> GetAttributeTypes(int? attributeTypeId)
        {
            var attributes = _attributeRepository.GetAttributes(attributeTypeId);

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

        public List<MpAttribute> CreateMissingAttributes(List<MpAttribute> attributes, int attributeType)
        {
            var attributeCategories = attributes.Select(attribute => attribute.CategoryId).Distinct().ToList();

            List<string> foundNames = new List<string>();


            foreach (var category in attributeCategories)
            {
                var filter = "," + String.Join(" OR ",
                                               attributes
                                                   .Where(attCategory => attCategory.CategoryId == category)
                                                   .Select(attribute => attribute.Name.ToLower()).ToList()) + ",," + attributeType + ",,," + category;
                //TODO: I think we can just set the attribute ID here if we find it.
                foundNames.AddRange(_attributeRepository.GetAttributesByFilter(filter)
                                        .Select(records => records.Name.ToLower()).ToList());
            }

            foreach (var attribute in attributes)
            {
                if (foundNames.Contains(attribute.Name.ToLower()))
                {
                    var filter = $",{attribute.Name},,,,,,{attribute.Category}";
                    attributes.First(a => a.Name == attribute.Name && a.CategoryId == attribute.CategoryId)
                        .AttributeId = _attributeRepository.GetAttributesByFilter(filter)[0].AttributeId;
                }
                else
                {
                    attributes.First(a => a.Name == attribute.Name && a.CategoryId == attribute.CategoryId)
                        .AttributeId = _attributeRepository.CreateAttribute(attribute);
                }
            }

            return attributes;
        }
    }
}