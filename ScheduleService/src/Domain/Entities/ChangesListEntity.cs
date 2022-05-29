﻿using Domain.Common;
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class ChangesListEntity : ListEntity<ChangesListItemEntity>
    {
        public DateTimeOffset Date { get; private set; }
        public bool IsOddWeek { get; private set; }

        public Guid? EducationalOrgId { get; private set; }
        public EducationalOrgEntity EducationalOrg { get; private set; }

        private ChangesListEntity() : base() { }
        public ChangesListEntity(Guid educOrgId, DateTimeOffset date, bool isOddWeek)
            : base()
        {
            EducationalOrgId = educOrgId;
            IsOddWeek = isOddWeek;
            Date = date;
        }

        public void UpdateListInfo(DateTimeOffset date, bool isOddWeek)
        {
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
