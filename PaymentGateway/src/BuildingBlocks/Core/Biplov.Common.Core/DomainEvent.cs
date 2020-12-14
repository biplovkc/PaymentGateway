using System;

namespace Biplov.Common.Core
{
    /// <summary>
    /// DomainEvents are emitted by Aggregate roots and should be handled by domain event handlers
    /// </summary>
    public class DomainEvent 
    {
        public DomainEvent()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;
        }

        public DomainEvent(Guid id, DateTime creationDate)
        {
            Id = id;
            CreationDate = creationDate;
        }

        public Guid Id { get; }

        public DateTime CreationDate { get; }
    }
}
