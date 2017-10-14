using Senit.Common.Messaging.Events;
using System;

namespace Senit.Messages.Events
{
    public class HelloEvent : IEvent
    {
        public Guid EventId { get; set; }
    }
}
