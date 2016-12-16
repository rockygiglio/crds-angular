using System;
using System.Linq;
using MinistryPlatform.Translation.Models;

namespace MinistryPlatform.Translation.Test.Helpers
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
    }
}