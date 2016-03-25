using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using crds_angular.Models.Crossroads.GoVolunteer;
using crds_angular.Services;
using FsCheck;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Services.Interfaces;
using Moq;
using NUnit.Framework;

namespace crds_angular.test.Services
{
    [TestFixture]
    public class GoSkillsServiceTest
    {
        private GoSkillsService _fixture;
        private Mock<IApiUserService> _apiUserService;
        private Mock<ISkillsService> _skillsService;

        [SetUp]
        public void Setup()
        {
            _apiUserService = new Mock<IApiUserService>();
            _skillsService = new Mock<ISkillsService>();
            _fixture = new GoSkillsService(_apiUserService.Object, _skillsService.Object);
            
        }

        [Test]
        public void ShouldRetrieveGoSkills()
        {

            Prop.ForAll<string>( token =>
            {
                var skills = MPSkills();
                _apiUserService.Setup(m => m.GetToken()).Returns(token);
                _skillsService.Setup(m => m.GetGoVolunteerSkills(token)).Returns(skills);
                var returned = _fixture.RetrieveGoSkills();
                Assert.IsInstanceOf<List<GoSkills>>(returned);
                Assert.AreEqual(skills.Count, returned.Count);
                _skillsService.VerifyAll();
            }).QuickCheckThrowOnFailure();           
        }

        private static List<MPGoVolunteerSkill> MPSkills()
        {            
            return new List<int>(10).Select(ls => new MPGoVolunteerSkill(
                         Gen.Sample(1, 1, Gen.OneOf(Arb.Generate<int>())).HeadOrDefault,
                         Gen.Sample(1, 1, Gen.OneOf(Arb.Generate<string>())).HeadOrDefault,
                         Gen.Sample(1, 1, Gen.OneOf(Arb.Generate<string>())).HeadOrDefault
                         )).ToList();          
        } 

    }
}
