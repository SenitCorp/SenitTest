using Microsoft.Extensions.Logging;
using Senit.Core.Messaging.Commands;
using Senit.Core.Messaging.RawRabbit;
using Senit.Messages.Commands;
using System.Threading.Tasks;

namespace Senit.Api.Handlers.Commands
{
    public class HelloCommandHandler : ICommandHandler<HelloCommand, HelloCommandResponse>
    {
        private readonly ILogger<HelloCommandHandler> _logger;

        public HelloCommandHandler(ILogger<HelloCommandHandler> logger)
        {
            _logger = logger;

        }

        public Task<CommandResponse<HelloCommandResponse>> HandleAsync(HelloCommand command)
        {
            _logger.LogInformation($"HelloCommand handled from {GetType().Name}.");

            var response = new HelloCommandResponse
            {
                ResponseId = command.CommandId
            };

            return Task.FromResult(new CommandResponse<HelloCommandResponse>(response));
        }
    }
}
