using Domain.Common;
using System;
using System.Collections.Generic;
using Xunit;

namespace Domain.Tests.Unit
{
    public class ListEntityTests
    {
        [Fact]
        public void ListItems_Should_Contains_ListItem_After_It_Has_Been_Appended()
        {
            var list1 = new TestList();
            var list2 = new TestList();
            var listItem = new TestListItem();
            var list3 = new TestList(new() { listItem });

            list1.AppendItem(listItem);
            list2.AppendItems(new [] {listItem});

            Assert.Contains(listItem, list1.ListItems);
            Assert.Contains(listItem, list2.ListItems);
            Assert.Contains(listItem, list3.ListItems);
        }

        [Fact]
        public void AppendItem_Should_Throw_ArgumentNullException()
        {
            var list1 = new TestList();

            Assert.Throws<ArgumentNullException>(() => list1.AppendItem(null));
            Assert.Throws<ArgumentNullException>(() => list1.AppendItems(null));
            Assert.Throws<ArgumentNullException>(() => list1.AppendItems(new TestListItem[] { null }));
        }

        private class TestList : ListEntity<TestListItem>
        {
            public TestList() : base() { }

            public TestList(List<TestListItem> listItems) : base(listItems) { }
        }

        private class TestListItem : ListItemEntity { }
    }
}
