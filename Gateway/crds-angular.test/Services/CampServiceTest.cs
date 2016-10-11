using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using crds_angular.Services;
using MinistryPlatform.Translation.Repositories.Interfaces;
using Moq;
using NUnit.Framework;

namespace crds_angular.test.Services
{
    [TestFixture]
    public class CampServiceTest
    {
        private Mock<IContactRepository> _contactService;
        private Mock<ICampRepository> _campService;

        private CampService _fixture;

        [SetUp]
        public void SetUp()
        {
            _contactService = new Mock<IContactRepository>();
            _campService = new Mock<ICampRepository>();

            _fixture = new CampService(_contactService.Object, _campService.Object);
        }

        [Test]

        public void ShouldGetCampEventDetails()
        {

            
        }
    }
}
