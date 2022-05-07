using Domain.Common;
using Domain.ValueObjects;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("ChangesListItems")]
    public class ChangesListItemEntity : ListItemEntity
    {
        public Guid GroupId { get; private set; }
        public GroupEntity Group { get; private set; }

        public Guid ChangesListId { get; private set; }
        public ChangesListEntity ChangesList { get; private set; }

        private ChangesListItemEntity() : base() { }
        public ChangesListItemEntity(Guid changesListId, Guid groupId, ItemInfo itemInfo) : base(itemInfo)
        {
            ChangesListId = changesListId;
            GroupId = groupId;
        }
    }
}
