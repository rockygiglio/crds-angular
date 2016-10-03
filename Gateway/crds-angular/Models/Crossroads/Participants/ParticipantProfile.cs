using MinistryPlatform.Translation.Models;

namespace crds_angular.Models.Crossroads.Participants
{
    public class ParticipantProfile : AutoMapper.Profile
    {
        protected override void Configure()
        {
            AutoMapper.Mapper.CreateMap<MpParticipant, MinistryPlatform.Translation.Models.MpParticipant>()
                .ForMember(dest => dest.ContactId, opts => opts.MapFrom(src => src.ContactId))
                .ForMember(dest => dest.DisplayName, opts => opts.MapFrom(src => src.DisplayName))
                .ForMember(dest => dest.EmailAddress, opts => opts.MapFrom(src => src.EmailAddress))
                .ForMember(dest => dest.ParticipantId, opts => opts.MapFrom(src => src.ParticipantId))
                .ForMember(dest => dest.AttendanceStart, opts => opts.MapFrom(src => src.AttendanceStart))
                .ForMember(dest => dest.ApprovedSmallGroupLeader, opts => opts.MapFrom(src => src.ApprovedSmallGroupLeader));
        }
    }
}