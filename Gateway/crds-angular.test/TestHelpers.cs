using System;
using System.Collections.Generic;
using System.Linq;
using crds_angular.Models.Crossroads.Attribute;
using crds_angular.Models.Crossroads.GoVolunteer;
using FsCheck;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Models;
using Equipment = crds_angular.Models.Crossroads.GoVolunteer.Equipment;
using Random = System.Random;

namespace crds_angular.test
{
    public static class TestHelpers
    {
        public static Random rnd = new Random(Guid.NewGuid().GetHashCode());

        public static int RandomInt()
        {
            return Gen.Sample(10000, 10000, Gen.OneOf(Arb.Generate<int>())).HeadOrDefault;
        }

        public static Registration RegistrationNoSpouse()
        {
            return new Registration()
            {
                AdditionalInformation = Gen.Sample(1, 100, Gen.OneOf(Arb.Generate<string>())).HeadOrDefault,
                ChildAgeGroup = ListOfChildrenAttending(2, 0, 1),
                CreateGroupConnector = true,
                Equipment = ListOfEquipment(),                
                InitiativeId = RandomInt(),
                OrganizationId = RandomInt(),
                PreferredLaunchSite = PreferredLaunchSite(),
                PrepWork = ListOfPrepWork(true, false),
                PrivateGroup = true,
                SpouseParticipation = false,
                Spouse = null,
                ProjectPreferences = ListOfProjectPreferences(),
                RegistrationId = RandomInt(),
                Self = Registrant(),
                RoleId = RandomInt(),
                WaiverSigned = true
            };
        }

        public static Registration RegistrationWithSpouse()
        {
            var registration = RegistrationNoSpouse();
            registration.SpouseParticipation = true;
            registration.Spouse = Registrant();
            return registration;
        }

        public static Registration RegistrationWithSpouseLimited()
        {
            var registration = RegistrationNoSpouse();
            registration.SpouseParticipation = true;
            registration.Spouse = RegistrantOnlyRequired();
            return registration;
        }

        public static Registration RegistrationWithGroupConnector()
        {
            var registration = RegistrationNoSpouse();
            registration.CreateGroupConnector = false;
            registration.GroupConnector = GroupConnector();
            return registration;
        }

        public static MinistryPlatform.Models.Communication Communication(MyContact sender, MyContact sendee, int templateId)
        {           
            return new Communication()
            {
                AuthorUserId = RandomInt(),
                DomainId = RandomInt(),
                EmailBody = Gen.Sample(100, 100, Gen.OneOf(Arb.Generate<string>())).HeadOrDefault,
                EmailSubject = Gen.Sample(100, 100, Gen.OneOf(Arb.Generate<string>())).HeadOrDefault,
                FromContact = new Contact() {ContactId = sender.Contact_ID, EmailAddress = sender.Email_Address},
                MergeData = new Dictionary<string, object>(),
                ReplyToContact = new Contact() {ContactId = sendee.Contact_ID, EmailAddress = sendee.Email_Address},
                TemplateId = templateId
            };
        }

        public static MyContact ContactFromRegistrant(Registrant r)
        {
            return new MyContact()
            {
                Age = RandomInt(),
                Contact_ID = r.ContactId,
                Email_Address = r.EmailAddress
            };
        }  

        public static Registrant Registrant()
        {
            return new Registrant()
            {
                ContactId = RandomInt(),
                DateOfBirth = "1980-02-21T05:00:00.000Z",
                EmailAddress = Gen.Sample(1, 1, Gen.OneOf(Arb.Generate<string>())).HeadOrDefault,
                FirstName = Gen.Sample(1, 1, Gen.OneOf(Arb.Generate<string>())).HeadOrDefault,
                LastName = Gen.Sample(1, 1, Gen.OneOf(Arb.Generate<string>())).HeadOrDefault,
                MobilePhone = Gen.Sample(1, 1, Gen.OneOf(Arb.Generate<string>())).HeadOrDefault
            };
        }

        public static Registrant RegistrantOnlyRequired()
        {
            return new Registrant()
            {
                ContactId = RandomInt(),
                FirstName = Gen.Sample(1, 1, Gen.OneOf(Arb.Generate<string>())).HeadOrDefault,
                LastName = Gen.Sample(1, 1, Gen.OneOf(Arb.Generate<string>())).HeadOrDefault
            };
        }

        public static GroupConnector GroupConnector()
        {
            return new GroupConnector()
            {
                AbsoluteMaximumVolunteers = 100,
                GroupConnectorId = 1234,
                Name = Gen.Sample(1, 1, Gen.OneOf(Arb.Generate<string>())).HeadOrDefault,
                PreferredLaunchSite = Gen.Sample(1, 1, Gen.OneOf(Arb.Generate<string>())).HeadOrDefault,
                PrimaryRegistraionContactId = RandomInt(),
                ProjectMaximumVolunteers = 1000,
                ProjectMinimumAge = 2
            };
        }

        public static PreferredLaunchSite PreferredLaunchSite()
        {
            return new PreferredLaunchSite()
            {
                Id = RandomInt(),
                Name = Gen.Sample(1, 20, Gen.OneOf(Arb.Generate<string>())).HeadOrDefault
            };
        }

        public static List<ProjectPreference> ListOfProjectPreferences(int size = 3)
        {
            return Enumerable.Range(0, size).Select(_ => new ProjectPreference()
            {
                Id = RandomInt(),
                Priority = RandomInt(),
                Name = Gen.Sample(1, 1, Gen.OneOf(Arb.Generate<string>())).HeadOrDefault
            }).ToList();
        }

