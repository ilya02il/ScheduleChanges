using System;
using System.Collections.Generic;

using ScheduleChanges.Core.Domain.BaseClasses;

namespace Domain.Common
{
    public abstract class ListEntity<TItem> : BaseEntity
        where TItem : ListItemEntity
    {
        public List<TItem> ListItems { get; private set; } = new List<TItem>();

        public virtual void AppendItem(TItem listItem)
        {
            if (listItem is null)
                throw new ArgumentNullException(nameof(listItem));

            ListItems.Add(listItem);
        }

        public virtual void AppendItems(IEnumerable<TItem> listItems)
        {
            if (listItems is null)
                throw new ArgumentNullException(nameof(listItems));

            foreach (var item in listItems)
                AppendItem(item);
        }

        protected ListEntity() { }

        protected ListEntity(List<TItem> listItems)
        {
            ListItems = listItems;
        }
    }
}