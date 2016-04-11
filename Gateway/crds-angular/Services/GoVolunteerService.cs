using System;
using System.Collections.Generic;
using System.Linq;
using crds_angular.Models.Crossroads.GoVolunteer;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Interfaces;
using Crossroads.Utilities.Services;
using log4net;
using MinistryPlatform.Translation.Services.Interfaces.GoCincinnati;
using IGroupConnectorService = MinistryPlatform.Translation.Services.Interfaces.GoCincinnati.IGroupConnectorService;
using MPInterfaces = MinistryPlatform.Translation.Services.Interfaces;

namespace crds_angular.Services
{
    public class GoVolunteerService : IGoVolunteerService

    {
        private readonly IAttributeService _attributeService;
        private readonly IConfigurationWrapper _configurationWrapper;
        private readonly MPInterfaces.IContactRelationshipService _contactRelationshipService;
        private readonly MPInterfaces.IContactService _contactService;
        private readonly IGroupConnectorService _groupConnectorService;
        private readonly ILog _logger = LogManager.GetLogger(typeof (GoVolunteerService));
        private readonly int _otherEquipmentId;
        private readonly MPInterfaces.IParticipantService _participantService;
        private readonly MPInterfaces.IProjectTypeService _projectTypeService;
        private readonly IRegistrationService _registrationService;
        private readonly IGoSkillsService _skillsService;
        private readonly MPInterfaces.ICommunicationService _communicationService;

        public GoVolunteerService(MPInterfaces.IParticipantService participantService,
                                  IRegistrationService registrationService,
                                  MPInterfaces.IContactService contactService,
                                  IGroupConnectorService groupConnectorService,
                                  IConfigurationWrapper configurationWrapper,
                                  MPInterfaces.IContactRelationshipService contactRelationshipService,
                                  MPInterfaces.IProjectTypeService projectTypeService,
                                  IAttributeService attributeService,
                                  IGoSkillsService skillsService,
                                  MPInterfaces.ICommunicationService comunicationService)
        {
            _participantService = participantService;
            _registrationService = registrationService;
            _contactService = contactService;
            _groupConnectorService = groupConnectorService;
            _configurationWrapper = configurationWrapper;
            _contactRelationshipService = contactRelationshipService;
            _projectTypeService = projectTypeService;
            _attributeService = attributeService;
            _otherEquipmentId = _configurationWrapper.GetConfigIntValue("GoCincinnatiOtherEquipmentAttributeId");
            _skillsService = skillsService;
            _communicationService = comunicationService;
        }

        public List<ChildrenOptions> ChildrenOptions()
        {
            var attributeTypeId = _configurationWrapper.GetConfigIntValue("GoCincinnatiRegistrationChildrenAttributeType");
            var attributeTypes = _attributeService.GetAttributeTypes(attributeTypeId);
            var attributes = attributeTypes.Single().Attributes;
            return attributes.Select(attribute => new ChildrenOptions
            {
                AttributeId = attribute.AttributeId,
                PluralLabel = string.Concat("Children Ages ", attribute.Name),
                SingularLabel = string.Concat("Child Age ", attribute.Name),
                Value = 0
            }).ToList();
        }

        public Registration CreateRegistration(Registration registration, string token)
        {
            var registrationDto = new MinistryPlatform.Translation.Models.GoCincinnati.Registration();
            try
            {
                registrationDto.ParticipantId = RegistrationContact(registration, token, registrationDto);
                var registrationId = CreateRegistration(registration, registrationDto);
                GroupConnector(registration, registrationId);
                SpouseInformation(registration);
                _skillsService.UpdateSkills(registrationDto.ParticipantId, registration.Skills, token);               
                Attributes(registration, registrationId);
            }
            catch (Exception ex)
            {
                const string msg = "Go Volunteer Service: CreateRegistration";
                _logger.Error(msg, ex);
                throw new Exception(msg, ex);
            }
            return registration;
        }

        public List<ProjectType> GetProjectTypes()
        {
            var pTypes = _projectTypeService.GetProjectTypes();
            return pTypes.Select(pt =>
            {
                var projType = new ProjectType();
                return projType.FromMpProjectType(pt);
            }).ToList();
        }

