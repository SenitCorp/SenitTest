using Senit.Core.Messaging.Commands;
using Senit.Core.Messaging.Events;
using Senit.Core.Messaging.Messages;
using System;
using System.Threading.Tasks;

namespace Senit.Core.Messaging
{
    public interface IMessageBus
    {
        Task PublishAsync<TMessage>(TMessage message) where TMessage : class;

        Task<TResponse> RequestAsync<TRequest, TResponse>(TRequest request) where TRequest : class where TResponse : class;

        Task SubscribeAsync<TMessage>(Func<TMessage, Task> subscribeMethod, Func<TMessage, Exception, Task> onErrorMethod = null) where TMessage : class;

        Task SubscribeAsync<TMessage>(string queueName, Func<TMessage, Task> subscribeMethod, Func<TMessage, Exception, Task> onErrorMethod = null) where TMessage : class;

        void AddEventHandler<TEvent, TEventHandlerType, TImplementationType>()
            where TEvent : class, IEvent
            where TEventHandlerType : IEventHandler<TEvent>
            where TImplementationType : IEventHandler<TEvent>;

        void AddMessageHandler<TMessage, TMessageHandlerType, TImplementationType>()
            where TMessage : class, IMessage
            where TMessageHandlerType : IMessageHandler<TMessage>
            where TImplementationType : IMessageHandler<TMessage>;

        void AddCommandHandler<TCommand, TCommandResponse>()
            where TCommand : class, ICommand
            where TCommandResponse : class, ICommandResponse;
    }
}
