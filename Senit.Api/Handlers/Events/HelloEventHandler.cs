using Senit.Common.Messaging.Events;
using System.Threading.Tasks;
using Senit.Common.Messaging;
using Microsoft.Extensions.Logging;
using Senit.Messages.Events;

namespace Senit.Api.Handlers.Events
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
            _logger.LogInformation($"HelloEvent handled from Source: '{messageContext.Source}'. ExecutionId='{messageContext.ExecutionId}'");

            return Task.CompletedTask;
        }
    }
}