        public bool SendMail(Registration registration)
        {
            var templateId = _configurationWrapper.GetConfigIntValue("GoVolunteerEmailTemplate");
            var fromContactId = _configurationWrapper.GetConfigIntValue("GoVolunteerEmailFromContactId");
            var fromContact = _contactService.GetContactById(fromContactId);

          

            var mergeData = SetupMergeData(registration);

            var communication = _communicationService.GetTemplateAsCommunication(templateId,
                                                             fromContactId,
                                                             fromContact.Email_Address,
                                                             fromContactId,
                                                             fromContact.Email_Address,
                                                             registration.Self.ContactId,
                                                             registration.Self.EmailAddress,
                                                             mergeData);
            var returnVal = _communicationService.SendMessage(communication);
            if (registration.SpouseParticipation && returnVal > 0)
            {
                var spouseTemplateId = _configurationWrapper.GetConfigIntValue("GoVolunteerEmailSpouseTemplate");                
                var spouseCommunication = _communicationService.GetTemplateAsCommunication(spouseTemplateId,
                                                                                           fromContactId,
                                                                                           fromContact.Email_Address,
                                                                                           fromContactId,
                                                                                           fromContact.Email_Address,
                                                                                           registration.Spouse.ContactId,
                                                                                           registration.Spouse.EmailAddress,
                                                                                           mergeData);
                _communicationService.SendMessage(spouseCommunication);
            }
            return returnVal > 0;
        }

        public Dictionary<string, object> SetupMergeData(Registration registration)
        {
            var tableAttrs = TableAttributes();
            var listOfP = ProfileDetails(registration);
            if (registration.SpouseParticipation)
            {
                listOfP = listOfP.Concat(SpouseDetails(registration)).ToList();
            }
            listOfP = listOfP.Concat(ChildrenDetails(registration)).ToList();
            if (registration.GroupConnector != null)
            {
                listOfP = listOfP.Concat(GroupConnectorDetails(registration)).ToList();
            }
            else
            {
                listOfP.Add(BuildParagraph("Preferred Launch Site: ", registration.PreferredLaunchSite.Name));
                listOfP.Add(BuildParagraph("Project Preference 1: ", registration.ProjectPreferences[0].Name));
                listOfP.Add(BuildParagraph("Project Preference 2: ", registration.ProjectPreferences[1].Name));
                listOfP.Add(BuildParagraph("Project Preference 3: ", registration.ProjectPreferences[2].Name));
            }
            if (registration.Equipment.Count > 0)
            {
                listOfP.Add(BuildParagraph("Special Equipment: ", registration.Equipment.Select(equip => equip.Notes).Aggregate((first, next) => first + ", " + next)));
            }
            if (registration.AdditionalInformation != null)
            {
                listOfP.Add(BuildParagraph("Additional Info: ", registration.AdditionalInformation));
            }
            listOfP = listOfP.Concat(PrepWorkDetails(registration)).ToList();
                
            var htmlTable = new HtmlElement("table", tableAttrs)
               .Append(new HtmlElement("tbody"))
               .Append(new HtmlElement("tr"))
               .Append(new HtmlElement("td"))
               .Append(listOfP);

            var dict = new Dictionary<string, object>()
            {
                {"HtmlTable", htmlTable.Build()}
            };

            return dict;
        }

        private List<HtmlElement> PrepWorkDetails(Registration registration)
        {

            var prepWork = new List<HtmlElement>();
            if (registration.PrepWork.Count < 2 && registration.PrepWork[0].Spouse)
            {
                prepWork.Add(BuildParagraph("Available for Prep Work: ", "No"));
                prepWork.Add(BuildParagraph("Spouse Available for Prep Work: ", "Yes, from " + registration.PrepWork[0].Name));
            } else if (registration.PrepWork.Count == 0)
            {
                prepWork.Add(BuildParagraph("Available for Prep Work: ", "No"));
                prepWork.Add(BuildParagraph("Spouse Available for Prep Work: ", "No"));
            }
            else if (registration.PrepWork.Count < 2 && !registration.PrepWork[0].Spouse)
            {
                prepWork.Add(BuildParagraph("Available for Prep Work: ", "Yes, from " + registration.PrepWork[0].Name));
                prepWork.Add(BuildParagraph("Spouse Available for Prep Work: ", "No"));
            }
            else
            {
                prepWork.Add(BuildParagraph("Available for Prep Work: ", "Yes, from " + registration.PrepWork[0].Name));
                prepWork.Add(BuildParagraph("Spouse Available for Prep Work: ", "Yes, from " + registration.PrepWork[1].Name));
            }

            return prepWork;
        } 

