﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crossroads.Utilities.Interfaces;
using Crossroads.Web.Common;
using Crossroads.Web.Common.Configuration;
using Crossroads.Web.Common.Security;
using MinistryPlatform.Translation.Extensions;
using MinistryPlatform.Translation.PlatformService;
using MinistryPlatform.Translation.Repositories;
using MinistryPlatform.Translation.Repositories.Interfaces;
using Moq;
using NUnit.Framework;

namespace MinistryPlatform.Translation.Test.Services
{
    public class UserServiceTest
    {
        private UserRepository _fixture;

        private Mock<IAuthenticationRepository> _authenticationService;
        private Mock<IConfigurationWrapper> _configurationWrapper;
        private Mock<IMinistryPlatformService> _ministryPlatformService;

        [SetUp]
        public void SetUp()
        {
            _authenticationService = new Mock<IAuthenticationRepository>();
            _configurationWrapper = new Mock<IConfigurationWrapper>();
            _ministryPlatformService = new Mock<IMinistryPlatformService>(MockBehavior.Strict);

            _configurationWrapper.Setup(mocked => mocked.GetConfigIntValue("UsersApiLookupPageView")).Returns(102030);
            _authenticationService.Setup(m => m.Authenticate(It.IsAny<string>(), It.IsAny<string>())).Returns(new AuthToken
            {
                AccessToken = "ABC",
                ExpiresIn = 123
            });

            _fixture = new UserRepository(_authenticationService.Object, _configurationWrapper.Object, _ministryPlatformService.Object);
        }

        [Test]
        public void TestGetUserById()
        {
            var mpResult = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    {"Can_Impersonate", true},
                    {"User_GUID", Guid.NewGuid()},
                    {"User_Name", "me@here.com"},
                    {"User_Email", "me@here.com"},
                    {"dp_RecordID", 1 }

                }
            };
            _ministryPlatformService.Setup(mocked => mocked.GetPageViewRecords(102030, "ABC", "\"me@here.com\",", string.Empty, 0)).Returns(mpResult);

            var user = _fixture.GetByUserId("me@here.com");
            _authenticationService.VerifyAll();
            _ministryPlatformService.VerifyAll();
            Assert.IsNotNull(user);
            Assert.AreEqual("me@here.com", user.UserId);
            Assert.AreEqual(mpResult[0]["User_GUID"].ToString(), user.Guid);
            Assert.IsTrue(user.CanImpersonate);
        }

        [Test]
        public void TestGetUserRoles()
        {
            var mpResult = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    {"Role_ID", 123},
                    {"Role_Name", "Role 123"}
                },
                new Dictionary<string, object>
                {
                    {"Role_ID", 456},
                    {"Role_Name", "Role 456"}
                }
            };

            _ministryPlatformService.Setup(mocked => mocked.GetSubpageViewRecords("User_Roles_With_ID", 987, "ABC", string.Empty, string.Empty, 0)).Returns(mpResult);

            var roles = _fixture.GetUserRoles(987);
            Assert.IsNotNull(roles);
            Assert.AreEqual(mpResult.Count, roles.Count);
            foreach (var result in mpResult)
            {
                Assert.IsTrue(roles.Exists(role => role.Id == result.ToInt("Role_ID") && role.Name.Equals(result.ToString("Role_Name"))));
            }
        }

        [Test]
        public void TestGetUserByAuthenticationToken()
        {
            var mpResult = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    {"Can_Impersonate", true},
                    {"User_GUID", Guid.NewGuid()},
                    {"User_Name", "me@here.com"},
                    {"User_Email", "me@here.com"},
                    {"dp_RecordID", 1 }
                        
                }
            };

            _ministryPlatformService.Setup(mocked => mocked.GetContactInfo("logged in")).Returns(new PlatformService.UserInfo() { ContactId = 123 });
            _ministryPlatformService.Setup(mocked => mocked.GetPageViewRecords(102030, "ABC", ",\"123\"", string.Empty, 0)).Returns(mpResult);

            var user = _fixture.GetByAuthenticationToken("logged in");
            _authenticationService.VerifyAll();
            _ministryPlatformService.VerifyAll();
            Assert.IsNotNull(user);
            Assert.AreEqual("me@here.com", user.UserId);
            Assert.AreEqual(mpResult[0]["User_GUID"].ToString(), user.Guid);
            Assert.IsTrue(user.CanImpersonate);
        }

    }
}
