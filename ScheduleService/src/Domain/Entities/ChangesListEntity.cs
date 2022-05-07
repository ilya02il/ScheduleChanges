using Domain.Common;
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class ChangesListEntity : ListEntity<ChangesListItemEntity>
    {
        public DateTime Date { get; private set; }
        public bool IsOddWeek { get; private set; }

        public Guid? EducationalOrgId { get; private set; }
        public EducationalOrgEntity EducationalOrg { get; private set; }

        private ChangesListEntity() : base() { }
        public ChangesListEntity(Guid educOrgId, DateTime date, bool isOddWeek, List<ChangesListItemEntity> listItems)
            : base(listItems)
        {
            EducationalOrgId = educOrgId;
            IsOddWeek = isOddWeek;
            Date = date;
        }

        public override void AppendItem(ChangesListItemEntity changesListItem)
        {
            if (changesListItem is null)
                throw new ArgumentNullException(nameof(changesListItem));

            ListItems.Add(changesListItem);
        }
    }
}
