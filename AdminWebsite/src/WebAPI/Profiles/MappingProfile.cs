using AutoMapper;
using CallSchedules.Messages;
using ChangesLists.Messages;
using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using Groups.Messages;
using ScheduleLists.Messages;
using System;
using System.Collections.Generic;
using WebAPI.Dtos.CallScheduleLists;
using WebAPI.Dtos.ChangesLists;
using WebAPI.Dtos.Groups;
using WebAPI.Dtos.ScheduleLists;

namespace WebAPI.Profiles
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



            CreateMapsForChangesLists();
            CreateMapsForScheduleLists();
            CreateMapsForGroups();
            CreateMapsForCallSchedules();
        }

        private void CreateMapsForCallSchedules()
        {
            CreateMap<CallSchedules.Messages.DayOfWeek, System.DayOfWeek>()
                .ConvertUsing(src => (System.DayOfWeek)src);

            CreateMap<CallScheduleListItem, CallScheduleListItemDto>()
                .ForMember(dest => dest.StartTimeTicks, act => act.MapFrom(src => src.StartTime.ToTimeSpan().Ticks))
                .ForMember(dest => dest.EndTimeTicks, act => act.MapFrom(src => src.EndTime.ToTimeSpan().Ticks));

            CreateMap<GetCallScheduleListResponse, CallScheduleListDto>();

            CreateMap<CreateCallScheduleListItemDto, CreateCallScheduleListItemRequest>();

            CreateMap<UpdateCallScheduleListItemDto, UpdateCallScheduleListItemRequest>();
        }

        private void CreateMapsForChangesLists()
        {
            CreateMap<GetChangesListByIdResponse, ChangesListDto>()
                .ForMember(dest => dest.Date, act => act.MapFrom(src => src.Date.ToDateTime()));

            CreateMap<ChangesListDto, CreateScheduleChangesListRequest>()
               .ForMember(dest => dest.EducOrgId, act => act.MapFrom(src => src.EducationalOrgId));

            CreateMap<ChangesListItemDto, ChangesListItem>();

            CreateMap<ChangesListItem, ChangesListItemDto>()
                .ForMember(dest => dest.Discipline, act => act.MapFrom(src => src.SubjectName))
                .ForMember(dest => dest.Teacher, act => act.MapFrom(src => src.TeacherInitials));

            CreateMap<CreateChangesListDto, CreateScheduleChangesListRequest>();

            CreateMap<UpdateChangesListInfoDto, UpdateScheduleChangesListRequest>();

            CreateMap<UpdateChangesListItemInfoDto, ChangesListItem>()
                .ForMember(dest => dest.SubjectName, act => act.MapFrom(src => src.Discipline))
                .ForMember(dest => dest.TeacherInitials, act => act.MapFrom(src => src.Teacher));

            CreateMap<CreateChangesListItemDto, ChangesListItem>();

            CreateMap<BriefChangesList, BriefChangesListDto>();
        }

        private void CreateMapsForScheduleLists()
        {
            //CreateMap<ScheduleListItem, ScheduleListItemDto>()
            //    .ForMember(dest => dest.Id, act => act.MapFrom(src => Guid.Parse(src.Id)));

            CreateMap<bool?, BooleanValue>()
                .ConvertUsing(boolean => boolean != null ? new BooleanValue() { Value = (bool)boolean } : null);

            CreateMap<ScheduleListItem, ScheduleListItemDto>()
                .ForMember(dest => dest.IsOddWeek, act => act.MapFrom(src => src.IsOddWeek.Value));

            CreateMap<ScheduleList, ScheduleListDto>();

            CreateMap<ScheduleListItemDto, ScheduleListItem>();

            CreateMap<UpdateScheduleListDto, ScheduleList>()
                .ForMember(dest => dest.DayOfWeek, act => act.MapFrom(src => (ScheduleLists.Messages.DayOfWeek)src.DayOfWeek));
        }

        private void CreateMapsForGroups()
        {
            CreateMap<Group, GroupDto>();

            CreateMap<GroupBrief, GroupBriefDto>();
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
