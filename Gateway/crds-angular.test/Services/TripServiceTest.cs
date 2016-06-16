using System;
using System.Collections.Generic;
using System.Linq;
using crds_angular.Models.Crossroads.Serve;
using crds_angular.Services;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Services.Interfaces;
using Moq;
using NUnit.Framework;
using IDonationRepository = MinistryPlatform.Translation.Services.Interfaces.IDonationRepository;
using IDonorRepository = MinistryPlatform.Translation.Services.Interfaces.IDonorRepository;
using IEventRepository = MinistryPlatform.Translation.Services.Interfaces.IEventRepository;
using IGroupRepository = MinistryPlatform.Translation.Services.Interfaces.IGroupRepository;

namespace crds_angular.test.Services
{
    [TestFixture]
    public class TripServiceTest
    {
        private Mock<IEventParticipantRepository> _eventParticipantService;
        private Mock<IDonationRepository> _donationService;
        private Mock<IGroupRepository> _groupService;
        private Mock<IFormSubmissionRepository> _formSubmissionService;
        private Mock<IEventRepository> _eventService;
        private Mock<IDonorRepository> _donorService;
        private Mock<IPledgeRepository> _pledgeService;
        private Mock<ICampaignRepository> _campaignService;
        private Mock<IPrivateInviteRepository> _privateInviteService;
        private Mock<ICommunicationRepository> _communicationService;
        private Mock<IContactRepository> _contactService;
        private Mock<IContactRelationshipRepository> _contactRelationshipService;
        private Mock<IConfigurationWrapper> _configurationWrapper;
        private Mock<IPersonService> _personService;
        private Mock<IServeService> _serveService;
        private Mock<IDestinationRepository> _destinationService;
        private TripService _fixture;

        [SetUp]
        public void SetUp()
        {
            _eventParticipantService = new Mock<IEventParticipantRepository>();
            _donationService = new Mock<IDonationRepository>();
            _groupService = new Mock<IGroupRepository>();
            _formSubmissionService = new Mock<IFormSubmissionRepository>();
            _eventService = new Mock<IEventRepository>();
            _donorService = new Mock<IDonorRepository>();
            _pledgeService = new Mock<IPledgeRepository>();
            _campaignService = new Mock<ICampaignRepository>();
            _privateInviteService = new Mock<IPrivateInviteRepository>();
            _communicationService = new Mock<ICommunicationRepository>();
            _contactService = new Mock<IContactRepository>();
            _contactRelationshipService = new Mock<IContactRelationshipRepository>();
            _configurationWrapper = new Mock<IConfigurationWrapper>();
            _personService = new Mock<IPersonService>();
            _serveService = new Mock<IServeService>();
            _destinationService = new Mock<IDestinationRepository>();

            _fixture = new TripService(_eventParticipantService.Object,
                                       _donationService.Object,
                                       _groupService.Object,
                                       _formSubmissionService.Object,
                                       _eventService.Object,
                                       _donorService.Object,
                                       _pledgeService.Object,
                                       _campaignService.Object,
                                       _privateInviteService.Object,
                                       _communicationService.Object,
                                       _contactService.Object,
                                       _contactRelationshipService.Object,
                                       _configurationWrapper.Object,
                                       _personService.Object,
                                       _serveService.Object,
                                       _destinationService.Object);
        }

