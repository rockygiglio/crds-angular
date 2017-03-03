﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using crds_angular.Models.Crossroads.GoVolunteer;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Services;
using log4net;
using Crossroads.Web.Common.Configuration;
using Crossroads.Web.Common.MinistryPlatform;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Models.GoCincinnati;
using MinistryPlatform.Translation.Repositories.Interfaces.GoCincinnati;
using IGroupConnectorRepository = MinistryPlatform.Translation.Repositories.Interfaces.GoCincinnati.IGroupConnectorRepository;
using MPInterfaces = MinistryPlatform.Translation.Repositories.Interfaces;

namespace crds_angular.Services
{
    public class GoVolunteerService : IGoVolunteerService

    {
        private readonly IAttributeService _attributeService;
        private readonly MPInterfaces.ICommunicationRepository _communicationService;
        private readonly IConfigurationWrapper _configurationWrapper;
        private readonly MPInterfaces.IContactRelationshipRepository _contactRelationshipService;
        private readonly MPInterfaces.IContactRepository _contactService;
        private readonly IGroupConnectorRepository _groupConnectorService;
        private readonly ILog _logger = LogManager.GetLogger(typeof (GoVolunteerService));
        private readonly int _otherEquipmentId;
        private readonly MPInterfaces.IParticipantRepository _participantService;
        private readonly MPInterfaces.IProjectTypeRepository _projectTypeService;
        private readonly IRegistrationRepository _registrationService;
        private readonly IGoSkillsService _skillsService;
        private readonly MPInterfaces.IUserRepository _userService;
        private readonly IApiUserRepository _apiUserRepository;
        private readonly IProjectRepository _projectRepository;

        public GoVolunteerService(MPInterfaces.IParticipantRepository participantService,
                                  IRegistrationRepository registrationService,
                                  MPInterfaces.IContactRepository contactService,
                                  IGroupConnectorRepository groupConnectorService,
                                  IConfigurationWrapper configurationWrapper,
                                  MPInterfaces.IContactRelationshipRepository contactRelationshipService,
                                  MPInterfaces.IProjectTypeRepository projectTypeService,
                                  IAttributeService attributeService,
                                  IGoSkillsService skillsService,
                                  MPInterfaces.ICommunicationRepository comunicationService,
                                  MPInterfaces.IUserRepository userService,
                                  IApiUserRepository apiUserRepository,
                                  IProjectRepository projectRepository)
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
            _userService = userService;
            _apiUserRepository = apiUserRepository;
            _projectRepository = projectRepository;
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

        public CincinnatiRegistration CreateRegistration(CincinnatiRegistration registration, string token)
        {
            try
            {
                var participantId = RegistrationContact(registration, token);
                var registrationId = CreateRegistration(registration, participantId);

                var asyncTasks = Observable.CombineLatest(
                    Observable.Start(() => GroupConnector(registration, registrationId)),
                    Observable.Start(() => _skillsService.UpdateSkills(participantId, registration.Skills, token)),
                    Observable.Start(() => Attributes(registration, registrationId))
                    );

                if (registration.SpouseParticipation)
                {
                    var spouse = Observable.Start<MpContact>(() => SpouseInformation(registration));
                    spouse.Subscribe(contact =>
                    {
                        if (contact != null)
                        {
                            registration.Spouse.ContactId = contact.ContactId;
                            registration.Spouse.EmailAddress = contact.EmailAddress;
                        }
                        Observable.Start(() => SendMail(registration));
                    });
                }
                else
                {
                    Observable.Start(() => SendMail(registration));
                }
                               
                return registration;

            }
            catch (Exception ex)
            {
                const string msg = "Go Volunteer Service: CreateRegistration";
                _logger.Error(msg, ex);
                throw new Exception(msg, ex);
            }
        }

