using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;
using Senit.Core.Messaging.Commands;
using Senit.Core.Messaging.Events;
using Senit.Core.Messaging.Messages;
using System;
using System.Linq;
using System.Threading.Tasks;
using IMessage = Senit.Core.Messaging.Messages.IMessage;

namespace Senit.Core.Messaging.EasyNetQ
{
    public class EasyNetQBus : IMessageBus
    {
        private readonly IBus _bus;
        private readonly IServiceProvider _serviceProvider;

        public EasyNetQBus(IBus bus, IServiceProvider serviceProvider)
        {
            _bus = bus;

            _serviceProvider = serviceProvider.CreateScope().ServiceProvider;
        }

        public async Task PublishAsync<TMessage>(TMessage message) where TMessage : class
        {
            await _bus.PublishAsync(message);
        }

        public async Task<TResponse> RequestAsync<TRequest, TResponse>(TRequest request) where TRequest : class where TResponse : class
        {
            return await _bus.RequestAsync<TRequest, TResponse>(request);
        }

        public void AddEventHandler<TEvent, TEventHandlerType, TImplementationType>()
            where TEvent : class, IEvent
            where TEventHandlerType : IEventHandler<TEvent>
            where TImplementationType : IEventHandler<TEvent>
        {
            var eventHandlerServices = _serviceProvider.GetServices<IEventHandlerDescriptor<TEvent, TEventHandlerType>>();

            var eventHandler = eventHandlerServices.FirstOrDefault(service => service.GetType() == typeof(TImplementationType)) as IEventHandler<TEvent>;

            if (eventHandler != null)
            {
                _bus.SubscribeAsync<TEvent>(typeof(TImplementationType).Name, async (@event) =>
                {
                    await eventHandler.HandleAsync(@event);
                });
            }
            else
            {
                throw new Exception($"No event handler was found for event type '{typeof(TEvent).Name}'.");
            }
        }

        public void AddMessageHandler<TMessage, TMessageHandlerType, TImplementationType>()
            where TMessage : class, IMessage
            where TMessageHandlerType : IMessageHandler<TMessage>
            where TImplementationType : IMessageHandler<TMessage>
        {
            var messageHandlerServices = _serviceProvider.GetServices<IMessageHandlerDescriptor<TMessage, TMessageHandlerType>>();

            var messageHandler = messageHandlerServices.FirstOrDefault(service => service.GetType() == typeof(TImplementationType)) as IMessageHandler<TMessage>;

            if (messageHandler != null)
            {
                _bus.SubscribeAsync<TMessage>(typeof(TImplementationType).Name, async (message) =>
                {
                    await messageHandler.HandleAsync(message);
                });
            }
            else
            {
                throw new Exception($"No message handler was found for message type '{typeof(TMessage).Name}'.");
            }
        }

        public void AddCommandHandler<TCommand, TCommandResponse>()
            where TCommand : class, ICommand
            where TCommandResponse : class, ICommandResponse
        {

            _bus.RespondAsync<TCommand, CommandResponse<TCommandResponse>>(async (command) =>
            {
                var handler = _serviceProvider.GetRequiredService<ICommandHandler<TCommand, TCommandResponse>>();

                var response = await handler.HandleAsync(command);

                return response;
            });
        }

        public Task SubscribeAsync<TMessage>(Func<TMessage, Task> subscribeMethod) where TMessage : class
        {
            _bus.SubscribeAsync<TMessage>(typeof(TMessage).Name, (message) =>
            {
                return subscribeMethod.Invoke(message);
            });

            return Task.CompletedTask;
        }

        public Task SubscribeAsync<TMessage>(string queueName, Func<TMessage, Task> subscribeMethod) where TMessage : class
        {
            _bus.SubscribeAsync<TMessage>(typeof(TMessage).Name, (message) =>
            {
                return subscribeMethod.Invoke(message);
            }, options =>
            {
                options.WithQueueName(queueName);
                options.WithAutoDelete(true);
                options.WithDurable(false);
            });

            return Task.CompletedTask;
        }
    }
}
