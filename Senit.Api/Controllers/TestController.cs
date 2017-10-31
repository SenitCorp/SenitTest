using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RawRabbit;
using RawRabbit.Enrichers.MessageContext;
using Senit.Common.Messaging;
using Senit.Common.Messaging.Commands;
using Senit.Messages.Commands;
using Senit.Messages.Events;
using System;
using System.Threading.Tasks;

namespace Senit.Api.Controllers
{
    [Route("")]
    public class TestController : Controller
    {
        private readonly IBusClient _busClient;
        private readonly ILogger _logger;

        public TestController(IBusClient busClient, ILoggerFactory loggerFactory)
        {
            _busClient = busClient;
            _logger = loggerFactory.CreateLogger<TestController>();
        }

        [HttpGet, Route("")]
        public async Task<IActionResult> Get()
        {
            var messageContext = new MessageContext
            {
                Source = Request.GetDisplayUrl(),
                ExecutionId = Guid.NewGuid().ToString()
            };

            var globalExecutionId = Guid.NewGuid().ToString();

            _logger.LogInformation($"ExecutionId: {messageContext.ExecutionId}");

            var response = await _busClient.RequestAsync<HelloCommand, CommandResponse<HelloCommandResponse>>(new HelloCommand
            {
                CommandId = Guid.NewGuid()
            }, options =>
            {
                options.UseMessageContext(messageContext);
            });

            _logger.LogInformation($"Response: {JsonConvert.SerializeObject(response)}");

            await _busClient.PublishAsync(new HelloEvent
            {
                EventId = Guid.NewGuid()
            }, options =>
            {
                options.UseMessageContext(messageContext);
            });

            return Ok();
        }
    }
}
