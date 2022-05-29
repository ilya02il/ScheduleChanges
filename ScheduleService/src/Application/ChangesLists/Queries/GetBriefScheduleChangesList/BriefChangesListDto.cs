using System;

namespace Application.ChangesLists.Queries.GetBriefScheduleChangesList
{
    public class BriefChangesListDto
    {
        public Guid Id { get; init; }
        public DateTimeOffset Date { get; init; }

        public BriefChangesListDto(Guid id, DateTimeOffset date)
        {
            Id = id;
            Date = date;
        }
    }
}