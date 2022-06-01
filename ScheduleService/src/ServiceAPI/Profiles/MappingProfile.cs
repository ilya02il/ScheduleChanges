using Application.CallSchedules.Dtos;
using Application.ChangesLists.Commands.CreateChangesList;
using Application.ChangesLists.Commands.CreateChangesListItem;
using Application.ChangesLists.Commands.UpdateChangesListItem;
using Application.Groups.Dtos;
using Application.ScheduleLists.Dtos;
using Application.ScheduleWithChangesList.Queries.GetScheduleWithChangesListQuery;
using AutoMapper;
using DatedSchedules.Messages;
using Domain.Common;
using Domain.Entities;
using Domain.ValueObjects;
using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using static DatedSchedules.Messages.GetDatedScheduleResponse.Types;

namespace ServiceAPI.Profiles
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

            ////////////////////////////////////////

            CreateMap<ListItemEntity, ScheduleWithChangesListItemDto>()
                .ForMember(dest => dest.Discipline, act => act.MapFrom(src => src.ItemInfo.SubjectName))
                .ForMember(dest => dest.Teacher, act => act.MapFrom(src => src.ItemInfo.TeacherInitials))
                .ForMember(dest => dest.Auditorium, act => act.MapFrom(src => src.ItemInfo.Auditorium))
                .ForMember(dest => dest.Position, act => act.MapFrom(src => src.ItemInfo.Position));

            CreateMap<ScheduleWithChangesListItemDto, ScheduleItem>();

            CreateMap<ScheduleWithChangesDto, GetDatedScheduleResponse>();

            ////////////////////////////////////////

            CreateMap<LessonCallEntity, CallScheduleListItemDto>()
                .ForMember(dest => dest.StartTime, act => act.MapFrom(src => src.StartTime.Ticks))
                .ForMember(dest => dest.EndTime, act => act.MapFrom(src => src.EndTime.Ticks));

            CreateMap<GroupEntity, GroupDto>();

            ////////////////////////////////////////

            CreateMap<ScheduleListItemEntity, ScheduleListItemDto>()
                .ForMember(dest => dest.Teacher, act => act.MapFrom(src => src.ItemInfo.TeacherInitials))
                .ForMember(dest => dest.Position, act => act.MapFrom(src => src.ItemInfo.Position))
                .ForMember(dest => dest.Discipline, act => act.MapFrom(src => src.ItemInfo.SubjectName))
                .ForMember(dest => dest.Auditorium, act => act.MapFrom(src => src.ItemInfo.Auditorium));

            CreateMap<ScheduleListEntity, ScheduleListDto>();

            CreateMap<ScheduleListItemDto, ItemInfo>()
                .ForMember(dest => dest.SubjectName, act => act.MapFrom(src => src.Discipline))
                .ForMember(dest => dest.TeacherInitials, act => act.MapFrom(src => src.Teacher));

            ////////////////////////////////////////

            CreateMap<ChangesListItemDto, ItemInfo>()
                .ForMember(dest => dest.SubjectName, act => act.MapFrom(src => src.SubjectName))
                .ForMember(dest => dest.TeacherInitials, act => act.MapFrom(src => src.TeacherInitials));

            CreateMap<ChangesListItemDto, ChangesListItemEntity>()
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
