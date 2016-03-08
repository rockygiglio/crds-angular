using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using crds_angular.Models.Crossroads.Attribute;
using crds_angular.Models.Crossroads.Groups;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Interfaces;
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
        private readonly int GroupGoalAttributeTypeId;
        private readonly int GroupTypeAttributeTypeId;
        private readonly int ParticipantGoalAttributeTypeId;
        private readonly int GenderTypeAttributeTypeId;
        private readonly int MaritalStatusTypeAttributeTypeId;
        private readonly int WeekdayTimesAttributeTypeId;
        private readonly int WeekendTimesAttributeTypeId;

        private readonly int ParticipantJourneyTogetherAttributeId;
        private readonly int ParticipantGenderManAttributeId;
        private readonly int ParticipantGenderWomanAttributeId;

        private readonly int GroupTypeMixedAttributeId;
        private readonly int GroupTypeMenAttributeId;
        private readonly int GroupTypeWomenAttributeId;
        private readonly int GroupTypeMarriedCouplesAttributeId;

        private readonly int GroupPetsAttributeTypeId;
        private readonly int GroupPetsDogAttributeId;
        private readonly int GroupPetsCatAttributeId;

        private readonly int GroupGoal1AttributeId;
        private readonly int GroupGoal2AttributeId;
        private readonly int GroupGoal3AttributeId;
        private readonly int GroupGoal4AttributeId;

        private readonly int ParticipantGoal1AttributeId;
        private readonly int ParticipantGoal2AttributeId;
        private readonly int ParticipantGoal3AttributeId;
        private readonly int ParticipantGoal4AttributeId;

        public GroupSearchService(IGroupService mpGroupService,
                                  IAttributeService attributeService, 
                                  IConfigurationWrapper configurationWrapper)
        {
            _mpGroupService = mpGroupService;
            _attributeService = attributeService;

            GroupGoalAttributeTypeId = configurationWrapper.GetConfigIntValue("GroupGoalAttributeTypeId");
            GroupTypeAttributeTypeId = configurationWrapper.GetConfigIntValue("GroupTypeAttributeTypeId");
            ParticipantGoalAttributeTypeId = configurationWrapper.GetConfigIntValue("ParticipantGoalAttributeTypeId");
            GenderTypeAttributeTypeId = configurationWrapper.GetConfigIntValue("GenderTypeAttributeTypeId");
            MaritalStatusTypeAttributeTypeId = configurationWrapper.GetConfigIntValue("MaritalStatusTypeAttributeTypeId");
            WeekdayTimesAttributeTypeId = configurationWrapper.GetConfigIntValue("WeekdayTimesAttributeTypeId");
            WeekendTimesAttributeTypeId = configurationWrapper.GetConfigIntValue("WeekendTimesAttributeTypeId");

            ParticipantJourneyTogetherAttributeId = configurationWrapper.GetConfigIntValue("ParticipantJourneyTogetherAttributeId");
            ParticipantGenderManAttributeId = configurationWrapper.GetConfigIntValue("ParticipantGenderManAttributeId");
            ParticipantGenderWomanAttributeId = configurationWrapper.GetConfigIntValue("ParticipantGenderWomanAttributeId");

            GroupTypeMixedAttributeId = configurationWrapper.GetConfigIntValue("GroupTypeMixedAttributeId");
            GroupTypeMenAttributeId = configurationWrapper.GetConfigIntValue("GroupTypeMenAttributeId");
            GroupTypeWomenAttributeId = configurationWrapper.GetConfigIntValue("GroupTypeWomenAttributeId");
            GroupTypeMarriedCouplesAttributeId = configurationWrapper.GetConfigIntValue("GroupTypeMarriedCouplesAttributeId");

            GroupPetsAttributeTypeId = configurationWrapper.GetConfigIntValue("GroupPetsAttributeTypeId");
            GroupPetsCatAttributeId = configurationWrapper.GetConfigIntValue("GroupPetsCatAttributeId");
            GroupPetsDogAttributeId = configurationWrapper.GetConfigIntValue("GroupPetsDogAttributeId");

            GroupGoal1AttributeId = configurationWrapper.GetConfigIntValue("GroupGoal1AttributeId");
            GroupGoal2AttributeId = configurationWrapper.GetConfigIntValue("GroupGoal2AttributeId");
            GroupGoal3AttributeId = configurationWrapper.GetConfigIntValue("GroupGoal3AttributeId");
            GroupGoal4AttributeId = configurationWrapper.GetConfigIntValue("GroupGoal4AttributeId");

            ParticipantGoal1AttributeId = configurationWrapper.GetConfigIntValue("ParticipantGoal1AttributeId");
            ParticipantGoal2AttributeId = configurationWrapper.GetConfigIntValue("ParticipantGoal2AttributeId");
            ParticipantGoal3AttributeId = configurationWrapper.GetConfigIntValue("ParticipantGoal3AttributeId");
            ParticipantGoal4AttributeId = configurationWrapper.GetConfigIntValue("ParticipantGoal4AttributeId");
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
            ObjectSingleAttributeDTO participantGoal;
            ObjectSingleAttributeDTO gender;
            ObjectSingleAttributeDTO maritalStatus;
            ObjectAttributeTypeDTO weekdayTimes;
            ObjectAttributeTypeDTO weekendTimes;

            // Single-Attributes
            participant.SingleAttributes.TryGetValue(ParticipantGoalAttributeTypeId, out participantGoal);
            participant.SingleAttributes.TryGetValue(GenderTypeAttributeTypeId, out gender);
            participant.SingleAttributes.TryGetValue(MaritalStatusTypeAttributeTypeId, out maritalStatus);

            // Attributes
            participant.AttributeTypes.TryGetValue(WeekdayTimesAttributeTypeId, out weekdayTimes);
            participant.AttributeTypes.TryGetValue(WeekendTimesAttributeTypeId, out weekendTimes);

            return mpGroups.Where(mpGroup =>
                                      CheckCapacity(maritalStatus, mpGroup.RemainingCapacity) &&
                                      InMarket(mpGroup.Address.Postal_Code) &&
                                      MatchDayTime(weekdayTimes, weekendTimes, mpGroup.SearchAttributes.MeetingRangeId) &&
                                      MatchGroupType(gender, maritalStatus, mpGroup.SearchAttributes.TypeId) &&
                                      MatchGoals(participantGoal, mpGroup.SearchAttributes.GoalId)
                );
        }

        private Boolean InMarket(String postalCode)
        {
            //TODO move to config
            var inMarket = new int[40355, 41001, 41005, 41006, 41007, 41011, 41012, 41014, 41015, 41016, 41017, 41018, 41019, 41022, 41030, 41033, 41035, 41040, 41042, 41045, 41046, 41048, 41051, 41052, 41053, 41054, 41059, 41063, 41071, 41072, 41073, 41074, 41075, 41076, 41080, 41083, 41085, 41086, 41091, 41092, 41094, 41095, 41097, 41099, 45001, 45002, 45003, 45004, 45005, 45011, 45012, 45013, 45014, 45015, 45018, 45030, 45032, 45033, 45034, 45036, 45039, 45040, 45041, 45042, 45044, 45050, 45051, 45052, 45053, 45054, 45055, 45056, 45061, 45062, 45063, 45064, 45065, 45066, 45067, 45068, 45069, 45070, 45071, 45102, 45103, 45106, 45107, 45111, 45112, 45113, 45114, 45118, 45119, 45120, 45122, 45130, 45140, 45142, 45146, 45147, 45148, 45150, 45152, 45153, 45154, 45156, 45157, 45158, 45160, 45162, 45174, 45176, 45177, 45201, 45202, 45203, 45204, 45205, 45206, 45207, 45208, 45209, 45211, 45212, 45213, 45214, 45215, 45216, 45217, 45218, 45219, 45220, 45221, 45222, 45223, 45224, 45225, 45226, 45227, 45229, 45230, 45231, 45232, 45233, 45234, 45235, 45236, 45237, 45238, 45239, 45240, 45241, 45242, 45243, 45244, 45245, 45246, 45247, 45248, 45249, 45250, 45251, 45252, 45253, 45254, 45255, 45258, 45262, 45263, 45264, 45267, 45268, 45269, 45270, 45271, 45273, 45274, 45275, 45277, 45280, 45296, 45298, 45299, 45301, 45305, 45311, 45320, 45325, 45327, 45330, 45342, 45343, 45345, 45370, 45381, 45401, 45402, 45403, 45406, 45409, 45410, 45412, 45413, 45417, 45419, 45420, 45422, 45423, 45428, 45429, 45430, 45432, 45434, 45437, 45439, 45440, 45441, 45448, 45449, 45458, 45459, 45469, 45470, 45475, 45479, 45481, 45482, 45490, 45999, 47001, 47003, 47006, 47010, 47011, 47012, 47016, 47017, 47018, 47019, 47020, 47021, 47022, 47024, 47025, 47030, 47031, 47032, 47033, 47035, 47036, 47038, 47039, 47040, 47041, 47043, 47060, 47324, 47325, 47353];
            postalCode = postalCode.Substring(0, 5);
            List<int> inMarketList = inMarket.OfType<int>().ToList();
            return inMarketList.Contains(int.Parse(postalCode));
        }

        private Boolean CheckCapacity(ObjectSingleAttributeDTO maritalStatus, int capacity)
        {
            return capacity > (maritalStatus.Value.AttributeId == ParticipantJourneyTogetherAttributeId ? 1 : 0);
        }

        private Boolean MatchGroupType(ObjectSingleAttributeDTO gender, ObjectSingleAttributeDTO maritalStatus, int? groupTypeId)
        {
            //TODO I can't remember if we wanted to force married couples with other married couples or not
            return (groupTypeId == GroupTypeMixedAttributeId && maritalStatus.Value.AttributeId != ParticipantJourneyTogetherAttributeId) ||
                   (groupTypeId == GroupTypeMarriedCouplesAttributeId && maritalStatus.Value.AttributeId == ParticipantJourneyTogetherAttributeId) ||
                   (groupTypeId == GroupTypeMenAttributeId && gender.Value.AttributeId == ParticipantGenderManAttributeId) ||
                   (groupTypeId == GroupTypeWomenAttributeId && gender.Value.AttributeId == ParticipantGenderWomanAttributeId);
        }

        private Boolean MatchDayTime(ObjectAttributeTypeDTO weekdayTime, ObjectAttributeTypeDTO weekendTime, int? attributeId)
        {
            var weekdayMatch = weekdayTime.Attributes.Exists(x => x.AttributeId == attributeId);
            var weekendMatch = weekendTime.Attributes.Exists(x => x.AttributeId == attributeId);
            return weekdayMatch || weekendMatch;
        }

        private Boolean MatchGoals(ObjectSingleAttributeDTO participantGoal, int GroupGoalID)
        {
            //TODO move to config
            var groupGoal1 = 6999;
            var groupGoal2 = 7002;
            var groupGoal3 = 7000;
            var groupGoal4 = 7001;
            var participantGoal1 = 7003;
            var participantGoal2 = 7004;
            var participantGoal3 = 7005;
            var participantGoal4 = 7006;

            Dictionary<int, int> matching = new Dictionary<int, int>()
            {
                { groupGoal1, participantGoal1},
                { groupGoal2, participantGoal2},
                { groupGoal3, participantGoal3},
                { groupGoal4, participantGoal4},
            };

            return participantGoal.Value.AttributeId == matching[GroupGoalID];
        }

        private List<GroupDTO> ConvertToGroupDto(IEnumerable<GroupSearchResult> mpGroups, List<Attribute> mpAttributes)
        {
            var groups = new List<GroupDTO>();

            foreach (var mpGroup in mpGroups)
            {
                var group = Mapper.Map<GroupDTO>(mpGroup);

                var searchAttributes = mpGroup.SearchAttributes;

                // Multi-Attributes
                var groupPets = GetPetAttributes(mpAttributes, searchAttributes);
                group.AttributeTypes.Add(groupPets.AttributeTypeId, groupPets);

                // Single-Attributes
                if (searchAttributes.GoalId.HasValue)
                {
                    var goal = ConvertToSingleAttribute(mpAttributes, searchAttributes.GoalId.Value);
                    group.SingleAttributes.Add(goal.Key, goal.Value);
                }               

                if (searchAttributes.KidsId.HasValue)
                {
                    var kids = ConvertToSingleAttribute(mpAttributes, searchAttributes.KidsId.Value);
                    group.SingleAttributes.Add(kids.Key, kids.Value);
                }

                if (searchAttributes.TypeId.HasValue)
                {
                    var groupType = ConvertToSingleAttribute(mpAttributes, searchAttributes.TypeId.Value);
                    group.SingleAttributes.Add(groupType.Key, groupType.Value);
                }

                groups.Add(group);
            }

            return groups;
        }

        private ObjectAttributeTypeDTO GetPetAttributes(List<Attribute> mpAttributes, GroupSearchAttributes searchAttributes)
        {
            var petsAttributeType = mpAttributes.First(x => x.AttributeTypeId == GroupPetsAttributeTypeId);

            var groupPets = new ObjectAttributeTypeDTO()
            {
                AttributeTypeId = petsAttributeType.AttributeTypeId,
                Name = petsAttributeType.AttributeTypeName,
                Attributes = new List<ObjectAttributeDTO>()
            };

            // TODO: Determine if we can loop over the attributes and lookup if attribute type is a searchAttrbitues.CatId or DogId           
            var cat = ConvertToMultiAttribute(mpAttributes, GroupPetsCatAttributeId, searchAttributes.CatId.HasValue);
            groupPets.Attributes.Add(cat);

            var dog = ConvertToMultiAttribute(mpAttributes, GroupPetsDogAttributeId, searchAttributes.DogId.HasValue);
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