        [Test]
        public void Search()
        {
            var mockMpSearchResponse = MockMpSearchResponse();
            _eventParticipantService.Setup(m => m.TripParticipants(It.IsAny<string>())).Returns(mockMpSearchResponse);

            var mockPledge1 = new MpPledge
            {
                PledgeId = 1,
                DonorId = mockMpSearchResponse[0].DonorId,
                PledgeCampaignId = mockMpSearchResponse[0].CampaignId,
                PledgeStatusId = 1
            };
            var mockPledge2 = new MpPledge
            {
                PledgeId = 2,
                DonorId = mockMpSearchResponse[1].DonorId,
                PledgeCampaignId = mockMpSearchResponse[1].CampaignId,
                PledgeStatusId = 1
            };
            var mockPledge3 = new MpPledge
            {
                PledgeId = 3,
                DonorId = mockMpSearchResponse[2].DonorId,
                PledgeCampaignId = mockMpSearchResponse[2].CampaignId,
                PledgeStatusId = 1
            };
            _pledgeService.Setup(m => m.GetPledgeByCampaignAndDonor(mockMpSearchResponse[0].CampaignId, mockMpSearchResponse[0].DonorId)).Returns(mockPledge1);
            _pledgeService.Setup(m => m.GetPledgeByCampaignAndDonor(mockMpSearchResponse[1].CampaignId, mockMpSearchResponse[1].DonorId)).Returns(mockPledge2);
            _pledgeService.Setup(m => m.GetPledgeByCampaignAndDonor(mockMpSearchResponse[2].CampaignId, mockMpSearchResponse[2].DonorId)).Returns(mockPledge3);

            var searchResults = _fixture.Search(It.IsAny<string>());

            _eventParticipantService.VerifyAll();
            _pledgeService.VerifyAll();
            Assert.AreEqual(2, searchResults.Count);

            var p1 = searchResults.FirstOrDefault(s => s.ParticipantId == 9999);
            Assert.IsNotNull(p1);
            Assert.AreEqual(2, p1.Trips.Count);

            var p2 = searchResults.FirstOrDefault(s => s.ParticipantId == 5555);
            Assert.IsNotNull(p2);
            Assert.AreEqual(1, p2.Trips.Count);
        }

        [Test]
        public void ShouldGetMyTrips()
        {
            const string token = "faker";
            var mockFamily = new List<FamilyMember> { new FamilyMember { ContactId = 12345 }, new FamilyMember { ContactId = 98765 } };
            _serveService.Setup(m => m.GetImmediateFamilyParticipants(token)).Returns(mockFamily);

            _donationService.Setup(m => m.GetMyTripDistributions(It.IsAny<int>())).Returns(MockTripDonationsResponse());
            _eventParticipantService.Setup(m => m.TripParticipants(It.IsAny<string>())).Returns(mockTripParticipants());
            _pledgeService.Setup(m => m.GetPledgeByCampaignAndDonor(It.IsAny<int>(), It.IsAny<int>())).Returns(mockPledgeCampaign());
            var myTrips = _fixture.GetMyTrips(token);

            _serveService.VerifyAll();
            _donationService.VerifyAll();
            _eventParticipantService.VerifyAll();

            Assert.IsNotNull(myTrips);
            Assert.AreEqual(2, myTrips.MyTrips.Count);
            Assert.AreEqual(2, myTrips.MyTrips[0].TripGifts.Count);
        }

        [Test]
        public void FundraisingDaysLeftShouldNotBeNegative()
        {
            const string token = "faker";
            var mockFamily = new List<FamilyMember> { new FamilyMember { ContactId = 12345 } };
            _serveService.Setup(m => m.GetImmediateFamilyParticipants(token)).Returns(mockFamily);

            _donationService.Setup(m => m.GetMyTripDistributions(It.IsAny<int>())).Returns(MockFundingPastTripDonationsResponse());
            _eventParticipantService.Setup(m => m.TripParticipants(It.IsAny<string>())).Returns(mockTripParticipants());
            _pledgeService.Setup(m => m.GetPledgeByCampaignAndDonor(It.IsAny<int>(), It.IsAny<int>())).Returns(mockPledgeCampaign());
            var myTrips = _fixture.GetMyTrips(token);

            Assert.IsNotNull(myTrips);
            Assert.AreEqual(0, myTrips.MyTrips[0].FundraisingDaysLeft);
        }

        private MpPledge mockPledgeCampaign()
        {
            return new MpPledge
            {
                CampaignName = "",
                CampaignStartDate = DateTime.Now,
                CampaignEndDate = DateTime.Now,
                CampaignTypeId = 1,
                CampaignTypeName = "test",
                DonorId = 3,
                PledgeCampaignId = 1,
                PledgeDonations = 1,
                PledgeId = 1,
                PledgeStatus = "active",
                PledgeStatusId = 1,
                PledgeTotal = 100
            };
        }

