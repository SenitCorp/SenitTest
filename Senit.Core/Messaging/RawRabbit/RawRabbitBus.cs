using Microsoft.Extensions.DependencyInjection;
using RawRabbit;
using RawRabbit.Common;
using RawRabbit.Enrichers.MessageContext;
using RawRabbit.Enrichers.MessageContext.Subscribe;
using Senit.Core.Messaging.Commands;
using Senit.Core.Messaging.Events;
using Senit.Core.Messaging.Exceptions;
using Senit.Core.Messaging.Messages;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Senit.Core.Messaging.RawRabbit
{
    public class RawRabbitBus : IMessageBus
    {
        private readonly IBusClient _busClient;
        private readonly IServiceProvider _serviceProvider;

        public RawRabbitBus(IBusClient busClient, IServiceProvider serviceProvider)
        {
            _busClient = busClient;
            _serviceProvider = serviceProvider.CreateScope().ServiceProvider;
        }

        public async Task PublishAsync<TMessage>(TMessage message) where TMessage : class
        {
            await _busClient.PublishAsync(message);
        }

        public async Task<TResponse> RequestAsync<TRequest, TResponse>(TRequest request)
            where TRequest : class
            where TResponse : class
        {
            return await _busClient.RequestAsync<TRequest, TResponse>(request);
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
                _busClient.SubscribeAsync<TEvent, MessageContext>(async (@event, messageContext) =>
                {
                    try
                    {
                        await eventHandler.HandleAsync(@event);
                        return new Ack();
                    }
                    catch (EventHandlerException)
                    {
                        return new Nack(true);
                    }
                }, ctx => ctx.UseMessageContext(c =>
                {
                    return new MessageContext
                    {
                        RetryInfo = c.GetRetryInformation(),
                    };
                }));
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
                _busClient.SubscribeAsync<TMessage, MessageContext>(async (@event, messageContext) =>
                {
                    try
                    {
                        await messageHandler.HandleAsync(@event);
                        return new Ack();
                    }
                    catch (MessageHandlerException)
                    {
                        return new Nack(true);
                    }
                }, ctx => ctx.UseMessageContext(c =>
                {
                    return new MessageContext
                    {
                        RetryInfo = c.GetRetryInformation(),
                    };
                }));
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

            _busClient.RespondAsync<TCommand, CommandResponse<TCommandResponse>, MessageContext>(async (command, messageContext) =>
            {
                var handler = _serviceProvider.GetRequiredService<ICommandHandler<TCommand, TCommandResponse>>();

                var response = await handler.HandleAsync(command);

                return response;
            });
        }

        public async Task SubscribeAsync<TMessage>(Func<TMessage, Task> subscribeMethod) where TMessage : class
        {
            await _busClient.SubscribeAsync<TMessage>((message) =>
            {
                return subscribeMethod.Invoke(message);
            });
        }

        public async Task SubscribeAsync<TMessage>(string queueName, Func<TMessage, Task> subscribeMethod) where TMessage : class
        {
            await _busClient.SubscribeAsync<TMessage>((message) =>
            {
                return subscribeMethod.Invoke(message);
            }, options =>
            {
                options.UseSubscribeConfiguration(config =>
                {
                    config.FromDeclaredQueue(queue =>
                    {
                        queue.WithName(queueName);
                        queue.WithAutoDelete(true);
                        queue.WithDurability(false);
                    });
                });
            });
        }
    }
}
