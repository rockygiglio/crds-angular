using System;
using System.Linq;
using crds_angular.Models.Crossroads.Events;
using MinistryPlatform.Translation.Models;

namespace crds_angular.test.Helpers
{
    public class Factories
    {
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
    }    
}
