using Domain.Entities;
using System;
using Xunit;

namespace Domain.Tests.Unit
{
    public class GroupEntityTests
    {
        [Fact]
        public void UpdateGroupInfo_Should_Throws_ArgumentException()
        {
            var group = new GroupEntity(Guid.NewGuid(), "group1", 2);

            Assert.Throws<ArgumentException>(() => group.UpdateGroupInfo("group1", -1));
        }
    }
}
