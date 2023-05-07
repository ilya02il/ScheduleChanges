using ScheduleChanges.Core.Domain.Attributes;
using ScheduleChanges.Core.Domain.BaseClasses;

namespace ScheduleChanges.Core.Tests.Domain.BaseClasses;

public class BaseValueObjectTests
{
    private class TestValueObject : BaseValueObject<TestValueObject>
    {
        public int SomeProp1 { get; }
        public int SomeProp2 { get; }

        public TestValueObject(int someProp1, int someProp2)
        {
            SomeProp1 = someProp1;
            SomeProp2 = someProp2;
        }
    }

    private class AnotherTestValueObject : BaseValueObject<AnotherTestValueObject>
    {
        public int SomeAnotherProp1 { get; }
        public int SomeAnotherProp2 { get; }

        public AnotherTestValueObject(int someAnotherProp1, int someAnotherProp2)
        {
            SomeAnotherProp1 = someAnotherProp1;
            SomeAnotherProp2 = someAnotherProp2;
        }
    }

    private class TestValueObjectWithIgnoredProp : BaseValueObject<TestValueObjectWithIgnoredProp>
    {
        public int Prop { get; set; }

        [NotIncludeToEqualityComponents]
        public int IgnoredProp { get; set; }

        public TestValueObjectWithIgnoredProp(int prop, int ignoredProp)
        {
            Prop = prop;
            IgnoredProp = ignoredProp;
        }
    }

    [Fact]
    public void ValueObjects_Should_Be_Equals()
    {
        var valObj1 = new TestValueObject(1, 2);
        var valObj2 = new TestValueObject(1, 2);
        var valObj3 = new AnotherTestValueObject(1, 2);
        var valObj4 = new AnotherTestValueObject(1, 2);

        valObj1.Should().BeEquivalentTo(valObj2);
        valObj3.Should().BeEquivalentTo(valObj4);

        (valObj1 == valObj2).Should().BeTrue();
        (valObj3 == valObj4).Should().BeTrue();
    }

    [Fact]
    public void ValueObjects_Should_Not_Be_Equals()
    {
        var valObj1 = new TestValueObject(1, 1);
        var valObj2 = new TestValueObject(1, 2);
        var valObj3 = new AnotherTestValueObject(1, 1);

        valObj1.Should().NotBeEquivalentTo(valObj2);
        valObj1.Should().NotBeEquivalentTo(valObj3);

        (valObj1 != valObj2).Should().BeTrue();
    }

    [Fact]
    public void ValueObjects_HashCodes_Should_Be_Equals()
    {
        var valObj1 = new TestValueObject(1, 2);
        var valObj2 = new TestValueObject(1, 2);
        var valObj3 = new AnotherTestValueObject(1, 3);
        var valObj4 = new AnotherTestValueObject(1, 3);

        (valObj1.GetHashCode() == valObj2.GetHashCode()).Should().BeTrue();
        (valObj3.GetHashCode() == valObj4.GetHashCode()).Should().BeTrue();
    }
    [Fact]
    public void ValueObjects_HashCodes_Should_Not_Be_Equals()
    {
        var valObj1 = new TestValueObject(1, 1);
        var valObj2 = new TestValueObject(1, 2);
        var valObj3 = new AnotherTestValueObject(1, 1);

        (valObj1.GetHashCode() != valObj2.GetHashCode()).Should().BeTrue();
        (valObj1.GetHashCode() != valObj3.GetHashCode()).Should().BeTrue();
    }

    [Fact]
    public void ValueObject_EqualOperator_Should_Ignore_Marked_Property_And_Return_True()
    {
        var valObj1 = new TestValueObjectWithIgnoredProp(1, 2);
        var valObj2 = new TestValueObjectWithIgnoredProp(1, 1);
        var valObj3 = new TestValueObjectWithIgnoredProp(1, 2);

        valObj1.Should().BeEquivalentTo(valObj2);
        valObj1.Should().BeEquivalentTo(valObj3);
    }
}
