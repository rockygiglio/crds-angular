using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using crds_angular.Models.Crossroads.Attribute;
using crds_angular.Models.Crossroads.Groups;
using crds_angular.Services.Interfaces;
using MinistryPlatform.Models;
using Attribute = MinistryPlatform.Models.Attribute;
using IAttributeService = MinistryPlatform.Translation.Services.Interfaces.IAttributeService;
using IGroupService = MinistryPlatform.Translation.Services.Interfaces.IGroupService;

namespace crds_angular.Services
{
    public class GroupSearchService : IGroupSearchService
    {
        private readonly IGroupService _mpGroupService;
        private readonly IAttributeService _attributeService;

        public GroupSearchService(IGroupService mpGroupService,
                                  IAttributeService attributeService)
        {
            _mpGroupService = mpGroupService;
            _attributeService = attributeService;
        }

        public List<GroupDTO> FindMatches(int groupTypeId, GroupParticipantDTO participant)
        {
            // Load all groups for potential match            
            var mpGroups = _mpGroupService.GetSearchResults(groupTypeId);
            var mpAttributes = _attributeService.GetAttributes(null);

            var mpFilteredGroups = FilterSearchResults(participant, mpGroups);

            var groups = ConvertToGroupDto(mpFilteredGroups, mpAttributes);
            return groups;            
        }

        private IEnumerable<GroupSearchResult> FilterSearchResults(GroupParticipantDTO participant, IEnumerable<GroupSearchResult> mpGroups)
        {
            // TODO: Add to configuration value
            var groupGoalAttributeTypeId = 71;
            var groupTypeAttributeTypeId = 73;
            var participantGoalAttributeTypeId = 72;
            var genderTypeAttributeTypeId = 76;
            var maritalStatusTypeAttributeTypeId = 77;
            var weekdayTimesAttributeTypeId = 78;
            var weekendTimesAttributeTypeId = 79;

            ObjectSingleAttributeDTO participantGoal;
            ObjectSingleAttributeDTO gender;
            ObjectSingleAttributeDTO maritalStatus;
            ObjectAttributeTypeDTO weekdayTimes;
            ObjectAttributeTypeDTO weekendTimes;

            // Single-Attributes
            participant.SingleAttributes.TryGetValue(participantGoalAttributeTypeId, out participantGoal);
            participant.SingleAttributes.TryGetValue(genderTypeAttributeTypeId, out gender);
            participant.SingleAttributes.TryGetValue(maritalStatusTypeAttributeTypeId, out maritalStatus);

            // Attributes
            participant.AttributeTypes.TryGetValue(weekdayTimesAttributeTypeId, out weekdayTimes);
            participant.AttributeTypes.TryGetValue(weekendTimesAttributeTypeId, out weekendTimes);

            return mpGroups.Where(mpGroup => CheckCapacity(maritalStatus, mpGroup.RemainingCapacity) &&
                                             InMarket(mpGroup.Address.Postal_Code) &&
                                             MatchDayTime(weekdayTimes, weekendTimes, mpGroup.SearchAttributes.MeetingRangeId) &&
                                             MatchGroupType(gender, maritalStatus, mpGroup.SearchAttributes.TypeId));
        }

        private Boolean InMarket(String postalCode)
        {
            //TODO handle "in market" zip codes
            return true;
        }

        private Boolean CheckCapacity(ObjectSingleAttributeDTO maritalStatus, int capacity)
        {
            //TODO use config
            var journeyTogetherId = 7022;
            return capacity > (maritalStatus.Value.AttributeId == journeyTogetherId ? 1 : 0);
        }

        private Boolean MatchGroupType(ObjectSingleAttributeDTO gender, ObjectSingleAttributeDTO maritalStatus, int groupTypeId)
        {
            //TODO handle type logic, marital status and gender
            //TODO use config
            var journeyTogether = 7022;
            var man = 7018;
            var woman = 7019;
            var married = 7010;
            var mixed = 7007;
            var men = 7008;
            var women = 7009;

            return (groupTypeId == mixed && maritalStatus.Value.AttributeId != journeyTogether)
                   //TODO I can't remember if we wanted to force married couples with other married couples or not
                   || (groupTypeId == married && maritalStatus.Value.AttributeId == journeyTogether)
                   || (groupTypeId == men && gender.Value.AttributeId == man)
                   || (groupTypeId == women && gender.Value.AttributeId == woman)
                ;
        }

