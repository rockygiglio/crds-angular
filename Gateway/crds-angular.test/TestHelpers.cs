using System;
using System.Collections.Generic;
using System.Linq;
using crds_angular.Models.Crossroads.Attribute;
using crds_angular.Models.Crossroads.GoVolunteer;
using FsCheck;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Models;
using MvcContrib;
using NUnit.Framework;
using Random = System.Random;

namespace crds_angular.test
{
    public static class TestHelpers
    {
        public static Random rnd = new Random(Guid.NewGuid().GetHashCode());           
        public static int RandomInt()
        {
            //var sample =  Gen.Sample(100, 100, Gen.OneOf(Arb.Generate<int>()));
            
            return rnd.Next();
            //var elem = rnd.Next(sample.Length -1);
            //return sample[elem];
        }

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
            return Enumerable.Range(0, size).Select(_ =>
                new GoSkills(
                    Gen.Sample(10000, 10000, Gen.OneOf(Arb.Generate<int>())).HeadOrDefault,
                    Gen.Sample(100, 10000, Gen.OneOf(Arb.Generate<int>())).HeadOrDefault,
                    Gen.Sample(100, 100, Gen.OneOf(Arb.Generate<string>())).HeadOrDefault,
                    Gen.Sample(100, 100, Gen.OneOf(Arb.Generate<string>())).HeadOrDefault,
                    Gen.Sample(100, 2, Gen.OneOf(Arb.Generate<bool>())).HeadOrDefault
                    )
            ).ToList();
                                          
            //return Enumerable.Repeat<int>(1, size).Select(i => new GoSkills(
            //    Gen.Sample(1, 1, Gen.OneOf(Arb.Generate<int>())).HeadOrDefault,
            //    Gen.Sample(1, 1, Gen.OneOf(Arb.Generate<int>())).HeadOrDefault,
            //    Gen.Sample(100, 100, Gen.OneOf(Arb.Generate<string>())).OrderBy(x => Guid.NewGuid()).Take(1).Single(),
            //    Gen.Sample(1, 1, Gen.OneOf(Arb.Generate<string>())).OrderBy(x => Guid.NewGuid()).Take(1).Single(),
            //    Gen.Sample(1, 1, Gen.OneOf(Arb.Generate<bool>())).OrderBy(x => Guid.NewGuid()).Take(1).Single())).ToList();           
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

        public static MyContact MyContact()
        {
            return new MyContact()
            {
                Address_ID = RandomInt(),
                Address_Line_1 = Gen.Sample(20, 1, Gen.OneOf(Arb.Generate<string>())).HeadOrDefault,
                Age = RandomInt(),
                City = Gen.Sample(20, 1, Gen.OneOf(Arb.Generate<string>())).HeadOrDefault,
                Contact_ID = RandomInt(),
                County = Gen.Sample(20, 1, Gen.OneOf(Arb.Generate<string>())).HeadOrDefault,
            };
        }
    }
}
