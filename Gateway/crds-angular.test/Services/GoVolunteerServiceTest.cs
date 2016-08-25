using System;
using System.Collections.Generic;
using System.Linq;
using crds_angular.Models.Crossroads.GoVolunteer;
using crds_angular.Services;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Interfaces;
using Crossroads.Utilities.Services;
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
        private readonly Mock<MinistryPlatform.Translation.Repositories.Interfaces.IContactRelationshipRepository> _contactRelationshipService;
        private readonly Mock<MinistryPlatform.Translation.Repositories.Interfaces.IContactRepository> _contactService;
        private readonly Mock<IGroupConnectorRepository> _groupConnectorService;
        private readonly Mock<MinistryPlatform.Translation.Repositories.Interfaces.IParticipantRepository> _participantService;
        private readonly Mock<MinistryPlatform.Translation.Repositories.Interfaces.IProjectTypeRepository> _projectTypeService;
        private readonly Mock<IRegistrationRepository> _registrationService;
        private readonly Mock<IGoSkillsService> _skillsService;
        private readonly Mock<ICommunicationRepository> _commnuicationService;
        private readonly Mock<IUserRepository> _userService;


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
                _userService.Object);
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

        private string Skills(Registration registration)
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
