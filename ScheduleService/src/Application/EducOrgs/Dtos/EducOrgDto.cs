using Application.Groups.Dtos;
using System;
using System.Collections.Generic;

namespace Application.EducOrgs.Dtos
{
    public class EducOrgDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public IList<GroupDto> Groups { get; init; } = new List<GroupDto>();
    }
}
