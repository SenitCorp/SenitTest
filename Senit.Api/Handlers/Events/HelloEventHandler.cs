using Microsoft.Extensions.Logging;
using Senit.Core.Messaging.Events;
using Senit.Messages.Events;
using System;
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

            throw new Exception("Unable to process message at this time.");

            //return Task.CompletedTask;
        }

        public Task OnError(HelloEvent @event, Exception ex)
        {
            _logger.LogError($"An error occured while processing message of type '{typeof(HelloEvent).Name}'. Error message: {ex.Message}.");

            return Task.CompletedTask;
        }
    }
}
