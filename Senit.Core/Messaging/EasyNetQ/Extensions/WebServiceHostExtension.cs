using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Senit.Core.Messaging;
using System;

namespace Senit.Core.Messaging.EasyNetQ.Extensions
{
    public static partial class WebServiceHostExtension
    {
        public static BusBuilder UseEasyNetQ(this IWebHostBuilder source, Action<IServiceCollection> registrationFunction)
        {
            return new BusBuilder(source, registrationFunction);
        }
    }
}
