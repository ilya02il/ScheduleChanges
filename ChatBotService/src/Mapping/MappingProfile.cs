﻿using AutoMapper;
using Domain.Dtos;
using DatedSchedules.Messages;
using static DatedSchedules.Messages.GetDatedScheduleResponse.Types;

namespace Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ScheduleItem, ScheduleItemDto>()
                .ForMember(dest => dest.StartTime, act => act.MapFrom(src => src.StartTime.ToTimeSpan()))
                .ForMember(dest => dest.EndTime, act => act.MapFrom(src => src.EndTime.ToTimeSpan()));
            CreateMap<GetDatedScheduleResponse, DatedScheduleDto>()
                .ForMember(dest => dest.Date, act => act.MapFrom(src => src.Date.ToDateTime()))
                .ForMember(dest => dest.EducOrgName, act => act.MapFrom(src => src.EducOrgName))
                .ForMember(dest => dest.GroupNumber, act => act.MapFrom(src => src.GroupNumber));
        }
    }
}
