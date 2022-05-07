using System.Collections.Generic;

namespace Domain.Common
{
    public abstract class ListEntity<TItem> : Entity
        where TItem : ListItemEntity
    {
        public List<TItem> ListItems { get; private set; } = new List<TItem>();

        public abstract void AppendItem(TItem listItem);

        protected ListEntity() { }

        protected ListEntity(List<TItem> listItems)
        {
            ListItems = listItems;
        }
    }
}