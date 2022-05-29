using System;

namespace WebAPI.Dtos.Groups
{
    public class CreateGroupDto
    {
        public string GroupNumber { get; init; }
        public int YearOfStudy { get; init; }
    }
}
