using Senit.Common.Messaging.Commands;
using System.Threading.Tasks;
using Senit.Common.Messaging;
using Microsoft.Extensions.Logging;
using Senit.Common.Messages.Commands;

namespace Senit.Api.Handlers.Commands
{
    public class HelloCommandHandler : ICommandHandler<HelloCommand, HelloCommandResponse>
    {
        private readonly ILogger _logger;

        public HelloCommandHandler(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<HelloCommandHandler>();

        }

        public Task<HelloCommandResponse> HandleAsync(HelloCommand command, MessageContext messageContext)
        {
            _logger.LogInformation($"HelloCommand handled from Source: '{messageContext.Source}'.");

            var response = new HelloCommandResponse
            {
                ResponseId = command.CommandId
            };

            return Task.FromResult(response);
        }
    }
}
