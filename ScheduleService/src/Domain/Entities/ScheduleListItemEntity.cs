using Domain.Common;
using Domain.ValueObjects;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("ScheduleListItems")]
    public class ScheduleListItemEntity : ListItemEntity
    {
        public bool? IsOddWeek { get; private set; }

        public Guid ScheduleListId { get; private set; }
        public ScheduleListEntity ScheduleList { get; private set; }

        private ScheduleListItemEntity() : base() { }
        public ScheduleListItemEntity(Guid scheduleListId, bool? isOddWeek, ItemInfo itemInfo) : base(itemInfo)
        {
            IsOddWeek = isOddWeek;
            ScheduleListId = scheduleListId;
        }
    }
}
