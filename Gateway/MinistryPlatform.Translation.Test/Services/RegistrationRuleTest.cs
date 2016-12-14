using System;
using System.Collections.Generic;
using System.IO;
using MinistryPlatform.Translation.Repositories.Rules;
using NUnit.Framework;

namespace MinistryPlatform.Translation.Test.Services
{
    [TestFixture]
    class RegistrationRuleTest
    {
        private RegistrationRule _fixture;

        [Test]
        public void ShouldPassRegistrationRuleFutureEndDate()
        {
            _fixture = new RegistrationRule(DateTime.MinValue, DateTime.MaxValue, 0, 100);
            var data = new Dictionary<string, object> {{ "registrantCount", 1}};
            var result = _fixture.RulePasses(data);
            Assert.IsTrue(result.RulePassed);
            Assert.AreEqual("Rule Passed", result.Message);
        }

        [Test]
        public void ShouldPassRegistrationRuleNoEndDate()
        {
            _fixture = new RegistrationRule(DateTime.MinValue, null, 0, 100);
            var data = new Dictionary<string, object> {{ "registrantCount", 1}};
            var result = _fixture.RulePasses(data);
            Assert.IsTrue(result.RulePassed);
            Assert.AreEqual("Rule Passed", result.Message);
        }

        [Test]
        public void ShouldPassInactiveRegistrationRuleFutureStartDate()
        {
            _fixture = new RegistrationRule(DateTime.MaxValue, DateTime.MaxValue, 0, 100);
            var data = new Dictionary<string, object> {{ "registrantCount", 1}};
            var result = _fixture.RulePasses(data);
            Assert.IsTrue(result.RulePassed);
            Assert.AreEqual("Rule Not Active", result.Message);
        }

        [Test]
        public void ShouldPassInactiveRegistrationRulePastEndDate()
        {
            _fixture = new RegistrationRule(DateTime.MinValue, DateTime.Today.AddDays(-1), 0, 100);
            var data = new Dictionary<string, object> {{ "registrantCount", 1}};
            var result = _fixture.RulePasses(data);
            Assert.IsTrue(result.RulePassed);
            Assert.AreEqual("Rule Not Active", result.Message);
        }

        [Test]
        public void ShouldFailRegistrationRule()
        {
            _fixture = new RegistrationRule(DateTime.MinValue, DateTime.MaxValue, 0, 100);
            var data = new Dictionary<string, object> {{ "registrantCount", 101}};
            var result = _fixture.RulePasses(data);
            Assert.IsFalse(result.RulePassed);
            Assert.AreEqual("Exceeded Maximum of 100", result.Message);
        }

        [Test]
        public void ShouldFailRegistrationRuleBadData()
        {
            _fixture = new RegistrationRule(DateTime.MinValue, DateTime.MaxValue, 0, 100);
            var data = new Dictionary<string, object> {{ "registrantCount", "threeve"}};
            var ex = Assert.Throws<InvalidDataException>(() => _fixture.RulePasses(data));
            Assert.AreEqual("Invalid data provided to registration rule.", ex.Message);
        }
    }
}