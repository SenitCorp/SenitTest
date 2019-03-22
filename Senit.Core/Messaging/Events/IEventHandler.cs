using System;
using System.Threading.Tasks;

namespace Senit.Core.Messaging.Events
{
    public interface IEventHandler<TEvent> : IEventHandlerDescriptor<TEvent, IEventHandler<TEvent>> where TEvent : IEvent
    {
        Task HandleAsync(TEvent @event);

        Task OnError(TEvent @event, Exception ex);
    }
}
