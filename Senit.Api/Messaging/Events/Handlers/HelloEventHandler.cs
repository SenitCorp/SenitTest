using Senit.Api.Messaging.Events.Messages;
using Senit.Common.Messaging.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Senit.Common.Messaging;
using Microsoft.Extensions.Logging;

namespace Senit.Api.Messaging.Events.Handlers
{
    public class HelloEventHandler : IEventHandler<HelloEvent>
    {
        private readonly ILogger _logger;

        public HelloEventHandler(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<HelloEventHandler>();
        }

        public Task HandleAsync(HelloEvent @event, MessageContext messageContext)
        {
            _logger.LogInformation("HelloEvent handled.");

            return Task.FromResult(0);
        }
    }
}
