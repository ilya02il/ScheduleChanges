namespace ScheduleChanges.Core.Domain.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class NotIncludeToEqualityComponentsAttribute : Attribute
{
    public NotIncludeToEqualityComponentsAttribute() { }
}
