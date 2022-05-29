using System;

namespace Application.ScheduleLists.Dtos
{
    public class ScheduleListItemDto
    {
        public Guid Id { get; init; }
        public int Position { get; init; }
        public bool? IsOddWeek { get; init; }
        public string Discipline { get; init; }
        public string Auditorium { get; init; }
        public string Teacher { get; init; }

        public ScheduleListItemDto(Guid id, bool? isOddWeek)
        {
            Id = id;
            IsOddWeek = isOddWeek;
        }
    }
}