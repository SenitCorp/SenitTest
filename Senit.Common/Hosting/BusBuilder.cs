using Microsoft.AspNetCore.Hosting;
using RawRabbit;
using Senit.Common.Messaging.Commands;
using Senit.Common.Messaging.Events;
using Senit.Common.Messaging.RawRabbit.Extensions;
using System;

namespace Senit.Common.Hosting
{
    public class BusBuilder
    {
        private readonly IWebHost _webHost;
        private readonly IBusClient _busClient;
        private readonly IServiceProvider _serviceProvider;

        public BusBuilder(IWebHost webHost, IBusClient busClient, IServiceProvider serviceProvider)
        {
            _webHost = webHost;
            _busClient = busClient;
            _serviceProvider = serviceProvider;
        }

        public BusBuilder AddEventHandler<TEvent>() where TEvent : IEvent
        {
            _busClient.AddEventHandler<TEvent>(_serviceProvider);

            return this;
        }

        public BusBuilder AddCommandHandler<TCommand, TCommandResponse>()
            where TCommand : ICommand
            where TCommandResponse : ICommandResponse
        {
            _busClient.AddCommandHandler<TCommand, TCommandResponse>(_serviceProvider);

            return this;
        }

        public IWebHost Build()
        {
            return _webHost;
        }
    }
}
