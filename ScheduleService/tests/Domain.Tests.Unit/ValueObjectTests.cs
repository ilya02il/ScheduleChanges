using Domain.Common;
using System.Collections.Generic;
using Xunit;

namespace Domain.Tests.Unit
{
    public class ValueObjectTests
    {
        [Fact]
        public void ValueObjects_Should_Not_Be_Equal()
        {
            var valObj1 = new TestValueObject(1, "first");
            var valObj2 = new TestValueObject(2, "second");
            var valObj3 = new AnotherTestValueObject("third");

            Assert.True(valObj1 != valObj2);
            Assert.True(valObj1 != valObj3);
            Assert.True(valObj1 != null);
        }

        [Fact]
        public void ValueObjects_Should_Be_Equal()
        {
            var valObj1 = new TestValueObject(1, "first");
            var valObj2 = new TestValueObject(1, "first");
            TestValueObject valObj3 = null;
            TestValueObject valObj4 = null;

            Assert.True(valObj1 == valObj2);
            Assert.True(valObj3 == valObj4);
        }

        [Fact]
        public void HashCodes_Should_Be_Equal()
        {
            var valObj1 = new TestValueObject(1, "first");
            var valObj2 = new TestValueObject(1, "first");

            var hashCode1 = valObj1.GetHashCode();
            var hashCode2 = valObj2.GetHashCode();

            Assert.True(hashCode1 == hashCode2);
        }

        private class TestValueObject : ValueObject
        {
            public int Property1 { get; private set; }
            public string Property2 { get; private set; }

            public TestValueObject(int prop1, string prop2)
            {
                Property1 = prop1;
                Property2 = prop2;
            }

            protected override IEnumerable<object> GetEqualityComponents()
            {
                yield return Property1;
                yield return Property2;
            }
        }

        private class AnotherTestValueObject : ValueObject
        {
            public string Property { get; private set; }

            public AnotherTestValueObject(string prop)
            {
                Property = prop;
            }

            protected override IEnumerable<object> GetEqualityComponents()
            {
                yield return Property;
            }
        }
    }
}
