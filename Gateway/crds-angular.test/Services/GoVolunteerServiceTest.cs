using System;
using System.Collections.Generic;
using System.Linq;
using crds_angular.Exceptions;
using crds_angular.Models.Crossroads.GoVolunteer;
using crds_angular.Services;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.FunctionalHelpers;
using Crossroads.Utilities.Interfaces;
using Crossroads.Utilities.Services;
using Crossroads.Web.Common.Configuration;
using Crossroads.Web.Common.MinistryPlatform;
using FsCheck.Experimental;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Models.GoCincinnati;
using MinistryPlatform.Translation.Repositories.Interfaces;
using MinistryPlatform.Translation.Repositories.Interfaces.GoCincinnati;
using Moq;
using NUnit.Framework;
using IAttributeService = crds_angular.Services.Interfaces.IAttributeService;
using IGroupConnectorRepository = MinistryPlatform.Translation.Repositories.Interfaces.GoCincinnati.IGroupConnectorRepository;

namespace crds_angular.test.Services
{
    [TestFixture]
    public class GoVolunteerServiceTest
    {
        private readonly GoVolunteerService _fixture;

        private readonly Mock<IAttributeService> _attributeService;
        private readonly Mock<IConfigurationWrapper> _configurationWrapper;
        private readonly Mock<IContactRelationshipRepository> _contactRelationshipService;
        private readonly Mock<IContactRepository> _contactService;
        private readonly Mock<IGroupConnectorRepository> _groupConnectorService;
        private readonly Mock<IParticipantRepository> _participantService;
        private readonly Mock<IProjectTypeRepository> _projectTypeService;
        private readonly Mock<IRegistrationRepository> _registrationService;
        private readonly Mock<IGoSkillsService> _skillsService;
        private readonly Mock<ICommunicationRepository> _commnuicationService;
        private readonly Mock<IUserRepository> _userService;
        private readonly Mock<IApiUserRepository> _apiUserRepository;
        private readonly Mock<IProjectRepository> _projectRepository;

        private readonly int CrossroadsOrganizationId = 2;

        public GoVolunteerServiceTest()
        {
            _attributeService = new Mock<IAttributeService>();
            _commnuicationService = new Mock<ICommunicationRepository>();
            _configurationWrapper = new Mock<IConfigurationWrapper>();
            _contactRelationshipService = new Mock<IContactRelationshipRepository>();
            _contactService = new Mock<IContactRepository>();
            _groupConnectorService = new Mock<IGroupConnectorRepository>();
            _participantService = new Mock<IParticipantRepository>();
            _projectTypeService = new Mock<IProjectTypeRepository>();
            _registrationService = new Mock<IRegistrationRepository>();
            _skillsService = new Mock<IGoSkillsService>();
            _userService = new Mock<IUserRepository>();
            _apiUserRepository = new Mock<IApiUserRepository>();
            _projectRepository = new Mock<IProjectRepository>();
            _fixture = new GoVolunteerService(_participantService.Object, 
                _registrationService.Object, 
                _contactService.Object, 
                _groupConnectorService.Object, 
                _configurationWrapper.Object, 
                _contactRelationshipService.Object, 
                _projectTypeService.Object, 
                _attributeService.Object, 
                _skillsService.Object,
                _commnuicationService.Object,
                _userService.Object,
                _apiUserRepository.Object,
                _projectRepository.Object);
        }

        [Test]
        public void ShouldSendEmailOnlyToVolunteer()
        {
            const int templateId = 123456789;
            const int fromContactId = 0987;                
            var fromContact = TestHelpers.MyContact(fromContactId);
            var registration = TestHelpers.RegistrationNoSpouse();
            var contactFromRegistration = TestHelpers.ContactFromRegistrant(registration.Self);
            var communication = TestHelpers.Communication(fromContact, contactFromRegistration, templateId);

            _configurationWrapper.Setup(m => m.GetConfigIntValue("GoVolunteerEmailTemplate")).Returns(templateId);
            _configurationWrapper.Setup(m => m.GetConfigIntValue("GoVolunteerEmailFromContactId")).Returns(fromContactId);
            _contactService.Setup(m => m.GetContactById(fromContactId)).Returns(fromContact);
            _commnuicationService.Setup(m => m.GetTemplateAsCommunication(templateId,
                                                                            fromContactId,
                                                                            fromContact.Email_Address,
                                                                            fromContact.Contact_ID,
                                                                            fromContact.Email_Address,
                                                                            registration.Self.ContactId,
                                                                            registration.Self.EmailAddress,
                                                                            It.IsAny<Dictionary<string, object>>())).Returns(communication);
            _commnuicationService.Setup(m => m.SendMessage(communication, false)).Returns(1);
            var success = _fixture.SendMail(registration);
            _commnuicationService.Verify();           
            Assert.IsTrue(success);                            
        }

