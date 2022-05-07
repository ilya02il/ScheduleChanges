using Domain.Common;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class EducationalOrgEntity : Entity
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
    }
}
