using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Results;
using crds_angular.Controllers.API;
using crds_angular.Exceptions;
using crds_angular.Models.Crossroads;
using crds_angular.Services.Interfaces;
using MinistryPlatform.Translation.Repositories.Interfaces;
using Moq;
using NUnit.Framework;

namespace crds_angular.test.controllers
{
    public class UserControllerTest
    {
        private UserController _fixture;

        private Mock<IAccountService> _accountService;
        private Mock<IUserRepository> _userRepository;

        [SetUp]
        public void SetUp()
        {
            _accountService = new Mock<IAccountService>();
            _userRepository = new Mock<IUserRepository>();

            _fixture = new UserController(_accountService.Object, _userRepository.Object);
        }

        [Test]
        public void ShouldRegisterNewUser()
        {
            var user = new User();
            _accountService.Setup(mocked => mocked.RegisterPerson(user)).Returns(user);

            var response = _fixture.Post(user);
            _accountService.VerifyAll();

            Assert.IsNotNull(response);
            Assert.IsInstanceOf<OkNegotiatedContentResult<User>>(response);
            var responseData = ((OkNegotiatedContentResult<User>) response).Content;
            Assert.IsNotNull(responseData);
            Assert.AreSame(user, responseData);
        }

        [Test]
        public void ShouldReturnBadResponseForDuplicateUser()
        {
            var user = new User();
            _accountService.Setup(mocked => mocked.RegisterPerson(user)).Throws(new DuplicateUserException("me@here.com"));

            try
            {
                _fixture.Post(user);
                Assert.Fail("Expected exception was not thrown");
            }
            catch (HttpResponseException e)
            {
                // Expected
            }
            _accountService.VerifyAll();
        }
    }
}
