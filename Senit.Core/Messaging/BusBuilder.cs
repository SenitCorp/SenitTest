using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Senit.Core.Messaging.Commands;
using Senit.Core.Messaging.Events;
using Senit.Core.Messaging.Messages;
using System;
using System.Collections.Generic;

namespace Senit.Core.Messaging
{
    public class BusBuilder
    {
        private readonly IWebHostBuilder _webHostBuilder;
        private readonly Action<IServiceCollection> _busRegistrationFunction;

        private List<(Type EventType, Type EventHandlerType, Type EventHandlerImplementationType)> _events = new List<(Type EventType, Type EventHandlerType, Type EventHandlerImplementation)>();

        private List<(Type MessageType, Type MessageHandlerType, Type MessageHandlerImplementationType)> _messages = new List<(Type MessageType, Type MessageHandlerType, Type MessageHandlerImplementationType)>();

        private Dictionary<(Type Command, Type CommandResponse), Type> _commands = new Dictionary<(Type Command, Type CommandResponse), Type>();

        public BusBuilder(IWebHostBuilder webHostBuilder, Action<IServiceCollection> registrationFunction)
        {
            _webHostBuilder = webHostBuilder;
            _busRegistrationFunction = registrationFunction;
        }

        public BusBuilder AddEventHandler<TEvent, TEventHandler>()
            where TEvent : IEvent
            where TEventHandler : IEventHandler<TEvent>, IEventHandlerDescriptor<TEvent, IEventHandler<TEvent>>
        {
            _events.Add((typeof(TEvent), typeof(IEventHandler<TEvent>), typeof(TEventHandler)));

            return this;
        }

        public BusBuilder AddMessageHandler<TMessage, TMessageHandler>()
            where TMessage : IMessage
            where TMessageHandler : IMessageHandler<TMessage>, IMessageHandlerDescriptor<TMessage, IMessageHandler<TMessage>>
        {
            _messages.Add((typeof(TMessage), typeof(IMessageHandler<TMessage>), typeof(TMessageHandler)));

            return this;
        }

        public BusBuilder AddCommandHandler<TCommand, TCommandResponse, TCommandHandler>()
            where TCommand : ICommand
            where TCommandResponse : ICommandResponse
            where TCommandHandler : ICommandHandler<TCommand, TCommandResponse>
        {
            _commands[(typeof(TCommand), typeof(TCommandResponse))] = typeof(TCommandHandler);

            return this;
        }

        public IWebHost Build()
        {
            //TODO: Document this reflection magic.

            var host = _webHostBuilder
                .ConfigureServices(serviceCollection =>
                {
                    if (_busRegistrationFunction != null)
                    {
                        _busRegistrationFunction.Invoke(serviceCollection);
                    }
                    else
                    {
                        throw new Exception($"The busRegistration function must be provided.");
                    }

                    foreach (var @event in _events)
                    {
                        serviceCollection.AddTransient(typeof(IEventHandlerDescriptor<,>).MakeGenericType(@event.EventType, @event.EventHandlerType), @event.EventHandlerImplementationType);
                    }

                    foreach (var message in _messages)
                    {
                        serviceCollection.AddTransient(typeof(IMessageHandlerDescriptor<,>).MakeGenericType(message.MessageType, message.MessageHandlerType), message.MessageHandlerImplementationType);
                    }

                    foreach (var command in _commands)
                    {
                        serviceCollection.AddTransient(typeof(ICommandHandler<,>).MakeGenericType(command.Key.Command, command.Key.CommandResponse), command.Value);
                    }
                })
                .Build();

            var busClient = host.Services.GetRequiredService<IMessageBus>();

            var addEventHandlerMethodInfo = typeof(IMessageBus).GetMethod(nameof(IMessageBus.AddEventHandler));

            foreach (var @event in _events)
            {
                var generic = addEventHandlerMethodInfo.MakeGenericMethod(@event.EventType, @event.EventHandlerType, @event.EventHandlerImplementationType);

                generic.Invoke(busClient, new object[] { });
            }

            var addMessageHandlerMethodInfo = typeof(IMessageBus).GetMethod(nameof(IMessageBus.AddMessageHandler));

            foreach (var message in _messages)
            {
                var generic = addMessageHandlerMethodInfo.MakeGenericMethod(message.MessageType, message.MessageHandlerType, message.MessageHandlerImplementationType);

                generic.Invoke(busClient, new object[] { });
            }

            var addCommandHandlerMethodInfo = typeof(IMessageBus).GetMethod(nameof(IMessageBus.AddCommandHandler));

            foreach (var command in _commands)
            {
                var generic = addCommandHandlerMethodInfo.MakeGenericMethod(command.Key.Command, command.Key.CommandResponse);

                generic.Invoke(busClient, new object[] { });
            }

            return host;
        }
    }
}
