using Domain.Common;
using System;

namespace Domain.Entities
{
    public class ScheduleListEntity : ListEntity<ScheduleListItemEntity>
    {
        public DayOfWeek DayOfWeek { get; private set; }

        public Guid GroupId { get; private set; }
        public GroupEntity Group { get; private set; }

        private ScheduleListEntity() : base() { }
        public ScheduleListEntity(Guid groupId, DayOfWeek dayOfWeek) : base()
        {
            GroupId = groupId;
            DayOfWeek = dayOfWeek;
        }

        public void UpdateDayOfWeek(DayOfWeek dayOfWeek)
        {
            DayOfWeek = dayOfWeek;
        }

        public override void AppendItem(ScheduleListItemEntity scheduleListItem)
        {
            if (scheduleListItem is null)
                throw new ArgumentNullException(nameof(scheduleListItem));

            ListItems.Add(scheduleListItem);
        }
    }
}
