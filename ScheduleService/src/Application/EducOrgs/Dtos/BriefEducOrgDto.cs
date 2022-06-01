using System;

namespace Application.EducOrgs.Dtos
{
    public class BriefEducOrgDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; }

        public BriefEducOrgDto(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
