using System;
using ScheduleChanges.Core.Domain.BaseClasses;

namespace Domain.Entities
{
    public class GroupEntity : BaseEntity
    {
        public string GroupNumber { get; private set; }
        public int YearOfStudy { get; private set; }

        public Guid EducationalOrgId { get; private set; }
        public EducationalOrgEntity EducationalOrg { get; private set; }

        private GroupEntity() : base() { }

        public GroupEntity(Guid educOrgId, string groupNumber, int yearOfStudy)
        {
            EducationalOrgId = educOrgId;
            GroupNumber = groupNumber;
            YearOfStudy = yearOfStudy;
        }

        public void UpdateGroupInfo(string groupNumber, int yearOfStudy)
        {
            if (!(yearOfStudy > 0))
                throw new ArgumentException("Year of study shoul be greater than 0");

            GroupNumber = groupNumber;
            YearOfStudy = yearOfStudy;
        }
    }
}