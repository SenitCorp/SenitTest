using Senit.Api.Messaging.Commands.Messages;
using Senit.Common.Messaging.Commands;
using System.Threading.Tasks;
using Senit.Common.Messaging;
using Microsoft.Extensions.Logging;

namespace Senit.Api.Messaging.Commands.Handlers
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
            _logger.LogInformation("HelloCommand handled.");

            var response = new HelloCommandResponse();

            return Task.FromResult(response);
        }
    }
}
