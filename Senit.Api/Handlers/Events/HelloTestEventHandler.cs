using Microsoft.Extensions.Logging;
using Senit.Core.Messaging.Events;
using Senit.Messages.Events;
using System;
using System.Threading.Tasks;

namespace Senit.Api.Handlers.Events
{
    public class HelloTestEventHandler : IEventHandler<HelloEvent>
    {
        private readonly ILogger<HelloTestEventHandler> _logger;

        public HelloTestEventHandler(ILogger<HelloTestEventHandler> logger)
        {
            _logger = logger;
        }

        public Task HandleAsync(HelloEvent @event)
        {
            _logger.LogInformation($"{typeof(HelloEvent).Name} handled from {GetType().Name}.");

            return Task.CompletedTask;
        }

        public Task OnError(HelloEvent @event, Exception ex)
        {
            _logger.LogError($"An error occured while processing message of type '{typeof(HelloEvent).Name}'. Error message: {ex.Message}.");

            return Task.CompletedTask;
        }
    }
}
