using System;

namespace WebAPI.Dtos.Groups
{
    public class GroupDto
    {
        public Guid Id { get; init; }
        public Guid EducOrgId { get; init; }
        public string GroupNumber { get; init; }
        public int YearOfStudy { get; init; }
    }
}