        [Test]
        public void ShouldSendEmailToVolunteerAndSpouse()
        {

            const int templateId = 123456789;
            const int spouseTemplateId = 98765432;

            const int fromContactId = 0987;
            var fromContact = TestHelpers.MyContact(fromContactId);
            var registration = TestHelpers.RegistrationWithSpouse();
            var contactFromRegistration = TestHelpers.ContactFromRegistrant(registration.Self);
            var spouseFromRegistration = TestHelpers.ContactFromRegistrant(registration.Spouse);

            var communication = TestHelpers.Communication(fromContact, contactFromRegistration, templateId);
            var spouseCommunication = TestHelpers.Communication(fromContact, spouseFromRegistration, spouseTemplateId);

            _configurationWrapper.Setup(m => m.GetConfigIntValue("GoVolunteerEmailTemplate")).Returns(templateId);
                

            _configurationWrapper.Setup(m => m.GetConfigIntValue("GoVolunteerEmailFromContactId")).Returns(fromContactId);
            _contactService.Setup(m => m.GetContactById(fromContactId)).Returns(fromContact);

            _commnuicationService.Setup(m => m.GetTemplateAsCommunication(templateId,
                                                                            fromContactId,
                                                                            fromContact.Email_Address,
                                                                            fromContact.Contact_ID,
                                                                            fromContact.Email_Address,
                                                                            registration.Self.ContactId,
                                                                            registration.Self.EmailAddress,
                                                                            It.IsAny<Dictionary<string, object>>())).Returns(communication);
            _configurationWrapper.Setup(m => m.GetConfigIntValue("GoVolunteerEmailSpouseTemplate")).Returns(spouseTemplateId);
            _commnuicationService.Setup(m => m.GetTemplateAsCommunication(spouseTemplateId,
                                                                            fromContactId,
                                                                            fromContact.Email_Address,
                                                                            fromContact.Contact_ID,
                                                                            fromContact.Email_Address,
                                                                            registration.Spouse.ContactId,
                                                                            registration.Spouse.EmailAddress,
                                                                            It.IsAny<Dictionary<string, object>>())).Returns(spouseCommunication);
            _commnuicationService.Setup(m => m.SendMessage(spouseCommunication, false)).Returns(1);            
            _commnuicationService.Setup(m => m.SendMessage(communication, false)).Returns(1);
               
                
            var success = _fixture.SendMail(registration);                
            _configurationWrapper.VerifyAll();           
            Assert.IsTrue(success);            
        }

        [Test]
        public void ShouldSetupMergeDataGroupConnector()
        {
            var registration = TestHelpers.RegistrationWithGroupConnector();

            _configurationWrapper.Setup(m => m.GetConfigIntValue("Children2To7")).Returns(registration.ChildAgeGroup[0].Id);
            _configurationWrapper.Setup(m => m.GetConfigIntValue("Children8To12")).Returns(registration.ChildAgeGroup[1].Id);
            _configurationWrapper.Setup(m => m.GetConfigIntValue("Children13To18")).Returns(registration.ChildAgeGroup[2].Id);

            var mergeData = _fixture.SetupMergeData(registration);

            _configurationWrapper.VerifyAll();

            var styles = Styles();

            var listOfP = new List<HtmlElement>
            {
                BuildParagraph("Name: ", registration.Self.FirstName + " " + registration.Self.LastName),
                BuildParagraph("Email: ", registration.Self.EmailAddress),
                BuildParagraph("Birthdate: ", "2/21/1980"),
                BuildParagraph("Mobile Phone: ", registration.Self.MobilePhone),
                BuildParagraph("Number of Children Ages 2-7: ", registration.ChildAgeGroup[0].Count.ToString()),
                new HtmlElement("p"),
                BuildParagraph("Number of Children Ages 13-18: ", registration.ChildAgeGroup[2].Count.ToString()),
                BuildParagraph("Group Connector: ", registration.GroupConnector.Name),
                BuildParagraph("Preferred Launch Site: ", registration.GroupConnector.PreferredLaunchSite),                                
                BuildParagraph("Special Equipment: ", registration.Equipment.Select(equipment => equipment.Notes).Aggregate((first, next) => first + ", " + next)),
                BuildParagraph("Additional Info: ", registration.AdditionalInformation),
                BuildParagraph("Available for Prep Work: ", "Yes, from " + registration.PrepWork[0].Name),
                BuildParagraph("Spouse Available for Prep Work: ", "No")
            };

            var htmlCell = new HtmlElement("td", styles).Append(listOfP);
            var htmlRow = new HtmlElement("tr", styles).Append(htmlCell);
            var htmlTBody = new HtmlElement("tbody", styles).Append(htmlRow);

            var htmlTable = new HtmlElement("table", styles).Append(htmlTBody);

            var dict = new Dictionary<string, object>()
            {
                {"HTML_TABLE", htmlTable.Build()},
                {"Nickname", registration.Self.FirstName},
                {"Lastname", registration.Self.LastName}
            };

            Assert.AreEqual(dict.Count, mergeData.Count);
            Assert.AreEqual(dict["HTML_TABLE"], mergeData["HTML_TABLE"]);
            Assert.AreEqual(dict["Nickname"], mergeData["Nickname"]);
            Assert.AreEqual(dict["Lastname"], mergeData["Lastname"]);   
        }