        private List<HtmlElement> ProfileDetails(Registration registration)
        {
            return new List<HtmlElement>
            {
                BuildParagraph("Name: ", registration.Self.FirstName + " " + registration.Self.LastName),
                BuildParagraph("Email: ", registration.Self.EmailAddress),
                BuildParagraph("Birthdate: ", registration.Self.DateOfBirth),
                BuildParagraph("Mobile Phone: ", registration.Self.MobilePhone)
            };
        }

        private List<HtmlElement> SpouseDetails(Registration registration)
        {
            var spouse = new List<HtmlElement>()
            {
                BuildParagraph("Spouse Name: ", registration.Spouse.FirstName + " " + registration.Spouse.LastName),
            };
            if (registration.Spouse.EmailAddress != null)
            {
                spouse.Add(BuildParagraph("Spouse Email: ", registration.Spouse.EmailAddress));
            }
            if (registration.Spouse.DateOfBirth != null)
            {
                spouse.Add(BuildParagraph("Spouse Birthdate: ", registration.Spouse.DateOfBirth));
            }
            if (registration.Spouse.MobilePhone != null)
            {
                spouse.Add(BuildParagraph("Spouse Mobile Phone: ", registration.Spouse.MobilePhone));
            }
            return spouse;
        }

        private List<HtmlElement> ChildrenDetails(Registration registration)
        {
            return registration.ChildAgeGroup.Select(c =>
            {
                if (c.Id == _configurationWrapper.GetConfigIntValue("Children2To7") && c.Count > 0)
                {
                    return BuildParagraph("Number of Children Ages 2-7: ", c.Count.ToString());
                }
                else if (c.Id == _configurationWrapper.GetConfigIntValue("Children8To12") && c.Count > 0)
                {
                    return BuildParagraph("Number of Children Ages 8-12: ", c.Count.ToString());
                }
                else if (c.Id == _configurationWrapper.GetConfigIntValue("Children13To18") && c.Count > 0)
                {
                    return BuildParagraph("Number of Children Ages 13-18: ", c.Count.ToString());
                }
                return new HtmlElement("p");

            }).ToList();
        }

        private List<HtmlElement> GroupConnectorDetails(Registration registration)
        {
            var ret = new List<HtmlElement>();
            if (!registration.CreateGroupConnector)
            {
                ret.Add(BuildParagraph("Group Connector: ", registration.GroupConnector.Name));
                if (registration.GroupConnector.ProjectType != null)
                {
                    ret.Add(BuildParagraph("Project Type: ", registration.GroupConnector.ProjectType));
                }
                ret.Add(BuildParagraph("Preferred Launch Site: ", registration.GroupConnector.PreferredLaunchSite));
            }
            else
            {
                ret.Add(BuildParagraph("Preferred Launch Site: ", registration.PreferredLaunchSite.Name));
                ret.Add(BuildParagraph("Project Preference 1: ", registration.ProjectPreferences[0].Name));
                ret.Add(BuildParagraph("Project Preference 2: ", registration.ProjectPreferences[1].Name));
                ret.Add(BuildParagraph("Project Preference 3: ", registration.ProjectPreferences[2].Name));
            }

            return ret;
        } 

        private Dictionary<string, string> TableAttributes()
        {
            return new Dictionary<string, string>()
            {
                {"width", "100%"},
                {"border", "1"},
                {"cellspacing", "0"},
                {"cellpadding", "5"},
                {"style", "font-size: medium; font-weight: normal;" }
            };
        } 

        private void Attributes(Registration registration, int registrationId)
        {
            ChildAgeGroups(registration, registrationId);
            PrepWork(registration, registrationId);
            Equipment(registration, registrationId);
            ProjectPreferences(registration, registrationId);
        }

        private void ProjectPreferences(Registration registration, int registrationId)
        {
            foreach (var projectPreference in registration.ProjectPreferences.Where(pref => pref.Id != 0))
            {
                _registrationService.AddProjectPreferences(registrationId, projectPreference.Id, projectPreference.Priority);
            }
        }

        private void Equipment(Registration registration, int registrationId)
        {
            foreach (var equipment in registration.Equipment)
            {
                var id = equipment.Id != 0 ? equipment.Id : _otherEquipmentId;
                _registrationService.AddEquipment(registrationId, id, equipment.Notes);
            }
        }

        private void PrepWork(Registration registration, int registrationId)
        {
            foreach (var prepWork in registration.PrepWork)
            {
                _registrationService.AddPrepWork(registrationId, prepWork.Id, prepWork.Spouse);
            }
        }

