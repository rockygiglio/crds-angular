using System;
using System.Collections.Generic;
using System.IO;
using MinistryPlatform.Translation.Repositories.Rules;
using NUnit.Framework;

namespace MinistryPlatform.Translation.Test.Services
{
    [TestFixture]
    public class GenderRuleTest
    {
        private GenderRule _fixture;

        [Test]
        public void ShouldPassGenderRuleFutureEndDate()
        {
            _fixture = new GenderRule(DateTime.MinValue, DateTime.MaxValue, 1);
            var data = new Dictionary<string, object> {{"GenderId", 1}};
            var result = _fixture.RulePasses(data);
            Assert.IsTrue(result.RulePassed);
            Assert.AreEqual("Rule Passed", result.Message);
        }

        [Test]
        public void ShouldPassGenderRuleNoEndDate()
        {
            _fixture = new GenderRule(DateTime.MinValue, null, 1);
            var data = new Dictionary<string, object> { { "GenderId", 1 } };
            var result = _fixture.RulePasses(data);
            Assert.IsTrue(result.RulePassed);
            Assert.AreEqual("Rule Passed", result.Message);
        }

        [Test]
        public void ShouldPassInactiveGenderRuleFutureStartDate()
        {
            _fixture = new GenderRule(DateTime.MaxValue, null, 1);
            var data = new Dictionary<string, object> { { "GenderId", 1 } };
            var result = _fixture.RulePasses(data);
            Assert.IsTrue(result.RulePassed);
            Assert.AreEqual("Rule Not Active", result.Message);
        }

        [Test]
        public void ShouldPassInactiveGenderRulePastEndDate()
        {
            _fixture = new GenderRule(DateTime.MinValue, DateTime.Today.AddDays(-1) ,1);
            var data = new Dictionary<string, object> { { "GenderId", 1 } };
            var result = _fixture.RulePasses(data);
            Assert.IsTrue(result.RulePassed);
            Assert.AreEqual("Rule Not Active", result.Message);
        }

        [Test]
        public void ShouldFailGenderRule()
        {
            _fixture = new GenderRule(DateTime.MinValue, DateTime.MaxValue, 1);
            var data = new Dictionary<string, object> { { "GenderId", 2 } };
            var result = _fixture.RulePasses(data);
            Assert.IsFalse(result.RulePassed);
            Assert.AreEqual("Gender must be 1", result.Message);
        }

        [Test]
        public void ShouldFailGenderRuleBadData()
        {
            _fixture = new GenderRule(DateTime.MinValue, DateTime.MaxValue, 1);
            var data = new Dictionary<string, object> { { "GenderId", "MALE" } };
            var ex = Assert.Throws<InvalidDataException>(() => _fixture.RulePasses(data));
            Assert.AreEqual("Invalid data provided to Gender Rule.", ex.Message);
        }
    }
}