        [Test]
        public void ShouldSetupMergeDataWithoutSpouse()
        {
            var registration = TestHelpers.RegistrationNoSpouse();

            _configurationWrapper.Setup(m => m.GetConfigIntValue("Children2To7")).Returns(registration.ChildAgeGroup[0].Id);
            _configurationWrapper.Setup(m => m.GetConfigIntValue("Children8To12")).Returns(registration.ChildAgeGroup[1].Id);
            _configurationWrapper.Setup(m => m.GetConfigIntValue("Children13To18")).Returns(registration.ChildAgeGroup[2].Id);

            var mergeData = _fixture.SetupMergeData(registration);

            _configurationWrapper.VerifyAll();

            var styles = Styles();

            var listOfP = new List<HtmlElement>
            {
                BuildParagraph("Name: ", registration.Self.FirstName + " " + registration.Self.LastName),
                BuildParagraph("Email: ", registration.Self.EmailAddress),
                BuildParagraph("Birthdate: ",  "2/21/1980"),
                BuildParagraph("Mobile Phone: ", registration.Self.MobilePhone),
                BuildParagraph("Number of Children Ages 2-7: ", registration.ChildAgeGroup[0].Count.ToString()),
                new HtmlElement("p"),
                BuildParagraph("Number of Children Ages 13-18: ", registration.ChildAgeGroup[2].Count.ToString()),
                BuildParagraph("Preferred Launch Site: ", registration.PreferredLaunchSite.Name), // need to get the site name...
                BuildParagraph("Project Preference 1: ", registration.ProjectPreferences[0].Name),
                BuildParagraph("Project Preference 2: ", registration.ProjectPreferences[1].Name),
                BuildParagraph("Project Preference 3: ", registration.ProjectPreferences[2].Name),
                BuildParagraph("Special Equipment: ", registration.Equipment.Select(equipment => equipment.Notes).Aggregate((first, next) => first + ", " + next)),
                BuildParagraph("Additional Info: ", registration.AdditionalInformation),
                BuildParagraph("Available for Prep Work: ", "Yes, from " + registration.PrepWork[0].Name),
                BuildParagraph("Spouse Available for Prep Work: ", "No")
            };

            var htmlCell = new HtmlElement("td", styles).Append(listOfP);
            var htmlRow = new HtmlElement("tr", styles).Append(htmlCell);
            var htmlTBody = new HtmlElement("tbody", styles).Append(htmlRow);

            var htmlTable = new HtmlElement("table", styles).Append(htmlTBody);

            var dict = new Dictionary<string, object>()
            {
                {"HTML_TABLE", htmlTable.Build()},
                {"Nickname", registration.Self.FirstName},
                {"Lastname", registration.Self.LastName}
            };
            Assert.AreEqual(dict.Count, mergeData.Count);
            Assert.AreEqual(dict["HTML_TABLE"], mergeData["HTML_TABLE"]);
            Assert.AreEqual(dict["Nickname"], mergeData["Nickname"]);
            Assert.AreEqual(dict["Lastname"], mergeData["Lastname"]);
        }

