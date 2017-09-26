using Senit.Common.Messaging.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Senit.Common.Messages.Events
{
    public class HelloEvent : IEvent
    {
        public Guid EventId { get; set; }
    }
}
