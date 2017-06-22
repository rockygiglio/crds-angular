﻿using System;
using System.Collections.Generic;
using System.Linq;
using crds_angular.Models.Crossroads.Attribute;
using crds_angular.Models.Crossroads.GoVolunteer;
using FsCheck;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Models.GoCincinnati;
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

        public static CincinnatiRegistration RegistrationNoSpouse()
        {
            return new CincinnatiRegistration()
            {
                AdditionalInformation = "something profound",
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

        public static AnywhereRegistration AnywhereRegistrationNoSpouse()
        {
            return new AnywhereRegistration()
            {
                InitiativeId = RandomInt(),
                OrganizationId = RandomInt(),
                PreferredLaunchSite = PreferredLaunchSite(),
                SpouseParticipation = false,
                Spouse = null,
                RegistrationId = RandomInt(),
                Self = Registrant(),
                GroupConnectorId = RandomInt()
            };
        }

        public static CincinnatiRegistration RegistrationWithSkills()
        {
            var reg = RegistrationNoSpouse();
            reg.Skills = ListOfGoSkills(3);
            return reg;
        }

        public static CincinnatiRegistration RegistrationWithSpouse()
        {
            var registration = RegistrationNoSpouse();
            registration.SpouseParticipation = true;
            registration.Spouse = Registrant();
            return registration;
        }

        public static CincinnatiRegistration RegistrationWithSpouseLimited()
        {
            var registration = RegistrationNoSpouse();
            registration.SpouseParticipation = true;
            registration.Spouse = RegistrantOnlyRequired();
            return registration;
        }

        public static CincinnatiRegistration RegistrationWithGroupConnector()
        {
            var registration = RegistrationNoSpouse();
            registration.CreateGroupConnector = false;
            registration.GroupConnector = GroupConnector();
            return registration;
        }

        public static MpCommunication Communication(MpMyContact sender, MpMyContact sendee, int templateId)
        {           
            return new MpCommunication()
            {
                AuthorUserId = RandomInt(),
                DomainId = RandomInt(),
                EmailBody = Gen.Sample(100, 100, Gen.OneOf(Arb.Generate<string>())).HeadOrDefault,
                EmailSubject = Gen.Sample(100, 100, Gen.OneOf(Arb.Generate<string>())).HeadOrDefault,
                FromContact = new MpContact() {ContactId = sender.Contact_ID, EmailAddress = sender.Email_Address},
                MergeData = new Dictionary<string, object>(),
                ReplyToContact = new MpContact() {ContactId = sendee.Contact_ID, EmailAddress = sendee.Email_Address},
                TemplateId = templateId
            };
        }

        public static MpMyContact ContactFromRegistrant(Registrant r)
        {
            return new MpMyContact()
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
                ContactId = 1234,
                DateOfBirth = "1980-02-21T05:00:00.000Z",
                EmailAddress = "randomEmail@crossroads.net",
                FirstName = "stupid",
                LastName = "name",
                MobilePhone = "0987654321"
            };
        }

        public static Registrant RegistrantOnlyRequired()
        {
            return new Registrant()
            {
                ContactId = RandomInt(),
                FirstName = "asdfasdfasdf",
                LastName = "asdkjhiensdfie"
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

        public static MpGroupConnector MpGroupConnector()
        {
            return new MpGroupConnector()
            {
                AbsoluteMaximumVolunteers = 100,
                Name = Gen.Sample(1, 1, Gen.OneOf(Arb.Generate<string>())).HeadOrDefault,
                PreferredLaunchSite = Gen.Sample(1, 1, Gen.OneOf(Arb.Generate<string>())).HeadOrDefault,
                PrimaryRegistrationID = RandomInt(),
                ProjectMaximumVolunteers = 1000,
                ProjectMinimumAge = 2
            };
        }

        public static PreferredLaunchSite PreferredLaunchSite()
        {
            return new PreferredLaunchSite()
            {
                Id = RandomInt(),
                Name = "some name"
            };
        }

        public static List<ProjectPreference> ListOfProjectPreferences(int size = 3)
        {
            return Enumerable.Range(0, size).Select( (curr) => new ProjectPreference()
            {
                Id = curr + 1,
                Priority = curr,
                Name = "Awesome Name"
            }).ToList();
        }

        public static List<PrepWork> ListOfPrepWork(bool registrant, bool spouse)
        {
            var prepwork = new List<PrepWork>();
            if (registrant)
            {
                prepwork.Add(
                    new PrepWork()
                    {
                        Id = RandomInt(),
                        Name = "Name McNameFace",
                        Spouse = false
                    });
            }
            if (spouse)
            {
                prepwork.Add(new PrepWork()
                {
                    Id = RandomInt(),
                    Name = "Spouse McSpouseFace",
                    Spouse = true
                });
            }
            return prepwork;
        } 

        public static List<Equipment> ListOfEquipment(int size = 2)
        {
            return Enumerable.Range(0, size).Select((curr) => new Equipment()
            {
                Id = curr + 1,
                Notes = "blah blah blah"                
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
            return Enumerable.Range(0, size).Select((curr) =>
                new GoSkills(
                    curr + 1,
                    curr + 1,
                    $"{curr +1}",
                    $"{curr + 1}",
                    true
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

        public static MpMyContact MyContact(int contactId = 0)
        {
            var contact = new MpMyContact()
            {
                Address_ID = 12,
                Address_Line_1 = "123 Sesme Street",
                Age = 23,
                City = "Cincinnati",
                Contact_ID = 123445,
                County = "USA"
            };
            if (contactId != 0)
            {
                contact.Contact_ID = contactId;
            }
            return contact;            
        }
    }
}