        [Test]
        public void ShouldSetupMergeDataWithSpouse()
        {
            var registration = TestHelpers.RegistrationWithSpouse();

            _configurationWrapper.Setup(m => m.GetConfigIntValue("Children2To7")).Returns(registration.ChildAgeGroup[0].Id);
            _configurationWrapper.Setup(m => m.GetConfigIntValue("Children8To12")).Returns(registration.ChildAgeGroup[1].Id);
            _configurationWrapper.Setup(m => m.GetConfigIntValue("Children13To18")).Returns(registration.ChildAgeGroup[2].Id);


            var mergeData = _fixture.SetupMergeData(registration);

            var styles = Styles();

            var listOfP = new List<HtmlElement>
            {
                BuildParagraph("Name: ", registration.Self.FirstName + " " + registration.Self.LastName),
                BuildParagraph("Email: ", registration.Self.EmailAddress),
                BuildParagraph("Birthdate: ",  "2/21/1980"),
                BuildParagraph("Mobile Phone: ", registration.Self.MobilePhone),
                BuildParagraph("Spouse Name: ", registration.Spouse.FirstName + " " + registration.Spouse.LastName),
                BuildParagraph("Spouse Email: ", registration.Spouse.EmailAddress),
                BuildParagraph("Spouse Birthdate: ",  "2/21/1980"),
                BuildParagraph("Spouse Mobile Phone: ", registration.Spouse.MobilePhone),
                BuildParagraph("Number of Children Ages 2-7: ", registration.ChildAgeGroup[0].Count.ToString()),
                new HtmlElement("p"),
                BuildParagraph("Number of Children Ages 13-18: ", registration.ChildAgeGroup[2].Count.ToString()),
                BuildParagraph("Preferred Launch Site: ", registration.PreferredLaunchSite.Name), // need to get the site name...
                BuildParagraph("Project Preference 1: ", registration.ProjectPreferences[0].Name),
                BuildParagraph("Project Preference 2: ", registration.ProjectPreferences[1].Name),
                BuildParagraph("Project Preference 3: ", registration.ProjectPreferences[2].Name),
                BuildParagraph("Special Equipment: ", registration.Equipment.Select(equipment => equipment.Notes).Aggregate((first, next) => first + ", " + next)),
                BuildParagraph("Additional Info: ", registration.AdditionalInformation),
                BuildParagraph("Available for Prep Work: ", "Yes, from " + registration.PrepWork[0].Name),
                BuildParagraph("Spouse Available for Prep Work: ", "No")
            };

            var htmlCell = new HtmlElement("td", styles).Append(listOfP);
            var htmlRow = new HtmlElement("tr", styles).Append(htmlCell);
            var htmlTBody = new HtmlElement("tbody", styles).Append(htmlRow);

            var htmlTable = new HtmlElement("table", styles).Append(htmlTBody);

            var dict = new Dictionary<string, object>()
            {
                {"HTML_TABLE", htmlTable.Build()},
                {"Nickname", registration.Self.FirstName},
                {"Lastname", registration.Self.LastName},
                {"Spouse_Nickname", registration.Spouse.FirstName },
                {"Spouse_Lastname", registration.Spouse.LastName }
            };

            Assert.AreEqual(dict.Count, mergeData.Count);
            Assert.AreEqual(dict["HTML_TABLE"], mergeData["HTML_TABLE"]);
            Assert.AreEqual(dict["Nickname"], mergeData["Nickname"]);
            Assert.AreEqual(dict["Lastname"], mergeData["Lastname"]);
            Assert.AreEqual(dict["Spouse_Nickname"], mergeData["Spouse_Nickname"]);
            Assert.AreEqual(dict["Spouse_Lastname"], mergeData["Spouse_Lastname"]);
        }

        [Test]
        public void ShouldSetupDataWithLimitedSpouseInfo()
        {
            var registration = TestHelpers.RegistrationWithSpouseLimited();


            _configurationWrapper.Setup(m => m.GetConfigIntValue("Children2To7")).Returns(registration.ChildAgeGroup[0].Id);
            _configurationWrapper.Setup(m => m.GetConfigIntValue("Children8To12")).Returns(registration.ChildAgeGroup[1].Id);
            _configurationWrapper.Setup(m => m.GetConfigIntValue("Children13To18")).Returns(registration.ChildAgeGroup[2].Id);


            var mergeData = _fixture.SetupMergeData(registration);

            var styles = Styles();

            var listOfP = new List<HtmlElement>
            {
                BuildParagraph("Name: ", registration.Self.FirstName + " " + registration.Self.LastName),
                BuildParagraph("Email: ", registration.Self.EmailAddress),
                BuildParagraph("Birthdate: ",  "2/21/1980"),
                BuildParagraph("Mobile Phone: ", registration.Self.MobilePhone),
                BuildParagraph("Spouse Name: ", registration.Spouse.FirstName + " " + registration.Spouse.LastName),
                BuildParagraph("Number of Children Ages 2-7: ", registration.ChildAgeGroup[0].Count.ToString()),
                new HtmlElement("p"),
                BuildParagraph("Number of Children Ages 13-18: ", registration.ChildAgeGroup[2].Count.ToString()),
                BuildParagraph("Preferred Launch Site: ", registration.PreferredLaunchSite.Name), // need to get the site name...
                BuildParagraph("Project Preference 1: ", registration.ProjectPreferences[0].Name),
                BuildParagraph("Project Preference 2: ", registration.ProjectPreferences[1].Name),
                BuildParagraph("Project Preference 3: ", registration.ProjectPreferences[2].Name),
                BuildParagraph("Special Equipment: ", registration.Equipment.Select(equipment => equipment.Notes).Aggregate((first, next) => first + ", " + next)),
                BuildParagraph("Additional Info: ", registration.AdditionalInformation),
                BuildParagraph("Available for Prep Work: ", "Yes, from " + registration.PrepWork[0].Name),
                BuildParagraph("Spouse Available for Prep Work: ", "No")
            };

            var htmlCell = new HtmlElement("td", styles).Append(listOfP);
            var htmlRow = new HtmlElement("tr", styles).Append(htmlCell);
            var htmlTBody = new HtmlElement("tbody", styles).Append(htmlRow);

            var htmlTable = new HtmlElement("table", styles).Append(htmlTBody);

            var dict = new Dictionary<string, object>()
            {
                {"HTML_TABLE", htmlTable.Build()},
                {"Nickname", registration.Self.FirstName},
                {"Lastname", registration.Self.LastName},
                {"Spouse_Nickname", registration.Spouse.FirstName },
                {"Spouse_Lastname", registration.Spouse.LastName }
            };

            Assert.AreEqual(dict.Count, mergeData.Count);
            Assert.AreEqual(dict["HTML_TABLE"], mergeData["HTML_TABLE"]);
            Assert.AreEqual(dict["Nickname"], mergeData["Nickname"]);
            Assert.AreEqual(dict["Lastname"], mergeData["Lastname"]);
            Assert.AreEqual(dict["Spouse_Nickname"], mergeData["Spouse_Nickname"]);
            Assert.AreEqual(dict["Spouse_Lastname"], mergeData["Spouse_Lastname"]);
        }

