using System;

namespace Domain.Common
{
    public abstract class DomainEvent
    {
        protected DomainEvent(DateTimeOffset dateOccurred)
        {
            DateOccurred = dateOccurred;
        }
        public bool IsPublished { get; set; }
        public DateTimeOffset DateOccurred { get; protected set; }
    }
}
