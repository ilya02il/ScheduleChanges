using ScheduleChanges.Core.Domain.BaseClasses;

namespace ScheduleChanges.Core.Tests.Domain.BaseClasses;

public class BaseEntityTests
{
    private class TestEntity : BaseEntity
    {
        public int TestProp1 { get; set; }
        public int TestProp2 { get; set; }

        public TestEntity() : base() { }
        public TestEntity(Guid id) : base(id) { }
    }

    private class AnotherTestEntity : BaseEntity
    {
        public AnotherTestEntity(Guid id) : base(id) { }
    }

    [Fact]
    public void The_Entities_Should_Be_Eqals()
    {
        var guid = Guid.NewGuid();

        var entity1 = new TestEntity(guid)
        {
            TestProp1 = 1,
            TestProp2 = 1
        };
        var entity2 = new TestEntity(guid)
        {
            TestProp1 = 2,
            TestProp2 = 2
        };
        var entity3 = entity1;

        TestEntity nullEntity1 = null;
        TestEntity nullEntity2 = null;

        entity1.Should().BeEquivalentTo(entity2);
        entity1.Should().BeEquivalentTo(entity3);

        (entity1 == entity2).Should().BeTrue();
        (entity1 == entity3).Should().BeTrue();

        (nullEntity1 == nullEntity2).Should().BeTrue();
    }

    [Fact]
    public void The_Entities_Should_Be_Not_Eqals()
    {
        var guid1 = Guid.NewGuid();

        var entity1 = new TestEntity(guid1)
        {
            TestProp1 = 1,
            TestProp2 = 2
        };
        var entity2 = new TestEntity()
        {
            TestProp1 = 1,
            TestProp2 = 2
        };
        var entity3 = new AnotherTestEntity(guid1);

        var entity4 = new TestEntity();

        entity1.Should().NotBeEquivalentTo(entity2);
        entity1.Should().NotBeEquivalentTo(entity3);
        entity1.Should().NotBeEquivalentTo(entity4);

        (entity1 != entity2).Should().BeTrue();
        (entity1 != entity3).Should().BeTrue();
        (entity1 != entity4).Should().BeTrue();

        (entity1 == null && null == entity1).Should().BeFalse();
    }

    [Fact]
    public void The_Entities_HashCodes_Should_Be_Eqals()
    {
        var guid = Guid.NewGuid();

        var entity1 = new TestEntity(guid)
        {
            TestProp1 = 1,
            TestProp2 = 1
        };

        var entity2 = new TestEntity(guid)
        {
            TestProp1 = 2,
            TestProp2 = 2
        };

        (entity1.GetHashCode() == entity2.GetHashCode()).Should().BeTrue();
    }

    [Fact]
    public void The_Entities_HashCodes_Should_Not_Be_Eqals()
    {
        var guid1 = Guid.NewGuid();

        var entity1 = new TestEntity(guid1);
        var entity2 = new TestEntity();
        var entity3 = new AnotherTestEntity(guid1);

        (entity1.GetHashCode() == entity2.GetHashCode()).Should().BeFalse();
        (entity1.GetHashCode() == entity3.GetHashCode()).Should().BeFalse();
    }
}