        public static List<PrepWork> ListOfPrepWork(bool registrant, bool spouse)
        {
            var prepwork = new List<PrepWork>();
            if (registrant)
            {
                prepwork.Add(new PrepWork() {Id = RandomInt(), Name = Gen.Sample(1, 1, Gen.OneOf(Arb.Generate<string>())).HeadOrDefault, Spouse = false});
            }
            if (spouse)
            {
                prepwork.Add(new PrepWork() {Id = RandomInt(), Name = Gen.Sample(1, 1, Gen.OneOf(Arb.Generate<string>())).HeadOrDefault, Spouse = true});
            }
            return prepwork;
        } 

        public static List<Equipment> ListOfEquipment(int size = 2)
        {
            return Enumerable.Range(0, size).Select(_ => new Equipment()
            {
                Id = RandomInt(),
                Notes = Gen.Sample(20, 1, Gen.OneOf(Arb.Generate<string>())).HeadOrDefault                
            }).ToList();
        }

        public static List<ChildrenAttending> ListOfChildrenAttending(int numberOf2, int numberOf8, int numberOf13)
        {
            return new List<ChildrenAttending>()
            {
                new ChildrenAttending()
                {
                    Id = 7040,
                    Count = numberOf2
                },
                new ChildrenAttending()
                {
                    Id = 7041,
                    Count = numberOf8
                },
                new ChildrenAttending()
                {
                    Id = 7042,
                    Count = numberOf13
                }
            };
        }

        public static List<MpGoVolunteerSkill> MPSkills(int size = 10)
        {
            return Enumerable.Repeat<MpGoVolunteerSkill>(new MpGoVolunteerSkill(
                Gen.Sample(1, 1, Gen.OneOf(Arb.Generate<int>())).HeadOrDefault,
                Gen.Sample(1, 1, Gen.OneOf(Arb.Generate<string>())).HeadOrDefault,
                Gen.Sample(1, 1, Gen.OneOf(Arb.Generate<string>())).HeadOrDefault,
                Gen.Sample(1, 1, Gen.OneOf(Arb.Generate<int>())).HeadOrDefault), size).ToList();        
        }

        public static List<GoSkills> ListOfGoSkills(int size = 10)
        {
            return Enumerable.Range(0, size).Select(_ =>
                new GoSkills(
                    Gen.Sample(10000, 10000, Gen.OneOf(Arb.Generate<int>())).HeadOrDefault,
                    Gen.Sample(100, 10000, Gen.OneOf(Arb.Generate<int>())).HeadOrDefault,
                    Gen.Sample(100, 100, Gen.OneOf(Arb.Generate<string>())).HeadOrDefault,
                    Gen.Sample(100, 100, Gen.OneOf(Arb.Generate<string>())).HeadOrDefault,
                    Gen.Sample(100, 2, Gen.OneOf(Arb.Generate<bool>())).HeadOrDefault
                    )
            ).ToList();                                          
        }

        public static List<AttributeTypeDTO> ListOfAttributeTypeDtos(int size = 10, int attributeListSize = 10)
        {
            return Enumerable.Repeat<AttributeTypeDTO>(new AttributeTypeDTO()
            {
                AllowMultipleSelections = Gen.Sample(1, 1, Gen.OneOf(Gen.Constant(true), Gen.Constant(false))).HeadOrDefault,
                Attributes = ListOfAttributeDtos(attributeListSize),
                AttributeTypeId = Gen.Sample(1, 1, Gen.OneOf(Arb.Generate<int>())).HeadOrDefault,
                Name = Gen.Sample(1, 1, Gen.OneOf(Arb.Generate<string>())).HeadOrDefault
            }, size).ToList();
        }

        public static List<AttributeDTO> ListOfAttributeDtos(int size = 10)
        {
            return Enumerable.Repeat<AttributeDTO>(new AttributeDTO()
            {
                AttributeId = Gen.Sample(1, 1, Gen.OneOf(Arb.Generate<int>())).HeadOrDefault,
                Name = Gen.Sample(20, 1, Gen.OneOf(Arb.Generate<string>())).HeadOrDefault,
                Category = Gen.Sample(20, 1, Gen.OneOf(Arb.Generate<string>())).HeadOrDefault,
                CategoryDescription = Gen.Sample(1, 1, Gen.OneOf(Arb.Generate<string>())).HeadOrDefault,
                CategoryId = Gen.Sample(1, 1, Gen.OneOf(Arb.Generate<int>())).HeadOrDefault,
                Description = Gen.Sample(1, 1, Gen.OneOf(Arb.Generate<string>())).HeadOrDefault,
                SortOrder = Gen.Sample(1, 1, Gen.OneOf(Arb.Generate<int>())).HeadOrDefault
            }, size).ToList();
        }

        public static MyContact MyContact(int contactId = 0)
        {
            var contact = new MyContact()
            {
                Address_ID = RandomInt(),
                Address_Line_1 = Gen.Sample(20, 1, Gen.OneOf(Arb.Generate<string>())).HeadOrDefault,
                Age = RandomInt(),
                City = Gen.Sample(20, 1, Gen.OneOf(Arb.Generate<string>())).HeadOrDefault,
                Contact_ID = RandomInt(),
                County = Gen.Sample(20, 1, Gen.OneOf(Arb.Generate<string>())).HeadOrDefault,
            };
            if (contactId != 0)
            {
                contact.Contact_ID = contactId;
            }
            return contact;            
        }
    }
}
