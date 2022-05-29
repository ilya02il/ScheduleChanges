using ProtoBuf;
using System;
using System.Collections.Generic;

namespace Domain.Dtos
{
    [ProtoContract]
    public class DatedScheduleDto
    {
        [ProtoMember(1)]
        public string EducOrgName { get; init; }
        [ProtoMember(2)]
        public string GroupNumber { get; init; }
        [ProtoMember(3)]
        public DateTimeOffset Date { get; init; }
        [ProtoMember(4)]
        public DateTimeOffset ExpirationTime { get; init; }
        [ProtoMember(5)]
        public List<ScheduleItemDto> ScheduleItems { get; init; }
    }
}