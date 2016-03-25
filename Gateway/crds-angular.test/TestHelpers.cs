using System.Collections.Generic;
using System.Linq;
using crds_angular.Models.Crossroads.GoVolunteer;
using FsCheck;
using MinistryPlatform.Translation.Models;

namespace crds_angular.test
{
    public static class TestHelpers
    {
        public static List<MPGoVolunteerSkill> MPSkills(int size = 10)
        {
            return Enumerable.Repeat<MPGoVolunteerSkill>(new MPGoVolunteerSkill(
                Gen.Sample(1, 1, Gen.OneOf(Arb.Generate<int>())).HeadOrDefault,
                Gen.Sample(1, 1, Gen.OneOf(Arb.Generate<string>())).HeadOrDefault,
                Gen.Sample(1, 1, Gen.OneOf(Arb.Generate<string>())).HeadOrDefault), size).ToList();        
        }

        public static List<GoSkills> ListOfGoSkills(int size = 10)
        {
            return Enumerable.Repeat<GoSkills>(new GoSkills(
                Gen.Sample(1, 1, Gen.OneOf(Arb.Generate<int>())).HeadOrDefault,
                Gen.Sample(1, 1, Gen.OneOf(Arb.Generate<string>())).HeadOrDefault,
                Gen.Sample(1, 1, Gen.OneOf(Arb.Generate<string>())).HeadOrDefault), size).ToList();           
        }
    }
}