        private List<MpTripDistribution> MockFundingPastTripDonationsResponse()
        {
            return new List<MpTripDistribution>
            {
                new MpTripDistribution
                {
                    ContactId = 1234,
                    EventTypeId = 6,
                    EventId = 8,
                    EventTitle = "GO Someplace",
                    EventStartDate = DateTime.Today,
                    EventEndDate = DateTime.Today,
                    TotalPledge = 1000,
                    CampaignStartDate = DateTime.Today.AddDays(-15),
                    CampaignEndDate = DateTime.Today.AddDays(-10),
                    DonorNickname = "John",
                    DonorFirstName = "John",
                    DonorLastName = "Donor",
                    DonorEmail = "crdsusertest+johndonor@gmail.com",
                    DonationDate = DateTime.Today,
                    DonationAmount = 350
                }
            };
        }

        private List<MpTripParticipant> mockTripParticipants()
        {
            return new List<MpTripParticipant>
            {
                new MpTripParticipant()
                {
                    EmailAddress = "myEmail@Address.com",
                    EventStartDate = new DateTime(2015, 10, 08),
                    EventEndDate = new DateTime(2015, 10, 23),
                    EventId = 8,
                    EventParticipantId = 21,
                    EventTitle = "Go Someplace",
                    EventType = "MissionTrip",
                    Lastname = "Name",
                    Nickname = "Funny",
                    ParticipantId = 213,
                    ProgramId = 2,
                    ProgramName = "Go Someplace",
                    CampaignId = 1,
                    DonorId = 3
                }
            };
        }

        private List<MpTripDistribution> MockTripDonationsResponse()
        {
            return new List<MpTripDistribution>
            {
                new MpTripDistribution
                {
                    ContactId = 1234,
                    EventTypeId = 6,
                    EventId = 8,
                    EventTitle = "GO Someplace",
                    EventStartDate = DateTime.Today,
                    EventEndDate = DateTime.Today,
                    TotalPledge = 1000,
                    CampaignStartDate = DateTime.Today,
                    CampaignEndDate = DateTime.Today,
                    DonorNickname = "John",
                    DonorFirstName = "John",
                    DonorLastName = "Donor",
                    DonorEmail = "crdsusertest+johndonor@gmail.com",
                    DonationDate = DateTime.Today,
                    DonationAmount = 350
                },
                new MpTripDistribution
                {
                    ContactId = 1234,
                    EventTypeId = 6,
                    EventId = 8,
                    EventTitle = "Go Someplace",
                    EventStartDate = DateTime.Today,
                    EventEndDate = DateTime.Today,
                    TotalPledge = 1000,
                    CampaignStartDate = DateTime.Today,
                    CampaignEndDate = DateTime.Today,
                    DonorNickname = "John",
                    DonorFirstName = "John",
                    DonorLastName = "Donor",
                    DonorEmail = "crdsusertest+johndonor@gmail.com",
                    DonationDate = DateTime.Today,
                    DonationAmount = 200
                }
            };
        }

        private static List<MpTripParticipant> MockMpSearchResponse()
        {
            return new List<MpTripParticipant>
            {
                new MpTripParticipant
                {
                    EmailAddress = "test@aol.com",
                    EventEndDate = new DateTime(2015, 7, 1),
                    EventId = 1,
                    EventParticipantId = 7777,
                    EventStartDate = new DateTime(2015, 7, 1),
                    EventTitle = "Test Trip 1",
                    EventType = "Go Trip",
                    Lastname = "Subject",
                    Nickname = "Test",
                    ParticipantId = 9999,
                    CampaignId = 1,
                    DonorId = 1
                },
                new MpTripParticipant
                {
                    EmailAddress = "test@aol.com",
                    EventEndDate = new DateTime(2015, 8, 1),
                    EventId = 2,
                    EventParticipantId = 888,
                    EventStartDate = new DateTime(2015, 8, 1),
                    EventTitle = "Test Trip 2",
                    EventType = "Go Trip",
                    Lastname = "Subject",
                    Nickname = "Test",
                    ParticipantId = 9999,
                    CampaignId = 2,
                    DonorId = 2
                },
                new MpTripParticipant
                {
                    EmailAddress = "spec@aol.com",
                    EventEndDate = new DateTime(2015, 7, 1),
                    EventId = 1,
                    EventParticipantId = 4444,
                    EventStartDate = new DateTime(2015, 7, 1),
                    EventTitle = "Test Trip 1",
                    EventType = "Go Trip",
                    Lastname = "Dummy",
                    Nickname = "Crash",
                    ParticipantId = 5555,
                    CampaignId = 3,
                    DonorId = 3
                }
            };
        }
    }
}