        [Test]
        public void ShouldSetupMergeDataWithSkills()
        {
            var registration = TestHelpers.RegistrationWithSkills();

            _configurationWrapper.Setup(m => m.GetConfigIntValue("Children2To7")).Returns(registration.ChildAgeGroup[0].Id);
            _configurationWrapper.Setup(m => m.GetConfigIntValue("Children8To12")).Returns(registration.ChildAgeGroup[1].Id);
            _configurationWrapper.Setup(m => m.GetConfigIntValue("Children13To18")).Returns(registration.ChildAgeGroup[2].Id);

            var mergeData = _fixture.SetupMergeData(registration);

            _configurationWrapper.VerifyAll();

            var styles = Styles();

            var listOfP = new List<HtmlElement>
            {
                BuildParagraph("Name: ", registration.Self.FirstName + " " + registration.Self.LastName),
                BuildParagraph("Email: ", registration.Self.EmailAddress),
                BuildParagraph("Birthdate: ", "2/21/1980"),
                BuildParagraph("Mobile Phone: ", registration.Self.MobilePhone),
                BuildParagraph("Number of Children Ages 2-7: ", registration.ChildAgeGroup[0].Count.ToString()),
                new HtmlElement("p"),
                BuildParagraph("Number of Children Ages 13-18: ", registration.ChildAgeGroup[2].Count.ToString()),
                BuildParagraph("Preferred Launch Site: ", registration.PreferredLaunchSite.Name), // need to get the site name...
                BuildParagraph("Project Preference 1: ", registration.ProjectPreferences[0].Name),
                BuildParagraph("Project Preference 2: ", registration.ProjectPreferences[1].Name),
                BuildParagraph("Project Preference 3: ", registration.ProjectPreferences[2].Name)
            };
            var skills = Skills(registration);
            if (skills != String.Empty)
            {
                listOfP.Add(BuildParagraph("Unique Skills: ", Skills(registration)));
            }
            listOfP.Add(BuildParagraph("Special Equipment: ", registration.Equipment.Select(equipment => equipment.Notes).Aggregate((first, next) => first + ", " + next)));
            listOfP.Add(BuildParagraph("Additional Info: ", registration.AdditionalInformation));           
            listOfP.Add(BuildParagraph("Available for Prep Work: ", "Yes, from " + registration.PrepWork[0].Name));
            listOfP.Add(BuildParagraph("Spouse Available for Prep Work: ", "No"));
            

            var htmlCell = new HtmlElement("td", styles).Append(listOfP);
            var htmlRow = new HtmlElement("tr", styles).Append(htmlCell);
            var htmlTBody = new HtmlElement("tbody", styles).Append(htmlRow);
            var htmlTable = new HtmlElement("table", styles).Append(htmlTBody);

            var dict = new Dictionary<string, object>()
            {
                {"HTML_TABLE", htmlTable.Build()},
                {"Nickname", registration.Self.FirstName},
                {"Lastname", registration.Self.LastName}
            };
            Assert.AreEqual(dict.Count, mergeData.Count);
            Assert.AreEqual(dict["HTML_TABLE"], mergeData["HTML_TABLE"]);
            Assert.AreEqual(dict["Nickname"], mergeData["Nickname"]);
            Assert.AreEqual(dict["Lastname"], mergeData["Lastname"]);

        }

