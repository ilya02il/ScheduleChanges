using Domain.Common;
using Xunit;

namespace Domain.Tests.Unit
{
    public class EntityBaseTests
    {
        [Fact]
        public void Entities_Should_Not_Be_Equal()
        {
            var entity1 = new TestEntity();
            var entity2 = new TestEntity();

            Assert.NotEqual(entity1, entity2);
            Assert.True(!entity1.Equals(entity2));
            Assert.True(entity1 != entity2);
            Assert.False(entity1 == entity2);
        }

        [Fact]
        public void Entities_Should_Be_Equal()
        {
            var entity = new TestEntity();

            Assert.Equal(entity, entity);
        }

        [Fact]
        public void HashCodes_Should_Be_Equal()
        {
            var entity = new TestEntity();

            var hashCode1 = entity.GetHashCode();
            var hashCode2 = entity.GetHashCode();

            Assert.Equal(hashCode1, hashCode2);
        }

        private class TestEntity : EntityBase { }
    }
}
