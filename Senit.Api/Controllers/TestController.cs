using Microsoft.AspNetCore.Mvc;
using RawRabbit;
using Senit.Common.Messages.Commands;
using Senit.Common.Messages.Events;
using System;
using System.Threading.Tasks;

namespace Senit.Api.Controllers
{
    [Route("")]
    public class TestController : Controller
    {
        private readonly IBusClient _busClient;

        public TestController(IBusClient busClient)
        {
            _busClient = busClient;
        }

        [HttpGet, Route("")]
        public IActionResult Get()
        {
            return Ok();
        }

        [HttpGet, Route("test1")]
        public async Task<IActionResult> Test1()
        {
            var response = await _busClient.RequestAsync<HelloCommand, HelloCommandResponse>(new HelloCommand
            {
                CommandId = Guid.NewGuid()
            });

            await _busClient.PublishAsync(new HelloEvent
            {
                EventId = Guid.NewGuid()
            });
            
            return Ok();
        }

        [HttpGet, Route("test2")]
        public async Task<IActionResult> Test2()
        {
            await _busClient.PublishAsync(new HelloEvent
            {
                EventId = Guid.NewGuid()
            });

            var response = await _busClient.RequestAsync<HelloCommand, HelloCommandResponse>(new HelloCommand
            {
                CommandId = Guid.NewGuid()
            });

            return Ok();
        }

        [HttpGet, Route("test3")]
        public async Task<IActionResult> Test3()
        {
            await _busClient.PublishAsync(new HelloEvent
            {
                EventId = Guid.NewGuid()
            });

            return Ok();
        }

        [HttpGet, Route("test4")]
        public async Task<IActionResult> Test4()
        {
            var response = await _busClient.RequestAsync<HelloCommand, HelloCommandResponse>(new HelloCommand
            {
                CommandId = Guid.NewGuid()
            });

            return Ok();
        }
    }
}
