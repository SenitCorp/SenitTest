using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RawRabbit;
using Senit.Api.Messaging.Commands.Messages;
using Senit.Api.Messaging.Events.Messages;
using Senit.Common.Messaging;
using Senit.Common.Messaging.Commands;

namespace Senit.Api.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly IBusClient _busClient;

        public ValuesController(IBusClient busClient)
        {
            _busClient = busClient;
        }

        // GET api/values
        [HttpGet]
        public async Task<IEnumerable<string>> Get()
        {
            var response = await _busClient.RequestAsync<HelloCommand, HelloCommandResponse>(new HelloCommand { });

            await _busClient.PublishAsync(new HelloEvent());

            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
