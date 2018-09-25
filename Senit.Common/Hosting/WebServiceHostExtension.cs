using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using RawRabbit;

namespace Senit.Common.Hosting
{
    public static class WebServiceHostExtension
    {
        public static BusBuilder UseRabbitMq(this IWebHostBuilder source)
        {
            var webHost = source.Build();

            var busClient = webHost.Services.GetRequiredService<IBusClient>();

            return new BusBuilder(webHost, busClient, webHost.Services);
        }
    }
}
