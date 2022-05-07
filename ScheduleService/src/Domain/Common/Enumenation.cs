using System;

namespace Domain.Common
{
    public abstract class Enumenation : IComparable
    {
        public int Id { get; private set; }
        public string Name { get; private set; }

        protected Enumenation(int id, string name) => (Id, Name) = (id, name);

        public override string ToString() => Name;

        public override bool Equals(object obj)
        {
            if (obj is not Enumenation other)
                return false;

            var typeMatches = GetType().Equals(other.GetType());
            var valueMatcher = Id.Equals(other.Id);

            return typeMatches && valueMatcher;
        }

        public static bool operator ==(Enumenation left, Enumenation right)
        {
            if (left is null && right is null)
                return true;

            if (left is null || right is null)
                return false;

            return left.Equals(right);
        }

        public static bool operator !=(Enumenation left, Enumenation right)
        {
            return !left.Equals(right);
        }

        public int CompareTo(object obj)
        {
            var other = (Enumenation)obj;
            return Id.CompareTo(other.Id);
        }

        public override int GetHashCode()
        {
            return (Id.ToString() + Name).GetHashCode();
        }
    }
}
