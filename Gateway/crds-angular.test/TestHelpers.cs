using System.Collections.Generic;
using System.Linq;
using crds_angular.Models.Crossroads.Attribute;
using crds_angular.Models.Crossroads.GoVolunteer;
using FsCheck;
using MinistryPlatform.Translation.Models;

namespace crds_angular.test
{
    public static class TestHelpers
    {
        public static List<MpGoVolunteerSkill> MPSkills(int size = 10)
        {
            return Enumerable.Repeat<MpGoVolunteerSkill>(new MpGoVolunteerSkill(
                Gen.Sample(1, 1, Gen.OneOf(Arb.Generate<int>())).HeadOrDefault,
                Gen.Sample(1, 1, Gen.OneOf(Arb.Generate<string>())).HeadOrDefault,
                Gen.Sample(1, 1, Gen.OneOf(Arb.Generate<string>())).HeadOrDefault,
                Gen.Sample(1, 1, Gen.OneOf(Arb.Generate<int>())).HeadOrDefault), size).ToList();        
        }

        public static List<GoSkills> ListOfGoSkills(int size = 10)
        {
            return Enumerable.Repeat<GoSkills>(new GoSkills(
                Gen.Sample(1, 1, Gen.OneOf(Arb.Generate<int>())).HeadOrDefault,
                Gen.Sample(1, 1, Gen.OneOf(Arb.Generate<string>())).HeadOrDefault,
                Gen.Sample(1, 1, Gen.OneOf(Arb.Generate<string>())).HeadOrDefault,
                Gen.Sample(1, 1, Gen.OneOf(Arb.Generate<bool>())).HeadOrDefault), size).ToList();           
        }

        public static List<AttributeTypeDTO> ListOfAttributeTypeDtos(int size = 10, int attributeListSize = 10)
        {
            return Enumerable.Repeat<AttributeTypeDTO>(new AttributeTypeDTO()
            {
                AllowMultipleSelections = Gen.Sample(1, 1, Gen.OneOf(Gen.Constant(true), Gen.Constant(false))).HeadOrDefault,
                Attributes = ListOfAttributeDtos(attributeListSize),
                AttributeTypeId = Gen.Sample(1, 1, Gen.OneOf(Arb.Generate<int>())).HeadOrDefault,
                Name = Gen.Sample(1, 1, Gen.OneOf(Arb.Generate<string>())).HeadOrDefault
            }, size).ToList();
        }

        public static List<AttributeDTO> ListOfAttributeDtos(int size = 10)
        {
            return Enumerable.Repeat<AttributeDTO>(new AttributeDTO()
            {
                AttributeId = Gen.Sample(1, 1, Gen.OneOf(Arb.Generate<int>())).HeadOrDefault,
                Name = Gen.Sample(20, 1, Gen.OneOf(Arb.Generate<string>())).HeadOrDefault,
                Category = Gen.Sample(20, 1, Gen.OneOf(Arb.Generate<string>())).HeadOrDefault,
                CategoryDescription = Gen.Sample(1, 1, Gen.OneOf(Arb.Generate<string>())).HeadOrDefault,
                CategoryId = Gen.Sample(1, 1, Gen.OneOf(Arb.Generate<int>())).HeadOrDefault,
                Description = Gen.Sample(1, 1, Gen.OneOf(Arb.Generate<string>())).HeadOrDefault,
                SortOrder = Gen.Sample(1, 1, Gen.OneOf(Arb.Generate<int>())).HeadOrDefault
            }, size).ToList();
        } 
    }
}
