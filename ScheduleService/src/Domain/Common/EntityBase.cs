using System;

namespace Domain.Common
{
    public abstract class EntityBase
    {
        public virtual Guid Id { get; init; } = Guid.NewGuid();

        public override bool Equals(object obj)
        {
            if (obj is not EntityBase other)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (Id == Guid.Empty || other.Id == Guid.Empty)
                return false;

            return Id == other.Id;
        }

        public static bool operator ==(EntityBase left, EntityBase right)
        {
            if (left is null && right is null)
                return true;

            if (left is null || right is null)
                return false;

            return left.Equals(right);
        }

        public static bool operator !=(EntityBase left, EntityBase right)
        {
            return !left.Equals(right);
        }

        public override int GetHashCode()
        {
            return (GetType().Name + Id).GetHashCode();
        }
    }
}
