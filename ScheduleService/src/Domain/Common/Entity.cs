using System;

namespace Domain.Common
{
    public abstract class Entity
    {
        public virtual Guid Id { get; init; } = Guid.NewGuid();

        public override bool Equals(object obj)
        {
            if (obj is not Entity other)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (Id == Guid.Empty || other.Id == Guid.Empty)
                return false;

            return Id == other.Id;
        }

        public static bool operator ==(Entity left, Entity right)
        {
            if (left is null && right is null)
                return true;

            if (left is null || right is null)
                return false;

            return left.Equals(right);
        }

        public static bool operator !=(Entity left, Entity right)
        {
            return !left.Equals(right);
        }

        public override int GetHashCode()
        {
            return (GetType().Name + Id).GetHashCode();
        }
    }
}
