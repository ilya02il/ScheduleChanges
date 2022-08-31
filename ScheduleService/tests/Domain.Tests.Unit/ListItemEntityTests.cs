using Domain.Common;
using Domain.ValueObjects;
using System;
using Xunit;

namespace Domain.Tests.Unit
{
    public class ListItemEntityTests
    {
        [Fact]
        public void Ctor_Should_Set_Right_ItemInfo()
        {
            var itemInfo = new ItemInfo
            {
                Position = 1,
                SubjectName = "subject name",
                Auditorium = "aud1",
                TeacherInitials = "teacher initials"
            };
            var item = new TestListItem(itemInfo);

            Assert.Equal(itemInfo, item.ItemInfo);
        }

        [Fact]
        public void UpdateItemInfo_Should_Update_Id()
        {
            var itemInfo1 = new ItemInfo
            {
                Position = 1,
                SubjectName = "subject name",
                Auditorium = "aud1",
                TeacherInitials = "teacher initials"
            };

            var item = new TestListItem(itemInfo1);

            item.UpdateItemInfo(new()
            {
                Position = 2,
                SubjectName = "subject",
                Auditorium = "aud2",
                TeacherInitials = "teacher"
            });

            Assert.Equal(new ItemInfo
            {
                Position = 2,
                SubjectName = "subject",
                Auditorium = "aud2",
                TeacherInitials = "teacher"
            },
            item.ItemInfo);
        }

        [Fact]
        public void UpdateItemInfo_Should_Throw_ArgumentNullException()
        {
            var item = new TestListItem();

            Assert.Throws<ArgumentNullException>(() => item.UpdateItemInfo(null));
        }

        private class TestListItem : ListItemEntity
        {
            public TestListItem() : base() { }
            public TestListItem(ItemInfo itemInfo) : base(itemInfo) { }
        }
    }
}
