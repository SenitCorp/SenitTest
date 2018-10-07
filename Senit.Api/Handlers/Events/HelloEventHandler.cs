﻿using Microsoft.Extensions.Logging;
using Senit.Core.Messaging.Events;
using Senit.Messages.Events;
using System.Threading.Tasks;

namespace Senit.Api.Handlers.Events
{
    public class HelloEventHandler : IEventHandler<HelloEvent>
    {
        private readonly ILogger<HelloEventHandler> _logger;

        public HelloEventHandler(ILogger<HelloEventHandler> logger)
        {
            _logger = logger;
        }

        public Task HandleAsync(HelloEvent @event)
        {
            _logger.LogInformation($"HelloEvent handled from {GetType().Name}.");

            return Task.CompletedTask;
        }
    }
}
