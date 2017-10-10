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

        public BusBuilder AddEventHandler<TEvent, TEventHandler>() where TEvent : IEvent where TEventHandler : IEventHandler<TEvent>
        {
            _busClient.AddEventHandler<TEvent, TEventHandler>(_serviceProvider);

            return this;
        }

        public BusBuilder AddCommandHandler<TCommand, TCommandResponse, TCommandHandler>()
            where TCommand : ICommand
            where TCommandResponse : ICommandResponse
            where TCommandHandler : ICommandHandler<TCommand, TCommandResponse>
        {
            _busClient.AddCommandHandler<TCommand, TCommandResponse, TCommandHandler>(_serviceProvider);

            return this;
        }

        public WebServiceHost Build()
        {
            return new WebServiceHost(_webHost);
        }
    }
}