        public AnywhereRegistration CreateAnywhereRegistration(AnywhereRegistration registration, int projectId, string token)
        {
            try
            {
                MpGroupConnector mpGroupConnector = _groupConnectorService.GetGroupConnectorByProjectId(projectId, token);
                registration.GroupConnectorId = mpGroupConnector.Id;

                var participantId = RegistrationContact(registration, token);
                CreateAnywhereRegistrationDto(registration, participantId);

                return registration;
            }
            catch (Exception e)
            {
                const string msg = "Go Volunteer Service: CreateAnywhereRegistration";
                _logger.Error(msg, e);
                throw new Exception(msg, e);
            }
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

        public bool SendMail(CincinnatiRegistration registration)
        {
            try
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
            catch (Exception e)
            {
                _logger.Error("Sending email failed");
                return false;
            }
        }

        public List<ProjectCity> GetParticipatingCities(int initiativeId)
        {
            var apiToken = _apiUserRepository.GetToken();
            var projects = _projectRepository.GetProjectsByInitiative(initiativeId, apiToken);
            var cities = projects.Select(p => new ProjectCity {ProjectId = p.ProjectId ,City = p.City, State = p.State}).ToList();
            return cities;
        }
        
        public Project GetProject(int projectId)
        {
            var apiToken = _apiUserRepository.GetToken();
            var project = _projectRepository.GetProject(projectId, apiToken);
            if (!project.Status) throw new ApplicationException(project.ErrorMessage);
            var groupConnector = _projectRepository.GetGroupConnector(projectId, apiToken);
            var jsonProject = new Project
            {
                AddressId = project.Value.AddressId,
                InitiativeId = project.Value.InitiativeId,
                LocationId = project.Value.LocationId,
                OrganizationId = project.Value.OrganizationId,
                ProjectId = project.Value.ProjectId,
                ProjectName = project.Value.ProjectName,
                ProjectStatusId = project.Value.ProjectStatusId,
                ProjectTypeId = project.Value.ProjectTypeId,
                ProjectType = project.Value.ProjectType,
                Location = $"{project.Value.City}, {project.Value.State}"                    
            };

            if (!groupConnector.Status) return jsonProject;
            var name = $"{groupConnector.Value.PrimaryContactNickname ?? groupConnector.Value.PrimaryContactFirstName} {groupConnector.Value.PrimaryContactLastName}";
            jsonProject.ContactId = groupConnector.Value.PrimaryContactId;
            jsonProject.ContactEmail = groupConnector.Value.PrimaryContactEmail;
            jsonProject.ContactDisplayName = name;
            return jsonProject;
        }

        public Dictionary<string, object> SetupMergeData(CincinnatiRegistration registration)
        {
            var styles = Styles();
            
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
            if (registration.Skills != null && registration.Skills.Where(sk => sk.Checked).ToList().Count > 0)
            {
                listOfP.Add(BuildParagraph("Unique Skills: ", registration.Skills.Where(sk => sk.Checked).Select(sk => sk.Name).Aggregate((first, next) => first + ", " + next)));
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

            var htmlCell = new HtmlElement("td", styles).Append(listOfP);
            var htmlRow = new HtmlElement("tr", styles).Append(htmlCell);
            var htmlTBody = new HtmlElement("tbody", styles).Append(htmlRow);
            var htmlTable = new HtmlElement("table", styles).Append(htmlTBody);
            

            var dict = new Dictionary<string, object>()
            {
                {"HTML_TABLE", htmlTable.Build()},
                {"Nickname", registration.Self.FirstName},
                {"Lastname", registration.Self.LastName},
            };

            if (registration.SpouseParticipation)
            {
                dict.Add("Spouse_Nickname", registration.Spouse.FirstName);
                dict.Add("Spouse_Lastname", registration.Spouse.LastName);
            }

            return dict;
        }

        private List<HtmlElement> PrepWorkDetails(CincinnatiRegistration registration)
        {
            var prepWork = new List<HtmlElement>();
            if (registration.PrepWork.Count == 0)
            {
                prepWork.Add(BuildParagraph("Available for Prep Work: ", "No"));
                prepWork.Add(BuildParagraph("Spouse Available for Prep Work: ", "No"));
            }
            else if (registration.PrepWork.Count < 2 && registration.PrepWork[0].Spouse)
            {
                prepWork.Add(BuildParagraph("Available for Prep Work: ", "No"));
                prepWork.Add(BuildParagraph("Spouse Available for Prep Work: ", "Yes, from " + registration.PrepWork[0].Name));
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
            var birthDate = DateTime.Parse(registration.Self.DateOfBirth);
            return new List<HtmlElement>
            {
                BuildParagraph("Name: ", registration.Self.FirstName + " " + registration.Self.LastName),
                BuildParagraph("Email: ", registration.Self.EmailAddress),
                BuildParagraph("Birthdate: ", birthDate.Month + "/" + birthDate.Day + "/" + birthDate.Year),
                BuildParagraph("Mobile Phone: ", registration.Self.MobilePhone)
            };
        }

        private List<HtmlElement> SpouseDetails(CincinnatiRegistration registration)
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
                var birthDate = DateTime.Parse(registration.Spouse.DateOfBirth);
                spouse.Add(BuildParagraph("Spouse Birthdate: ", birthDate.Month + "/" + birthDate.Day + "/" + birthDate.Year));
            }
            if (registration.Spouse.MobilePhone != null)
            {
                spouse.Add(BuildParagraph("Spouse Mobile Phone: ", registration.Spouse.MobilePhone));
            }
            return spouse;
        }

        private List<HtmlElement> ChildrenDetails(CincinnatiRegistration registration)
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

        private List<HtmlElement> GroupConnectorDetails(CincinnatiRegistration registration)
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

        private Dictionary<string, string> Styles()
        {
            return new Dictionary<string, string>()
            {
                {"style", "border-spacing: 0; border-collapse: collapse; vertical-align: top; text-align: left; width: 100%; padding: 0; border:none; border-color:#ffffff;font-size: medium; font-weight: normal;" }
            };
        } 

        private void Attributes(CincinnatiRegistration registration, int registrationId)
        {
            ChildAgeGroups(registration, registrationId);
            PrepWork(registration, registrationId);
            Equipment(registration, registrationId);
            ProjectPreferences(registration, registrationId);
        }

        private void ProjectPreferences(CincinnatiRegistration registration, int registrationId)
        {
            foreach (var projectPreference in registration.ProjectPreferences.Where(pref => pref.Id != 0))
            {
                _registrationService.AddProjectPreferences(registrationId, projectPreference.Id, projectPreference.Priority);
            }
        }

        private void Equipment(CincinnatiRegistration registration, int registrationId)
        {
            foreach (var equipment in registration.Equipment.Where(e => e != null))
            {
                var id = equipment.Id != 0 ? equipment.Id : _otherEquipmentId;
                _registrationService.AddEquipment(registrationId, id, equipment.Notes);
            }
        }

        private void PrepWork(CincinnatiRegistration registration, int registrationId)
        {
            foreach (var prepWork in registration.PrepWork)
            {
                _registrationService.AddPrepWork(registrationId, prepWork.Id, prepWork.Spouse);
            }
        }

        private void ChildAgeGroups(CincinnatiRegistration registration, int registrationId)
        {
            foreach (var ageGroup in registration.ChildAgeGroup)
            {
                _registrationService.AddAgeGroup(registrationId, ageGroup.Id, ageGroup.Count);
            }
        }

        private MpContact SpouseInformation(CincinnatiRegistration registration)
        {
            

            if (!AddSpouse(registration))
            {
                return new MpContact()
                {
                    ContactId = registration.Spouse.ContactId,
                    EmailAddress = registration.Spouse.EmailAddress ?? _contactService.GetContactEmail(registration.Spouse.ContactId)
                };
            }
            else
            {
                var contact = Observable.Start(() => _contactService.CreateSimpleContact(registration.Spouse.FirstName,
                                                                                         registration.Spouse.LastName,
                                                                                         registration.Spouse.EmailAddress,
                                                                                         registration.Spouse.DateOfBirth,
                                                                                         registration.Spouse.MobilePhone));
                contact.Subscribe<MpContact>(c =>
                {
                    Observable.CombineLatest(
                        Observable.Start(() => { _participantService.CreateParticipantRecord(c.ContactId); }),
                        Observable.Start(() => CreateRelationship(registration, c.ContactId))
                        );
                });

                return contact.Wait();
            }
            
        }

        private void CreateRelationship(Registration registration, int contactId)
        {
            var relationship = new MpRelationship
            {
                RelationshipID = _configurationWrapper.GetConfigIntValue("MarriedTo"),
                RelatedContactID = contactId,
                StartDate = DateTime.Today
            };
            _contactRelationshipService.AddRelationship(relationship, registration.Self.ContactId);
        }

        private static bool AddSpouse(CincinnatiRegistration registration)
        {
            if (!registration.SpouseParticipation)
            {
                return false;
            }
            return registration.Spouse.ContactId == 0;
        }

        private void GroupConnector(CincinnatiRegistration registration, int registrationId)
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

        private int CreateRegistration(CincinnatiRegistration registration, int participantId)
        {
            var registrationDto = new MinistryPlatform.Translation.Models.GoCincinnati.MpRegistration();
            registrationDto.ParticipantId = participantId;
            var preferredLaunchSiteId = PreferredLaunchSite(registration);
            registrationDto.AdditionalInformation = registration.AdditionalInformation;
            registrationDto.InitiativeId = registration.InitiativeId;
            registrationDto.OrganizationId = registration.OrganizationId;
            registrationDto.OtherOrganizationName = registration.OtherOrganizationName;
            registrationDto.PreferredLaunchSiteId = preferredLaunchSiteId;
            registrationDto.RoleId = registration.RoleId;
            registrationDto.SpouseParticipation = registration.SpouseParticipation;
            return Registration(registrationDto);
        }

        private int CreateAnywhereRegistrationDto(AnywhereRegistration registration, int participantId)
        {
            var registrationDto = new MpRegistration();

            registrationDto.ParticipantId = participantId;
            var preferredLaunchSiteId = PreferredLaunchSite(registration);
            registrationDto.PreferredLaunchSiteId = preferredLaunchSiteId;
            registrationDto.InitiativeId = registration.InitiativeId;
            registrationDto.SpouseParticipation = registration.SpouseParticipation;
            registrationDto.OrganizationId = registration.OrganizationId == 0 ? _configurationWrapper.GetConfigIntValue("CrossroadsOrganizationId") : registration.OrganizationId;

            var registrationId = Registration(registrationDto);
            registrationDto.RegistrationId = registrationId;

            _groupConnectorService.CreateGroupConnectorRegistration(registration.GroupConnectorId, registrationId);

            return registrationId;
        }

        private int Registration(MinistryPlatform.Translation.Models.GoCincinnati.MpRegistration registrationDto)
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
            if (registration.PreferredLaunchSite == null || registration.PreferredLaunchSite.Id == 0)
            {
                // use group connector
                var groupConnector = _groupConnectorService.GetGroupConnectorById(registration.GroupConnectorId);
                preferredLaunchSiteId = groupConnector.PreferredLaunchSiteId;
            }
            else
            {
                // use preferred id
                preferredLaunchSiteId = registration.PreferredLaunchSite.Id;
            }
            return preferredLaunchSiteId;
        }

        private int RegistrationContact(Registration registration, string token)
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
            var contact = _contactService.CreateSimpleContact(registration.Self.FirstName,
                                                                registration.Self.LastName,
                                                                registration.Self.EmailAddress,
                                                                registration.Self.DateOfBirth,
                                                                registration.Self.MobilePhone);
            registration.Self.ContactId = contact.ContactId;
            var participantId = _participantService.CreateParticipantRecord(contact.ContactId);
            return participantId;
        }

        private int ExistingParticipant(Registration registration, string token)
        {
            // update name/email/dob/mobile
            var dict = registration.Self.GetDictionary();
            _contactService.UpdateContact(registration.Self.ContactId, dict);

            // update the user record?
            MpUser user = _userService.GetByAuthenticationToken(token);
            user.UserId = registration.Self.EmailAddress;
            user.UserEmail = registration.Self.EmailAddress;
            user.DisplayName = registration.Self.LastName + ", " + registration.Self.FirstName;
            _userService.UpdateUser(user);

            //get participant
            var participant = _participantService.GetParticipantRecord(token);
            return participant.ParticipantId;
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
