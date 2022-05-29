using System;

namespace Application.Groups.Dtos
{
    public class BriefGroupDto
    {
        public Guid Id { get; init; }
        public string GroupNumber { get; init; }

        public BriefGroupDto(Guid id, string groupNumber)
        {
            Id = id;
            GroupNumber = groupNumber;
        }
    }
}
