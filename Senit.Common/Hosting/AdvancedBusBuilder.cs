using Microsoft.AspNetCore.Hosting;
using RawRabbit;
using Senit.Common.Messaging.Commands;
using Senit.Common.Messaging.Events;
using Senit.Common.Messaging.RawRabbit.Extensions;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RawRabbit.Configuration;

namespace Senit.Common.Hosting
{
    public class AdvancedBusBuilder
    {
        private readonly IWebHostBuilder _webHostBuilder;
        private readonly RawRabbitConfiguration _configuration;

        private Dictionary<Type, Type> _events = new Dictionary<Type, Type>();
        private Dictionary<(Type Command, Type CommandResponse), Type> _commands = new Dictionary<(Type Command, Type CommandResponse), Type>();

        public AdvancedBusBuilder(IWebHostBuilder webHostBuilder, RawRabbitConfiguration configuration)
        {
            _webHostBuilder = webHostBuilder;
            _configuration = configuration;
        }

        public AdvancedBusBuilder AddEventHandler<TEvent, TEventHandler>() where TEvent : IEvent where TEventHandler : IEventHandler<TEvent>
        {
            _events.Add(typeof(TEvent), typeof(TEventHandler));

            return this;
        }

        public AdvancedBusBuilder AddCommandHandler<TCommand, TCommandResponse, TCommandHandler>()
            where TCommand : ICommand
            where TCommandResponse : ICommandResponse
            where TCommandHandler : ICommandHandler<TCommand, TCommandResponse>
        {
            _commands.Add((typeof(TCommand), typeof(TCommandResponse)), typeof(TCommandHandler));

            return this;
        }

        public IWebHost Build()
        {
            var host = _webHostBuilder
                .ConfigureServices(serviceCollection =>
                {
                    serviceCollection.AddRawRabbit(_configuration);

                    foreach(var @event in _events)
                    {
                        serviceCollection.AddTransient(typeof(IEventHandler<>).MakeGenericType(@event.Key), @event.Value);
                    }

                    foreach(var command in _commands)
                    {
                        serviceCollection.AddTransient(typeof(ICommandHandler<,>).MakeGenericType(command.Key.Command, command.Key.CommandResponse), command.Value);
                    }
                })
                .Build();

            var busClient = host.Services.GetRequiredService<IBusClient>();


            foreach (var @event in _events)
            {
                var methodInfo = typeof(BusClientExtension).GetMethod(nameof(BusClientExtension.AddEventHandler));

                var generic = methodInfo.MakeGenericMethod(@event.Key);

                generic.Invoke(busClient, new object[] { busClient, host.Services });
            }

            foreach (var command in _commands)
            {
                var methodInfo = typeof(BusClientExtension).GetMethod(nameof(BusClientExtension.AddCommandHandler));

                var generic = methodInfo.MakeGenericMethod(command.Key.Command, command.Key.CommandResponse);

                generic.Invoke(busClient, new object[] { busClient, host.Services });
            }

            return host;
        }
    }
}
