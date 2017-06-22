﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using crds_angular.Controllers.API;
using crds_angular.Services.Interfaces;
using Crossroads.ClientApiKeys;
using Crossroads.Web.Common.MinistryPlatform;
using Crossroads.Web.Common.Security;
using MinistryPlatform.Translation.Repositories.Interfaces;
using Moq;
using NUnit.Framework;

namespace crds_angular.test.controllers
{
    public class ImageControllerTest
    {
        private ImageController _fixture;
        private Mock<IMinistryPlatformService> _mpService;
        private Mock<IAuthenticationRepository> _authenticationRepository;
        private Mock<IApiUserRepository> _apiUserRepository;
        private Mock<IUserImpersonationService> _userImpersonationService;

        private List<Type> _attributesThatRequireApiKey;

        [SetUp]
        public void SetUp()
        {
            _attributesThatRequireApiKey = new List<Type>
            {
                typeof(HttpPostAttribute),
                typeof(HttpPutAttribute),
                typeof(HttpDeleteAttribute)
            };

            _mpService = new Mock<IMinistryPlatformService>();
            _authenticationRepository = new Mock<IAuthenticationRepository>();
            _apiUserRepository = new Mock<IApiUserRepository>();
            _userImpersonationService = new Mock<IUserImpersonationService>();

            _fixture = new ImageController(_mpService.Object, _authenticationRepository.Object, _apiUserRepository.Object, _userImpersonationService.Object);
        }

        [Test]
        public void EndpointMethodsThatOnlyReadImagesShouldIgnoreClientApiKey()
        {
            var publicMethods = _fixture.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            publicMethods.ToList().ForEach(m =>
            {
                if (!MethodHasHttpUpdateAttribute(m))
                {
                    Assert.IsNotNull(m.GetCustomAttribute<IgnoreClientApiKeyAttribute>(), $"Method {m.Name} should have [IgnoreClientApiKey] attribute, as it is only reading image information");
                }
            });
        }

        [Test]
        public void EndpointMethodsThatCreateOrUpdateImagesShouldNotIgnoreClientApiKey()
        {
            var publicMethods = _fixture.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            publicMethods.ToList().ForEach(m =>
            {
                if (MethodHasHttpUpdateAttribute(m))
                {
                    Assert.IsNull(m.GetCustomAttribute<IgnoreClientApiKeyAttribute>(), $"Method {m.Name} should not have [IgnoreClientApiKey] attribute, as it is updating image information");
                }
            });
        }

        private bool MethodHasHttpUpdateAttribute(MethodInfo m)
        {
            return m.GetCustomAttributes().ToList().Exists(attr => _attributesThatRequireApiKey.Contains(attr.GetType()));
        }

    }
}
