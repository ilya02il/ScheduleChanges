using Application.ScheduleWithChangesList.Queries.GetScheduleWithChangesListQuery;
using AutoMapper;
using Domain.Common;
using Google.Protobuf.WellKnownTypes;
using DatedSchedules.Messages;
using static DatedSchedules.Messages.GetDatedScheduleResponse.Types;
using Application.ChangesLists.Commands.CreateChangesList;
using Domain.Entities;
using Application.ChangesLists.Commands.CreateChangesListItem;
using Domain.ValueObjects;
using Application.ChangesLists.Commands.UpdateChangesListItem;
using ChangesLists.Messages;
using Google.Protobuf.Collections;
using System.Collections.Generic;
using Application.ChangesLists.Queries.GetBriefScheduleChangesList;
using ScheduleLists.Messages;
using Application.ScheduleLists.Dtos;
using System;
using Application.Groups.Dtos;
using Groups.Messages;
using Application.CallSchedules.Dtos;
using CallSchedules.Messages;

namespace GrpcAPI.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap(typeof(IEnumerable<>), typeof(RepeatedField<>))
                .ConvertUsing(typeof(IEnumerableToRepeatedFieldTypeConverter<,>));

            CreateMap<Guid, string>()
                .ConvertUsing(src => src.ToString());

            CreateMap<string, Guid>()
                .ConvertUsing(src => Guid.Parse(src));

            CreateMap<Duration, TimeSpan>()
                .ConvertUsing(src => src.ToTimeSpan());

            CreateMap<TimeSpan, Duration>()
                .ConvertUsing(src => src.ToDuration());

            CreateMap<DateTimeOffset, Timestamp>()
                .ConvertUsing(src => src.ToTimestamp());

            CreateMap<Timestamp, DateTimeOffset>()
                .ConvertUsing(src => src.ToDateTimeOffset());

            //

            CreateMap<CallScheduleListItemDto, CallScheduleListItem>();

            CreateMap<LessonCallEntity, CallScheduleListItemDto>();

            CreateMapsForScheduleWithChangesLists();
            CreateMapsForChangesLists();
            CreateMapsForScheduleLists();
            CreateMapsForGroups();
        }

        private void CreateMapsForGroups()
        {
            CreateMap<GroupEntity, GroupDto>();

            CreateMap<GroupDto, Group>();

            CreateMap<BriefGroupDto, GroupBrief>();
        }

        private void CreateMapsForScheduleWithChangesLists()
        {
            CreateMap<ListItemEntity, ScheduleWithChangesListItemDto>()
                .ForMember(dest => dest.Discipline, act => act.MapFrom(src => src.ItemInfo.SubjectName))
                .ForMember(dest => dest.Teacher, act => act.MapFrom(src => src.ItemInfo.TeacherInitials));

            CreateMap<ScheduleWithChangesListItemDto, ScheduleItem>();

            CreateMap<ScheduleWithChangesDto, GetDatedScheduleResponse>();
        }

        private void CreateMapsForChangesLists()
        {
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

            CreateMap<ChangesListItemEntity, ChangesListItem>()
                .ForMember(dest => dest.SubjectName, act => act.MapFrom(src => src.ItemInfo.SubjectName))
                .ForMember(dest => dest.TeacherInitials, act => act.MapFrom(src => src.ItemInfo.TeacherInitials));

            CreateMap<ChangesListEntity, GetChangesListByIdResponse>()
                .ForMember(dest => dest.ListId, act => act.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.EducOrgId, act => act.MapFrom(src => src.EducationalOrgId.ToString()));

            CreateMap<BriefChangesListDto, BriefChangesList>();
        }

        private void CreateMapsForScheduleLists()
        {
            CreateMap<BooleanValue, bool>()
                .ConvertUsing(booleanValue => booleanValue.Value);

            CreateMap<ScheduleListItem, ScheduleListItemDto>();

            CreateMap<ScheduleListItemDto, ScheduleListItem>()
                .ForMember(dest => dest.IsOddWeek, act => act.MapFrom(src => src.IsOddWeek != null ? new BooleanValue() { Value = src.IsOddWeek.Value } : null));

            CreateMap<ScheduleListEntity, ScheduleListDto>();

            CreateMap<ItemInfo, ScheduleListItemDto>()
                .ForMember(dest => dest.Discipline, act => act.MapFrom(src => src.SubjectName))
                .ForMember(dest => dest.Teacher, act => act.MapFrom(src => src.TeacherInitials))
                .ReverseMap();

            CreateMap<ScheduleListItemEntity, ScheduleListItemDto>()
                .ConvertUsing(typeof(ScheduleListItemEntityToScheduleListItemDtoConverter));

            CreateMap<ScheduleListDto, ScheduleList>();
        }

        private class ScheduleListItemEntityToScheduleListItemDtoConverter
            : ITypeConverter<ScheduleListItemEntity, ScheduleListItemDto>
        {
            public ScheduleListItemDto Convert(ScheduleListItemEntity source, ScheduleListItemDto destination, ResolutionContext context)
            {
                destination = new ScheduleListItemDto(source.Id, source.IsOddWeek);
                context.Mapper.Map(source.ItemInfo, destination);

                return destination;
            }
        }

        private class IEnumerableToRepeatedFieldTypeConverter<TITemSource, TITemDest>
            : ITypeConverter<IEnumerable<TITemSource>, RepeatedField<TITemDest>>
        {

            public RepeatedField<TITemDest> Convert(IEnumerable<TITemSource> source, RepeatedField<TITemDest> destination, ResolutionContext context)
            {
                destination ??= new RepeatedField<TITemDest>();

                foreach (var item in source)
                {
                    destination.Add(context.Mapper.Map<TITemDest>(item));
                }
                return destination;
            }
        }
    }
}
