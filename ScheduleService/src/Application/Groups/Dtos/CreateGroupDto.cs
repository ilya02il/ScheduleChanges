using System;

namespace Application.Groups.Dtos
{
    public class CreateGroupDto
    {
        public Guid EducOrgId { get; init; }
        public string GroupNumber { get; init; }
        public int YearOfStudy { get; init; }
    }
}
