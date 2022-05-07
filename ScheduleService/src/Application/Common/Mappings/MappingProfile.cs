using Application.ScheduleWithChangesList.Queries.GetScheduleWithChangesListQuery;
using AutoMapper;
using Domain.Common;

namespace Application.Common.Mapping
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

            //CreateMap<ChangesListItemEntity, ScheduleWithChangesListItemDto>()
            //    .ForMember(dest => dest.Position, act => act.MapFrom(src => src.ItemInfo.Position))
            //    .ForMember(dest => dest.Discipline, act => act.MapFrom(src => src.ItemInfo.SubjectName))
            //    .ForMember(dest => dest.Teacher, act => act.MapFrom(src => src.ItemInfo.TeacherInitials))
            //    .ForMember(dest => dest.Auditorium, act => act.MapFrom(src => src.ItemInfo.Auditorium));

            //CreateMap<ScheduleListItemEntity, ScheduleWithChangesListItemDto>()
            //    .ForMember(dest => dest.Position, act => act.MapFrom(src => src.ItemInfo.Position))
            //    .ForMember(dest => dest.Discipline, act => act.MapFrom(src => src.ItemInfo.SubjectName))
            //    .ForMember(dest => dest.Teacher, act => act.MapFrom(src => src.ItemInfo.TeacherInitials))
            //    .ForMember(dest => dest.Auditorium, act => act.MapFrom(src => src.ItemInfo.Auditorium));
        }
    }
}
