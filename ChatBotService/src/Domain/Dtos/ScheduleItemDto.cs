using ProtoBuf;
using System;

namespace Domain.Dtos
{
    [ProtoContract]
    public class ScheduleItemDto
    {
        [ProtoMember(1)]
        public int Position { get; init; }
        [ProtoMember(2)]
        public TimeSpan StartTime { get; init; }
        [ProtoMember(3)]
        public TimeSpan EndTime { get; init; }
        [ProtoMember(4)]
        public string Discipline { get; init; }
        [ProtoMember(5)]
        public string Teacher { get; init; }
        [ProtoMember(6)]
        public string Auditorium { get; init; }

        public ScheduleItemDto() { } 
    }
}