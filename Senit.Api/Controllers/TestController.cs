using Microsoft.AspNetCore.Mvc;
using RawRabbit;
using Senit.Api.Messaging.Commands.Messages;
using Senit.Api.Messaging.Events.Messages;
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
        public async Task<IActionResult> Get()
        {
            return Ok();
        }

        [HttpGet, Route("test1")]
        public async Task<IActionResult> Test1()
        {
            var response = await _busClient.RequestAsync<HelloCommand, HelloCommandResponse>(new HelloCommand { });

            await _busClient.PublishAsync(new HelloEvent());
            
            return Ok();
        }

        [HttpGet, Route("test2")]
        public async Task<IActionResult> Test2()
        {
            await _busClient.PublishAsync(new HelloEvent());

            var response = await _busClient.RequestAsync<HelloCommand, HelloCommandResponse>(new HelloCommand { });

            return Ok();
        }

        [HttpGet, Route("test3")]
        public async Task<IActionResult> Test3()
        {
            await _busClient.PublishAsync(new HelloEvent());

            return Ok();
        }

        [HttpGet, Route("test4")]
        public async Task<IActionResult> Test4()
        {
            var response = await _busClient.RequestAsync<HelloCommand, HelloCommandResponse>(new HelloCommand { });

            return Ok();
        }
    }
}
