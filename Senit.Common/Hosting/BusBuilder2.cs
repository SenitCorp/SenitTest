using Microsoft.AspNetCore.Hosting;
using RawRabbit;
using Senit.Common.Messaging.Commands;
using Senit.Common.Messaging.Events;
using Senit.Common.Messaging.RawRabbit.Extensions;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Senit.Common.Hosting
{
    public class BusBuilder2
    {
        private readonly IWebHost _webHost;
        private readonly IBusClient _busClient;
        private readonly IServiceProvider _serviceProvider;

        private readonly IWebHostBuilder _webHostBuilder;

        private List<TEvent> _events = new List<TEvent>();
        private List<(Type, Type)> _commands = new List<(Type, Type)>();

        public BusBuilder2(IWebHostBuilder webHostBuilder)
        {
            _webHostBuilder = webHostBuilder;
        }

        public BusBuilder2 AddEventHandler<TEvent>() where TEvent : IEvent
        {
            //_busClient.AddEventHandler<TEvent>(_serviceProvider);

            _events.Add(TEvent);

            return this;
        }

        public BusBuilder2 AddCommandHandler<TCommand, TCommandResponse>()
            where TCommand : ICommand
            where TCommandResponse : ICommandResponse
        {
            //_busClient.AddCommandHandler<TCommand, TCommandResponse>(_serviceProvider);

            //_commands.Add((typeof(TCommand), typeof(TCommandResponse)));

            return this;
        }

        public IWebHost Build()
        {
            return _webHostBuilder
                .ConfigureServices(builder =>
                {
                    _events.ForEach(e =>
                    {
                        builder.AddTransient<IEventHandler<Type>>();
                    });
                })
                .Build();
        }
    }
}
