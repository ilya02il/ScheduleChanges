using Application.CallSchedules.Dtos;
using Application.ChangesLists.Commands.CreateChangesList;
using Application.ChangesLists.Commands.CreateChangesListItem;
using Application.ChangesLists.Commands.UpdateChangesListItem;
using Application.Groups.Dtos;
using Application.ScheduleLists.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.ValueObjects;

namespace WebAPI.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<LessonCallEntity, CallScheduleListItemDto>();

            CreateMap<GroupEntity, GroupDto>();

            CreateMap<ScheduleListEntity, ScheduleListDto>();

            CreateMap<ScheduleListItemDto, ItemInfo>()
                .ForMember(dest => dest.SubjectName, act => act.MapFrom(src => src.Discipline))
                .ForMember(dest => dest.TeacherInitials, act => act.MapFrom(src => src.Teacher));

            CreateMap<ChangesListItemDto, ItemInfo>()
                .ForMember(dest => dest.SubjectName, act => act.MapFrom(src => src.SubjectName))
                .ForMember(dest => dest.TeacherInitials, act => act.MapFrom(src => src.TeacherInitials));

            CreateMap<ChangesListItemDto, ChangesListItemEntity>()
                .ForMember(dest => dest.ChangesListId, act => act.MapFrom(src => src.ListId))
                .ForMember(dest => dest.ItemInfo, act => act.MapFrom((src, dest, info, context) => context.Mapper.Map(src, info)));

            CreateMap<CreateChangesListItemCommand, ItemInfo>()
                .ForMember(dest => dest.SubjectName, act => act.MapFrom(src => src.Discipline))
                .ForMember(dest => dest.TeacherInitials, act => act.MapFrom(src => src.Teacher));

            CreateMap<CreateChangesListItemCommand, ChangesListItemEntity>()
                .ForMember(dest => dest.ChangesListId, act => act.MapFrom(src => src.ListId))
                .ForMember(dest => dest.ItemInfo, act => act.MapFrom((src, dest, info, context) => context.Mapper.Map(src, info)));

            CreateMap<UpdateChangesListItemCommand, ItemInfo>()
                .ForMember(dest => dest.SubjectName, act => act.MapFrom(src => src.Discipline))
                .ForMember(dest => dest.TeacherInitials, act => act.MapFrom(src => src.Teacher));
        }
    }
}
