using Application.ScheduleWithChangesList.Queries.GetScheduleWithChangesListQuery;
using AutoMapper;
using Domain.Common;
using Google.Protobuf.WellKnownTypes;
using ScheduleService;
using static ScheduleService.GetDatedScheduleResponse.Types;

namespace GrpcServices.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ListItemEntity, ScheduleWithChangesListItemDto>()
                .ForMember(dest => dest.Position, act => act.MapFrom(src => src.ItemInfo.Position))
                .ForMember(dest => dest.Discipline, act => act.MapFrom(src => src.ItemInfo.SubjectName))
                .ForMember(dest => dest.Teacher, act => act.MapFrom(src => src.ItemInfo.TeacherInitials))
                .ForMember(dest => dest.Auditorium, act => act.MapFrom(src => src.ItemInfo.Auditorium));

            CreateMap<ScheduleWithChangesListItemDto, ScheduleItem>()
                .ForMember(dest => dest.StartTime, act => act.MapFrom(src => src.StartTime.ToDuration()))
                .ForMember(dest => dest.EndTime, act => act.MapFrom(src => src.EndTime.ToDuration()));
            CreateMap<ScheduleWithChangesDto, GetDatedScheduleResponse>()
                .ForMember(dest => dest.Date, act => act.MapFrom(src => src.Date.ToTimestamp()));
        }
    }
}