        [Test]
        public void ShouldGetListOfParticipatingCities()
        {
            const int initiativeId = 12;

            var mockCities = MockCityList();
            _projectRepository.Setup(m => m.GetProjectsByInitiative(initiativeId, It.IsAny<string>())).Returns(mockCities);

            var result = _fixture.GetParticipatingCities(initiativeId);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
            Assert.AreEqual(mockCities.Count, result.Count);
            _projectRepository.VerifyAll();
        }

        private List<MpProject> MockCityList()
        {
            return new List<MpProject>
            {
                new MpProject
                {
                    ProjectId = 1,
                    City = "Cleveland",
                    State = "OH"
                },
                new MpProject
                {
                    ProjectId = 2,
                    City = "Phoenix",
                    State = "AZ"
                }
            };
        }

        [Test]
        public void ShouldGetProjectDetails()
        {
            const int projectId = 564;
            const string apiToken = "clevelandsux";

            var mpProject = new MpProject
            {
                AddressId = 1,
                InitiativeId = 2,
                LocationId = 3,
                OrganizationId = 4,
                ProjectId = projectId,
                ProjectStatusId = 5,
                ProjectTypeId = 6,
                ProjectName = "Make Cleveland Great (Again?)",
                City = "Cleveland",
                State = "OH"
            };

            var mpGroupConnector = new MpGroupConnector
            {
                PrimaryContactId = 234,
                PrimaryContactEmail = "me@mail.com",
                PrimaryContactFirstName = "Drew",
                PrimaryContactLastName = "Carey",
            };

            var returnVal = new Ok<MpProject>(mpProject);
            var groupConnectorReturn = new Ok<MpGroupConnector>(mpGroupConnector);

            _apiUserRepository.Setup(m => m.GetToken()).Returns(apiToken);
            _projectRepository.Setup(m => m.GetProject(projectId, apiToken)).Returns(returnVal);
            _projectRepository.Setup(m => m.GetGroupConnector(projectId, apiToken)).Returns(groupConnectorReturn);

            var project = _fixture.GetProject(projectId);
            Assert.AreEqual(mpProject.AddressId, project.AddressId);
            Assert.AreEqual(mpProject.ProjectId, project.ProjectId);
            Assert.AreEqual("Cleveland, OH", project.Location);
            Assert.AreEqual("Drew Carey", project.ContactDisplayName);
            _apiUserRepository.VerifyAll();
            _projectRepository.VerifyAll();
        }

        [Test]
        public void ShouldGetProjectDetailsAndUseNickname()
        {
            const int projectId = 564;
            const string apiToken = "clevelandsux";

            var mpProject = new MpProject
            {
                AddressId = 1,
                InitiativeId = 2,
                LocationId = 3,
                OrganizationId = 4,
                ProjectId = projectId,
                ProjectStatusId = 5,
                ProjectTypeId = 6,
                ProjectName = "Make Cleveland Great (Again?)",
                City = "Cleveland",
                State = "OH"
            };

            var mpGroupConnector = new MpGroupConnector
            {
                PrimaryContactId = 234,
                PrimaryContactEmail = "me@mail.com",
                PrimaryContactFirstName = "Drew",
                PrimaryContactLastName = "Carey",
                PrimaryContactNickname = "D"
            };

            var returnVal = new Ok<MpProject>(mpProject);
            var groupConnectorReturn = new Ok<MpGroupConnector>(mpGroupConnector);

            _apiUserRepository.Setup(m => m.GetToken()).Returns(apiToken);
            _projectRepository.Setup(m => m.GetProject(projectId, apiToken)).Returns(returnVal);
            _projectRepository.Setup(m => m.GetGroupConnector(projectId, apiToken)).Returns(groupConnectorReturn);

            var project = _fixture.GetProject(projectId);
            Assert.AreEqual(mpProject.AddressId, project.AddressId);
            Assert.AreEqual(mpProject.ProjectId, project.ProjectId);
            Assert.AreEqual("Cleveland, OH", project.Location);
            Assert.AreEqual("D Carey", project.ContactDisplayName);
            _apiUserRepository.VerifyAll();
            _projectRepository.VerifyAll();
        }

        [Test]
        public void ShouldThrowExceptionIfProjectNotFound()
        {
            const int projectId = 564;
            const string apiToken = "clevelandsux";
            
            var returnVal = new Err<MpProject>("Project not found!");

            _apiUserRepository.Setup(m => m.GetToken()).Returns(apiToken);
            _projectRepository.Setup(m => m.GetProject(projectId, apiToken)).Returns(returnVal);

            Assert.Throws<ApplicationException>(() =>
            {
                _fixture.GetProject(projectId);
                _apiUserRepository.VerifyAll();
                _projectRepository.VerifyAll();
            });
        }

