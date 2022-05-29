using Application.ScheduleWithChangesList.Queries.GetScheduleWithChangesListQuery;
using AutoMapper;
using DatedSchedules.Messages;
using Domain.Common;
using static DatedSchedules.Messages.GetDatedScheduleResponse.Types;

namespace Application.Mapping
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

            CreateMap<ScheduleWithChangesListItemDto, ScheduleItem>();
            CreateMap<ScheduleWithChangesDto, GetDatedScheduleResponse>()
                .ForMember(dest => dest.EducOrgName, act => act.MapFrom(src => src.EducOrgName))
                .ForMember(dest => dest.GroupNumber, act => act.MapFrom(src => src.GroupNumber));
        }
    }
}
