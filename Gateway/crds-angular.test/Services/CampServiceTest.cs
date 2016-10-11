using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using crds_angular.Services;
using MinistryPlatform.Translation.Models;
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
        private Mock<IApiUserRepository> _apiUserRepository;

        private CampService _fixture;

        [SetUp]
        public void SetUp()
        {
            _contactService = new Mock<IContactRepository>();
            _campService = new Mock<ICampRepository>();
            _apiUserRepository = new Mock<IApiUserRepository>();

            _fixture = new CampService(_contactService.Object, _campService.Object);
        }

        [Test]

        public void ShouldGetCampEventDetails()
        {
            const string token = "asdfasdf";
            int eventId = 123;

            var campEventDTO = new List<MpCampEvent>
            {
                
            }
            _apiUserRepository.Setup(m => m.GetToken()).Returns(token);
            _campService.Setup(M => M.GetCampEventDetails(eventId)).Returns(campEventDTO);

        }

        private List<MpCampEvent> campEvents()
        {
            return new List<MpCampEvent>
            {
                new MpCampEvent()
                {
                    EventId = 123,
                    EventTitle = "Camp Event",
                    EventType = "camp",
                    OnlineProductId = 156,
                    ProgramId = 555,
                    RegistrationStartDate = DateTime.Today.AddDays(-3),
                    RegistrationEndDate = DateTime.Today.AddDays(90),
                    StartDate = DateTime.Today.AddDays(150),
                    EndDate = DateTime.Today.AddDays(155)                 
                }
            };
        }
    }
}
