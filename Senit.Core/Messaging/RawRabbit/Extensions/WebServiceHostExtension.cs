using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using RawRabbit;
using RawRabbit.Configuration;
using System;
using System.Threading.Tasks;

namespace Senit.Core.Messaging.RawRabbit.Extensions
{
    public static class WebServiceHostExtension
    {
        public static BusBuilder UseRabbitMq(this IWebHostBuilder source, Action<IServiceCollection> registrationFunction)
        {
            return new BusBuilder(source, registrationFunction);
        }
    }
}
