using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using RawRabbit;
using System;

namespace Senit.Common.Hosting
{
    public class Builder
    {
        private readonly IWebHost _webHost;
        private IBusClient _busClient;
        private readonly IServiceProvider _serviceProvider;

        public Builder(IWebHost webHost, IServiceProvider serviceProvider)
        {
            _webHost = webHost;
            _serviceProvider = serviceProvider;
        }

        public BusBuilder UseRabbitMq()
        {
            _busClient = _serviceProvider.GetRequiredService<IBusClient>();

            return new BusBuilder(_webHost, _busClient, _serviceProvider);
        }

        public WebServiceHost Build()
        {
            return new WebServiceHost(_webHost);
        }
    }
}
