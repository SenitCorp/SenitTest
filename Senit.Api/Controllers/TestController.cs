using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Senit.Core.Messaging;
using Senit.Core.Messaging.Commands;
using Senit.Messages.Commands;
using Senit.Messages.Events;
using System;
using System.Threading.Tasks;

namespace Senit.Api.Controllers
{
    [Route("")]
    public class TestController : Controller
    {
        private readonly IMessageBus _busClient;
        private readonly ILogger<TestController> _logger;

        public TestController(IMessageBus busClient, ILogger<TestController> logger)
        {
            _busClient = busClient;
            _logger = logger;
        }

        [HttpGet, Route("")]
        public async Task<IActionResult> Get()
        {
            var response = await _busClient.RequestAsync<HelloCommand, CommandResponse<HelloCommandResponse>>(new HelloCommand
            {
                CommandId = Guid.NewGuid()
            });

            _logger.LogInformation($"Response: {JsonConvert.SerializeObject(response)}");

            await _busClient.PublishAsync(new HelloEvent
            {
                EventId = Guid.NewGuid()
            });

            return Ok();
        }
    }
}