        private Boolean MatchDayTime(ObjectAttributeTypeDTO weekdayTime, ObjectAttributeTypeDTO weekendTime, int attributeId)
        {
            var weekdayMatch = weekdayTime.Attributes.Exists(x => x.AttributeId == attributeId);
            var weekendMatch = weekendTime.Attributes.Exists(x => x.AttributeId == attributeId);
            return weekdayMatch || weekendMatch;
        }

        private List<GroupDTO> ConvertToGroupDto(IEnumerable<GroupSearchResult> mpGroups, List<Attribute> mpAttributes)
        {
            var groups = new List<GroupDTO>();

            foreach (var mpGroup in mpGroups)
            {
                var group = Mapper.Map<GroupDTO>(mpGroup);

                var searchAttributes = mpGroup.SearchAttributes;

                // Multi-Attributes
                var groupPets = GetPetAttributes(mpAttributes, mpGroup, searchAttributes);
                group.AttributeTypes.Add(groupPets.AttributeTypeId, groupPets);

                // Single-Attributes
                var goal = ConvertToSingleAttribute(mpAttributes, searchAttributes.GoalId);
                group.SingleAttributes.Add(goal.Key, goal.Value);

                var kids = ConvertToSingleAttribute(mpAttributes, searchAttributes.KidsId);
                group.SingleAttributes.Add(kids.Key, kids.Value);

                var groupType = ConvertToSingleAttribute(mpAttributes, searchAttributes.TypeId);
                group.SingleAttributes.Add(groupType.Key, groupType.Value);

                groups.Add(group);
            }

            return groups;
        }

        private ObjectAttributeTypeDTO GetPetAttributes(List<Attribute> mpAttributes, GroupSearchResult mpGroup, GroupSearchAttributes searchAttributes)
        {
            // TODO: Add to configuration value
            var petsAttributeTypeId = 74;

            var petsAttributeType = mpAttributes.First(x => x.AttributeTypeId == petsAttributeTypeId);

            var groupPets = new ObjectAttributeTypeDTO()
            {
                AttributeTypeId = petsAttributeType.AttributeTypeId,
                Name = petsAttributeType.AttributeTypeName,
                Attributes = new List<ObjectAttributeDTO>()
            };

            // TODO: Determine if we can loop over the attributes and lookup if attribute type is a searchAttrbitues.CatId or DogId
            // To remove need to hardcode/lookup catAttributeTypeId            
            var catAttributeId = 7012;
            var dogAttributeId = 7011;

            var cat = ConvertToMultiAttribute(mpAttributes, catAttributeId, searchAttributes.CatId.HasValue);
            groupPets.Attributes.Add(cat);

            var dog = ConvertToMultiAttribute(mpAttributes, dogAttributeId, searchAttributes.DogId.HasValue);
            groupPets.Attributes.Add(dog);

            return groupPets;
        }

        private KeyValuePair<int, ObjectSingleAttributeDTO> ConvertToSingleAttribute(List<Attribute> mpAttributes, int attributeId)
        {
            var mpAttribute = mpAttributes.First(x => x.AttributeId == attributeId);
            var groupSingleAttribute = new ObjectSingleAttributeDTO()
            {
                Notes = string.Empty,
                Value = new AttributeDTO()
                {
                    AttributeId = mpAttribute.AttributeId,
                    Category = mpAttribute.Category,
                    CategoryDescription = mpAttribute.CategoryDescription,
                    CategoryId = mpAttribute.CategoryId,
                    Description = mpAttribute.Description,
                    Name = mpAttribute.Name,
                    SortOrder = mpAttribute.SortOrder
                }
            };

            return new KeyValuePair<int, ObjectSingleAttributeDTO>(mpAttribute.AttributeTypeId, groupSingleAttribute);            
        }

        private ObjectAttributeDTO ConvertToMultiAttribute(List<Attribute> mpAttributes, int attributeId, bool selected)
        {
            var mpAttribute = mpAttributes.First(x => x.AttributeId == attributeId);

            var groupAttribute = new ObjectAttributeDTO()
            {
                AttributeId = mpAttribute.AttributeId,
                Category = mpAttribute.Category,
                CategoryDescription = mpAttribute.CategoryDescription,
                Description = mpAttribute.Description,
                Selected = selected,
                StartDate = DateTime.Today,
                EndDate = null,
                Notes = string.Empty
            };

            return groupAttribute;
        }
    }
}