        private void ChildAgeGroups(Registration registration, int registrationId)
        {
            foreach (var ageGroup in registration.ChildAgeGroup)
            {
                _registrationService.AddAgeGroup(registrationId, ageGroup.Id, ageGroup.Count);
            }
        }

        private void SpouseInformation(Registration registration)
        {
            if (!registration.SpouseParticipation)
            {
                return;
            }
            if (registration.Spouse.ContactId != 0)
            {
                return;
            }

            var contactId = _contactService.CreateSimpleContact(registration.Spouse.FirstName,
                                                                registration.Spouse.LastName,
                                                                registration.Spouse.EmailAddress,
                                                                registration.Spouse.DateOfBirth,
                                                                registration.Spouse.MobilePhone);

            // create relationship
            var relationship = new MinistryPlatform.Models.Relationship
            {
                RelationshipID = _configurationWrapper.GetConfigIntValue("MarriedTo"),
                RelatedContactID = contactId,
                StartDate = DateTime.Today
            };
            _contactRelationshipService.AddRelationship(relationship, registration.Self.ContactId);
        }

        private void GroupConnector(Registration registration, int registrationId)
        {
            if (registration.CreateGroupConnector)
            {
                _groupConnectorService.CreateGroupConnector(registrationId, registration.PrivateGroup);
            }
            else if (registration.GroupConnector.GroupConnectorId != 0)
            {
                _groupConnectorService.CreateGroupConnectorRegistration(registration.GroupConnector.GroupConnectorId, registrationId);
            }
        }

        private int CreateRegistration(Registration registration, MinistryPlatform.Translation.Models.GoCincinnati.Registration registrationDto)
        {
            var preferredLaunchSiteId = PreferredLaunchSite(registration);
            registrationDto.AdditionalInformation = registration.AdditionalInformation;
            registrationDto.InitiativeId = registration.InitiativeId;
            registrationDto.OrganizationId = registration.OrganizationId;
            registrationDto.PreferredLaunchSiteId = preferredLaunchSiteId;
            registrationDto.RoleId = registration.RoleId;
            registrationDto.SpouseParticipation = registration.SpouseParticipation;
            return Registration(registrationDto);
        }

        private int Registration(MinistryPlatform.Translation.Models.GoCincinnati.Registration registrationDto)
        {
            int registrationId;
            try
            {
                registrationId = _registrationService.CreateRegistration(registrationDto);
            }
            catch (Exception ex)
            {
                _logger.Error("GO Volunteer Service - Create Registration (Create Registration)", ex);
                throw;
            }
            return registrationId;
        }

        private int PreferredLaunchSite(Registration registration)
        {
            int preferredLaunchSiteId;
            if (registration.PreferredLaunchSite.Id == 0)
            {
                // use group connector
                var groupConnector = _groupConnectorService.GetGroupConnectorById(registration.GroupConnector.GroupConnectorId);
                preferredLaunchSiteId = groupConnector.PreferredLaunchSiteId;
            }
            else
            {
                // use preferred id
                preferredLaunchSiteId = registration.PreferredLaunchSite.Id;
            }
            return preferredLaunchSiteId;
        }

        private int RegistrationContact(Registration registration, string token, MinistryPlatform.Translation.Models.GoCincinnati.Registration registrationDto)
        {
            // Create or Update Contact
            var participantId = registration.Self.ContactId != 0 ? ExistingParticipant(registration, token) : CreateParticipant(registration);

            if (participantId == 0)
            {
                throw new ApplicationException("Registration Participant Not Found");
            }
            return participantId;
        }

        private int CreateParticipant(Registration registration)
        {
            //create contact & participant
            var contactId = _contactService.CreateSimpleContact(registration.Self.FirstName,
                                                                registration.Self.LastName,
                                                                registration.Self.EmailAddress,
                                                                registration.Self.DateOfBirth,
                                                                registration.Self.MobilePhone);
            registration.Self.ContactId = contactId;
            var participantId = _participantService.CreateParticipantRecord(contactId);
            return participantId;
        }

        private int ExistingParticipant(Registration registration, string token)
        {
            // update name/email/dob/mobile
            var dict = registration.Self.GetDictionary();
            _contactService.UpdateContact(registration.Self.ContactId, dict);

            //get participant
            var participant = _participantService.GetParticipantRecord(token);
            var participantId = participant.ParticipantId;
            return participantId;
        }

        private HtmlElement BuildParagraph(String label, String value)
        {
            return new HtmlElement("p", new HtmlElement("strong", label).Append(new HtmlElement("span", value)));
        }
    }
}