        [Test]
        public void ShouldSaveAnywhereRegistration()
        {
            const int projectId = 564;
            const string token = "asdf";
            const int groupConnectorId = 1324;
            const int participantId = 9876543;
            const int preferredLaunchSiteId = 654;
            const int registrationId = 321654;
            var user = new MpUser() {};
            var registration = BuildRegistration();
            var registrationDto = BuildRegistrationDto(participantId, preferredLaunchSiteId, registration);

            _groupConnectorService.Setup(m => m.GetGroupConnectorByProjectId(projectId, token))
                .Returns(new MpGroupConnector {Id = groupConnectorId});
            _groupConnectorService.Setup(m => m.GetGroupConnectorById(groupConnectorId))
                .Returns(new MpGroupConnector() {PreferredLaunchSiteId = preferredLaunchSiteId});
            _groupConnectorService.Setup(m => m.CreateGroupConnectorRegistration(groupConnectorId, registrationId));
            _contactService.Setup(m => m.UpdateContact(registration.Self.ContactId, It.IsAny<Dictionary<string, object>>()))
                .Callback((int cid, Dictionary<string, object> dict) =>
                          {
                              Assert.AreEqual(registration.Self.ContactId, cid);
                              Assert.AreEqual(registration.Self.GetDictionary(), dict);
                              Assert.AreEqual(registration.Self.FirstName, dict["Nickname"]);
                          });
            _userService.Setup(m => m.GetByAuthenticationToken(token))
                .Returns(user);
            _userService.Setup(m => m.UpdateUser(It.IsAny<MpUser>()))
                .Callback((MpUser updatedUser) =>
                         {
                             Assert.AreEqual(registration.Self.EmailAddress, updatedUser.UserId);
                             Assert.AreEqual(registration.Self.EmailAddress, updatedUser.UserEmail);
                             Assert.AreEqual(registration.Self.LastName + ", " + registration.Self.FirstName, updatedUser.DisplayName);
                         });
            _participantService.Setup(m => m.GetParticipantRecord(token))
                .Returns(new MpParticipant() {ParticipantId = participantId});
            _configurationWrapper.Setup(m => m.GetConfigIntValue("CrossroadsOrganizationId"))
                .Returns(CrossroadsOrganizationId);
            _registrationService.Setup(m => m.CreateRegistration(It.IsAny<MpRegistration>()))
                .Returns((MpRegistration mpRegistration) =>
                         {
                             Assert.AreEqual(mpRegistration.OrganizationId, CrossroadsOrganizationId);
                             Assert.AreEqual(mpRegistration.ParticipantId, participantId);
                             Assert.AreEqual(mpRegistration.PreferredLaunchSiteId, preferredLaunchSiteId);
                             Assert.AreEqual(mpRegistration.InitiativeId, registration.InitiativeId);
                             Assert.AreEqual(mpRegistration.SpouseParticipation, registration.SpouseParticipation);
                             return registrationId;
                         });

            _fixture.CreateAnywhereRegistration(registration, projectId, token);

            _groupConnectorService.VerifyAll();
            _contactService.VerifyAll();
            _userService.VerifyAll();
            _participantService.VerifyAll();
            _registrationService.VerifyAll();
        }

        [Test]
        public void ShouldThrowExceptionIfGroupConnectorHasNoLaunchSite()
        {
            const int projectId = 564;
            const string token = "asdf";
            const int groupConnectorId = 1324;
            const int participantId = 9876543;
            const int preferredLaunchSiteId = 654;
            const int registrationId = 321654;
            var user = new MpUser() { };
            var registration = BuildRegistration();
            var registrationDto = BuildRegistrationDto(participantId, preferredLaunchSiteId, registration);

            _groupConnectorService.Setup(m => m.GetGroupConnectorByProjectId(projectId, token))
                .Returns(new MpGroupConnector { Id = groupConnectorId });
            _groupConnectorService.Setup(m => m.GetGroupConnectorById(groupConnectorId))
                .Returns((MpGroupConnector) null);
            _contactService.Setup(m => m.UpdateContact(registration.Self.ContactId, It.IsAny<Dictionary<string, object>>()))
                .Callback((int cid, Dictionary<string, object> dict) =>
                {
                    Assert.AreEqual(registration.Self.ContactId, cid);
                    Assert.AreEqual(registration.Self.GetDictionary(), dict);
                    Assert.AreEqual(registration.Self.FirstName, dict["Nickname"]);
                });
            _userService.Setup(m => m.GetByAuthenticationToken(token))
                .Returns(user);
            _userService.Setup(m => m.UpdateUser(It.IsAny<MpUser>()))
                .Callback((MpUser updatedUser) =>
                {
                    Assert.AreEqual(registration.Self.EmailAddress, updatedUser.UserId);
                    Assert.AreEqual(registration.Self.EmailAddress, updatedUser.UserEmail);
                    Assert.AreEqual(registration.Self.LastName + ", " + registration.Self.FirstName, updatedUser.DisplayName);
                });
            _participantService.Setup(m => m.GetParticipantRecord(token))
                .Returns(new MpParticipant() { ParticipantId = participantId });

            Assert.Throws<Exception>(() =>
                                     {
                                         _fixture.CreateAnywhereRegistration(registration, projectId, token);
                                     });
            _groupConnectorService.VerifyAll();
            _contactService.VerifyAll();
            _userService.VerifyAll();
            _participantService.VerifyAll();
        }

