using System.Collections.Generic;

using ScheduleChanges.Core.Domain.BaseClasses;

namespace Domain.Entities
{
    public class EducationalOrgEntity : BaseEntity
    {
        public string Name { get; private set; }
        public List<GroupEntity> Groups { get; private set; }
        public List<ChangesListEntity> ChangesLists { get; private set; }
        public List<LessonCallEntity> LessonCalls { get; private set; }

        private EducationalOrgEntity() : base() { }

        public EducationalOrgEntity(string name)
        {
            Name = name;
        }

        public void UpdateName(string name)
        {
            if (name == Name)
                return;

            Name = name;
        }
    }
}
