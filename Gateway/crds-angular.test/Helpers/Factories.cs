using System;
using System.Linq;
using crds_angular.Models.Crossroads.Camp;
using crds_angular.Models.Crossroads.Events;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Models.Product;

namespace crds_angular.test.Helpers
{
    public class Factories
    {
        public static void MpProductOptionPrice()
        {
            if (!FactoryGirl.NET.FactoryGirl.DefinedFactories.Contains(typeof(MpProductOptionPrice)))
            {
                FactoryGirl.NET.FactoryGirl.Define(() => new MpProductOptionPrice());
            }
        }

        public static void MpEvent()
        {
            if (!FactoryGirl.NET.FactoryGirl.DefinedFactories.Contains(typeof(MpEvent)))
            {
                FactoryGirl.NET.FactoryGirl.Define(() => new MpEvent());
            }
        }

        public static void MpProduct()
        {
            if (!FactoryGirl.NET.FactoryGirl.DefinedFactories.Contains(typeof(MpProduct)))
            {
                FactoryGirl.NET.FactoryGirl.Define(() => new MpProduct());
            }
        }

        public static void MpParticipant()
        {
            if (!FactoryGirl.NET.FactoryGirl.DefinedFactories.Contains(typeof(MpParticipant)))
            {
                FactoryGirl.NET.FactoryGirl.Define(() => new MpParticipant
                {
                    ContactId = 21435,
                    Age = 10,
                    DisplayName = "Bluth, George Micheal",
                    EmailAddress = "george-micheal@bluthindustries.com",
                    ParticipantId = 90898,
                    Nickname = "George Micheal",
                    PreferredName = "George"
                });
            }
        }

        public static void CampProductDTO()
        {
            if (!FactoryGirl.NET.FactoryGirl.DefinedFactories.Contains(typeof(CampProductDTO)))
            {
                FactoryGirl.NET.FactoryGirl.Define(() => new CampProductDTO
                {
                    ContactId = 9823,
                    EventId = 78765,
                    FinancialAssistance = false
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
