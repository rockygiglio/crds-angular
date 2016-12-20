using System;
using System.Linq;
using crds_angular.Models.Crossroads.Camp;
using crds_angular.Models.Crossroads.Events;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Models.Rules;

namespace crds_angular.test.Helpers
{
    public class Factories
    {
        public static void MPProductRuleSet()
        {
            if (!FactoryGirl.NET.FactoryGirl.DefinedFactories.Contains(typeof(MPProductRuleSet)))
            {
                FactoryGirl.NET.FactoryGirl.Define(() => new MPProductRuleSet()
                {
                    ProductId = 134,
                    RulesetId = 345,
                    StartDate = DateTime.Now.AddDays(-30)
                });
            }
        }

        public static void MpEvent()
        {
            if (!FactoryGirl.NET.FactoryGirl.DefinedFactories.Contains(typeof(MpEvent)))
            {
                FactoryGirl.NET.FactoryGirl.Define(() => new MpEvent
                {
                    EventId = 4234,
                    CongregationId = 767,
                    OnlineProductId = 1234
                });
            }
        }

        public static void MpCongregation()
        {
            if (!FactoryGirl.NET.FactoryGirl.DefinedFactories.Contains(typeof(MpCongregation)))
            {
                FactoryGirl.NET.FactoryGirl.Define<MpCongregation>(() => new MpCongregation
                {
                    CongregationId = 12,
                    LocationId = 4,
                    Name = "Oakley"
                });
            }
        }

        public static void MpGroupParticipant()
        {
            if (!FactoryGirl.NET.FactoryGirl.DefinedFactories.Contains(typeof(MpGroupParticipant)))
            {
                FactoryGirl.NET.FactoryGirl.Define<MpGroupParticipant>(() => new MpGroupParticipant
                {
                    GroupId = 2123,
                    GroupParticipantId = 1243
                });
            }
        }

        public static void EventParticipant()
        {
            if (!FactoryGirl.NET.FactoryGirl.DefinedFactories.Contains(typeof(MpEventParticipant)))
            {

                FactoryGirl.NET.FactoryGirl.Define<MpEventParticipant>(() => new MpEventParticipant
                                                                       {
                                                                           EventTitle = "Ok Fake Title",
                                                                           ChildcareRequired = false,
                                                                           ContactId = 8767,
                                                                           EndDate = null,
                                                                           EventId = 98,
                                                                           EventParticipantId = 987,
                                                                           EventStartDateTime = new DateTime(2017, 10, 11),
                                                                           GroupId = 12,
                                                                           GroupName = "Fake Group Name",
                                                                           GroupParticipantId = 3456,
                                                                           ParticipantEmail = "SomeEmail23@gmail.com",
                                                                           ParticipantStatus = 2,
                                                                           ParticipantId = 435
                                                                       });
            }
        }

        public static void EventParticipantDTO()
        {
            if (!FactoryGirl.NET.FactoryGirl.DefinedFactories.Contains(typeof(EventParticipantDTO)))
            {
                FactoryGirl.NET.FactoryGirl.Define<EventParticipantDTO>(() => new EventParticipantDTO
                                                                        {
                                                                            EventId = 621234,
                                                                            EndDate = null,
                                                                            EventParticipantId = 1235,
                                                                            ParticipantId = 2561,
                                                                            ParticipationStatus = 5
                                                                        });
            }
        }

        public static void CampReservationDTO()
        {
            if (!FactoryGirl.NET.FactoryGirl.DefinedFactories.Contains(typeof(CampReservationDTO)))
            {
                FactoryGirl.NET.FactoryGirl.Define(() => new CampReservationDTO
                {
                    ContactId = 0,
                    FirstName = "Jon",
                    LastName = "Horner",
                    MiddleName = "",
                    BirthDate = new DateTime(2006, 04, 03) + "",
                    Gender = 1,
                    PreferredName = "Jon",
                    SchoolAttending = "Mason",
                    CurrentGrade = 3,
                    SchoolAttendingNext = "Mason",
                    CrossroadsSite = 3,
                    RoomMate = ""
                });
            }
        }
    }
}
