using Microsoft.Extensions.Logging;
using Senit.Core.Messaging.Events;
using Senit.Messages.Events;
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
            _logger.LogInformation($"HelloEvent handled from {GetType().Name}.");

            return Task.CompletedTask;
        }
    }
}