        [Test]
        public void ShouldThrowDuplicateUserException()
        {
            const int projectId = 564;
            const string token = "asdf";
            const int groupConnectorId = 1324;
            const int participantId = 9876543;
            const int preferredLaunchSiteId = 654;
            const int registrationId = 321654;
            var user = new MpUser() { };
            var registration = BuildRegistration();
            var registrationDto = BuildRegistrationDto(participantId, preferredLaunchSiteId, registration);

            _groupConnectorService.Setup(m => m.GetGroupConnectorByProjectId(projectId, token))
                .Returns(new MpGroupConnector { Id = groupConnectorId });
            _contactService.Setup(m => m.UpdateContact(registration.Self.ContactId, It.IsAny<Dictionary<string, object>>()))
                .Callback((int cid, Dictionary<string, object> dict) =>
                {
                    Assert.AreEqual(registration.Self.ContactId, cid);
                    Assert.AreEqual(registration.Self.GetDictionary(), dict);
                    Assert.AreEqual(registration.Self.FirstName, dict["Nickname"]);
                });
            _userService.Setup(m => m.GetByAuthenticationToken(token))
                .Returns(user);
            _userService.Setup(m => m.UpdateUser(It.IsAny<MpUser>()))
                .Throws(new DuplicateUserException(registration.Self.EmailAddress));
            _configurationWrapper.Setup(m => m.GetConfigIntValue("CrossroadsOrganizationId"))
                .Returns(CrossroadsOrganizationId);

            Assert.Throws<DuplicateUserException>(() =>
                                                  {
                                                      _fixture.CreateAnywhereRegistration(registration, projectId, token);
                                                  });

            _groupConnectorService.VerifyAll();
            _contactService.VerifyAll();
            _userService.VerifyAll();
            _participantService.VerifyAll();
            _registrationService.VerifyAll();
        }

        private AnywhereRegistration BuildRegistration()
        {
            return new AnywhereRegistration
            {
                GroupConnectorId = 0,
                InitiativeId = 3,
                OrganizationId = 0,
                Self = new Registrant
                {
                    ContactId = 1234567,
                    DateOfBirth = "01/01/1000",
                    EmailAddress = "abomb@thebomb.com",
                    FirstName = "a",
                    LastName = "bomb",
                    MobilePhone = "555-555-5555"
                },
                SpouseParticipation = false
            };
        }

        private MpRegistration BuildRegistrationDto(int participantId, int preferredLaunchSiteId, AnywhereRegistration registration)
        {
            return new MpRegistration()
            {
                ParticipantId = participantId,
                PreferredLaunchSiteId = preferredLaunchSiteId,
                InitiativeId = registration.InitiativeId,
                SpouseParticipation =  registration.SpouseParticipation,
                OrganizationId = CrossroadsOrganizationId
            };
        }

        private string Skills(CincinnatiRegistration registration)
        {
            if (registration.Skills != null && registration.Skills.Where(sk => sk.Checked).ToList().Count > 0)
            {
                return registration.Skills.Where(sk => sk.Checked).Select(sk => sk.Name).Aggregate((first, next) => first + ", " + next);                
            }
            return "";
        } 

        private Dictionary<string, string> Styles()
        {
            return new Dictionary<string, string>()
            {
                {"style", "border-spacing: 0; border-collapse: collapse; vertical-align: top; text-align: left; width: 100%; padding: 0; border:none; border-color:#ffffff;font-size: medium; font-weight: normal;" }
            };
        }

        private static HtmlElement BuildParagraph(string label, string value)
        {
            var els = new List<HtmlElement>()
            {
                new HtmlElement("strong", label),
                new HtmlElement("span", value)
            }
            ;
            return new HtmlElement("p", els);
        } 

    }
}
