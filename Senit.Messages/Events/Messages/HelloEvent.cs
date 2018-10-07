using System;

namespace Senit.Messages.Events
{
    public class HelloEvent : Core.Messaging.Events.IEvent
    {
        public Guid EventId { get; set; }
    }
}
