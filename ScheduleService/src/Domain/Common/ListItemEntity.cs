using Domain.ValueObjects;
using System;

namespace Domain.Common
{
    public class ListItemEntity : Entity
    {
        public ItemInfo ItemInfo { get; private set; }

        protected ListItemEntity() { }
        protected ListItemEntity(ItemInfo itemInfo)
        {
            ItemInfo = itemInfo;
        }

        public void UpdateItemInfo(ItemInfo itemInfo)
        {
            if (itemInfo is null)
                throw new ArgumentNullException(nameof(itemInfo));

            if (itemInfo.Equals(ItemInfo))
                return;

            ItemInfo = itemInfo;
        }
    }
}
