using Domain.Common;
using System.Collections.Generic;

namespace Domain.ValueObjects
{
    public class EducationalInfo : ValueObject
    {
        public string EducOrgName { get; set; }
        public int YearOfStudy { get; set; }
        public string GroupNumber { get; set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return EducOrgName;
            yield return YearOfStudy;
            yield return GroupNumber;
        }
    }
}
